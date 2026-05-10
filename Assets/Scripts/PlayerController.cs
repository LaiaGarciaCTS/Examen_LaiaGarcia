using UnityEngine;
using UnityEngine.InputSystem;
 
//  PLAYER CONTROLLER
 
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
 
    [Header("Posicion inicial")]
    public Vector3 startPosition;
 
    [Header("Salud")]
    public int vidasMaximas = 3;
 
    [Header("Limite de caida — ajustar en Inspector")]
    public float limiteCaida = -10f;
 
    [Header("Ataque")]
    public Transform puntoAtaque;
    public float radioAtaque = 0.5f;
    public int danioAtaque = 1;
    public LayerMask capaEnemigos;
    public float cooldownAtaque = 0.4f;
 
    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoDaño;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoLlave;
 
    //  PRIVADO
    private Rigidbody2D rBody2D;
    private SpriteRenderer sr;
    private Animator animator;
    private AudioSource _audioSource;
    private GroundSensor sensor;
 
    private InputAction moveAction;
    private InputAction jumpAction;
 
    private Vector2 moveDirection;
    private int vidasActuales;
    private bool estaMuerto = false;
    private float timerAtaque = 0f;
    private float timerInvulnerabilidad = 0f;
    private bool esInvulnerable = false;
 
    //  AWAKE
    void Awake()
    {
        rBody2D      = GetComponent<Rigidbody2D>();
        sr           = GetComponent<SpriteRenderer>();
        animator     = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        sensor       = GetComponentInChildren<GroundSensor>();
 
        moveAction = InputSystem.actions["Move"];
        jumpAction = InputSystem.actions["Jump"];
    }
 
    //  START
    void Start()
    {
        transform.position = startPosition;
        vidasActuales = vidasMaximas;
        UIManager.Instancia?.ActualizarVidas(vidasActuales);
 
        Debug.Log("PLAYER: limiteCaida = " + limiteCaida +
            " | Posicion inicial Y = " + startPosition.y +
            " | Ajusta limiteCaida en el Inspector si hace falta.");
    }
 
    //  UPDATE
    void Update()
    {
        if (estaMuerto) return;
 
        timerAtaque -= Time.deltaTime;
 
        // Movimiento
        moveDirection = moveAction.ReadValue<Vector2>();
 
        if (moveDirection.x > 0)      { sr.flipX = false; animator.SetBool("IsRunning", true); }
        else if (moveDirection.x < 0) { sr.flipX = true;  animator.SetBool("IsRunning", true); }
        else                          { animator.SetBool("IsRunning", false); }
 
        // Salto
        if (jumpAction.WasPressedThisFrame() && sensor.IsGrounded())
        {
            rBody2D.linearVelocity = new Vector2(rBody2D.linearVelocity.x, jumpForce);
            ReproducirSonido(sonidoSalto);
        }
 
        animator.SetBool("IsJumping", !sensor.IsGrounded());
 
        // Ataque
        if (Input.GetKeyDown(KeyCode.Z) && timerAtaque <= 0f)
        {
            timerAtaque = cooldownAtaque;
            ReproducirSonido(sonidoAtaque);
            if (puntoAtaque != null)
            {
                Collider2D[] golpeados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, capaEnemigos);
                foreach (Collider2D col in golpeados)
                    col.GetComponent<EnemyController>()?.RecibirDaño(danioAtaque);
            }
        }
 
        // Caída al vacío — debug para ver el Y actual en consola mientras cae
        if (transform.position.y < limiteCaida)
        {
            Debug.Log("PLAYER: Caída detectada en Y=" + transform.position.y + " | Llamando a Morir()");
            Morir();
        }
 
        // Invulnerabilidad
        if (esInvulnerable)
        {
            timerInvulnerabilidad -= Time.deltaTime;
            sr.enabled = (Mathf.Sin(Time.time * 20f) > 0f);
            if (timerInvulnerabilidad <= 0f)
            {
                esInvulnerable = false;
                sr.enabled = true;
            }
        }
    }
 
    //  FIXED UPDATE
    void FixedUpdate()
    {
        if (estaMuerto) return;
        rBody2D.linearVelocity = new Vector2(moveDirection.x * movementSpeed, rBody2D.linearVelocity.y);
    }
 
    //  API PÚBLICA
    public void RecibirDaño(int cantidad = 1)
    {
        if (estaMuerto || esInvulnerable) return;
 
        vidasActuales -= cantidad;
        ReproducirSonido(sonidoDaño);
        UIManager.Instancia?.ActualizarVidas(vidasActuales);
 
        if (vidasActuales <= 0)
            Morir();
        else
        {
            esInvulnerable = true;
            timerInvulnerabilidad = 1f;
        }
    }
 
    public void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
 
        Debug.Log("PLAYER: Morir() ejecutado.");
 
        ReproducirSonido(sonidoMuerte);
        rBody2D.linearVelocity = Vector2.zero;
        rBody2D.bodyType = RigidbodyType2D.Kinematic;
        animator.SetTrigger("Morir");
        sr.enabled = true;
 
        if (GameManager.Instancia != null)
        {
            Debug.Log("PLAYER: Llamando a GameManager.GameOver()");
            GameManager.Instancia.GameOver();
        }
        else
        {
            Debug.LogError("PLAYER: GameManager.Instancia es NULL. " +
                "Comprueba que hay un GameObject GameManager en la escena con el script GameManager.cs");
        }
    }
 
    public void ReproducirSonidoMoneda() => ReproducirSonido(sonidoMoneda);
    public void ReproducirSonidoLlave()  => ReproducirSonido(sonidoLlave);
 
    public void ReproducirSonido(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
            _audioSource.PlayOneShot(clip);
    }
 
    void OnDrawGizmosSelected()
    {
        if (puntoAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
        }
    }
}