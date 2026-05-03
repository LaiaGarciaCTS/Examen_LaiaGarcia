using UnityEngine;
using UnityEngine.InputSystem;
 
//  PLAYER CONTROLLER
 
public class PlayerController : MonoBehaviour
{
    // ------------------------------------------------------------------
    //  VARIABLES PÚBLICAS (visibles en Inspector)
    // ------------------------------------------------------------------
    public Vector3 startPosition;
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
 
    [Header("Salud")]
    public int vidasMaximas = 3;
 
    [Header("Caída")]
    public float limiteCaida = -10f;
 
    [Header("Ataque")]
    public Transform puntoAtaque;
    public float radioAtaque = 0.5f;
    public int dañoAtaque = 1;
    public LayerMask capaEnemigos;
    public float cooldownAtaque = 0.4f;
 
    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoDaño;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoLlave;
 
    // ------------------------------------------------------------------
    //  VARIABLES PRIVADAS
    // ------------------------------------------------------------------
    private Rigidbody2D rBody2D;
    private SpriteRenderer renderer;
    private Animator animator;
    private AudioSource _audioSource;
    private GroundSensor sensor;      // <-- usa tu GroundSensor con OnTrigger
 
    private InputAction moveAction;
    private InputAction jumpAction;
 
    private Vector2 moveDirection;
    private int vidasActuales;
    private bool estaMuerto = false;
    private float timerAtaque = 0f;
    private float timerInvulnerabilidad = 0f;
    private bool esInvulnerable = false;
 
    // ==================================================================
    //  AWAKE
    // ==================================================================
    void Awake()
    {
        rBody2D      = GetComponent<Rigidbody2D>();
        renderer     = GetComponent<SpriteRenderer>();
        animator     = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        sensor       = GetComponentInChildren<GroundSensor>(); // busca en hijos
 
        moveAction = InputSystem.actions["Move"];
        jumpAction = InputSystem.actions["Jump"];
    }
 
    // ==================================================================
    //  START
    // ==================================================================
    void Start()
    {
        transform.position = startPosition;
        vidasActuales = vidasMaximas;
        // UIManager.Instancia?.ActualizarVidas(vidasActuales);
    }
 
    // ==================================================================
    //  UPDATE
    // ==================================================================
    void Update()
    {
        if (estaMuerto) return;
 
        timerAtaque -= Time.deltaTime;
 
        // Leer dirección del teclado
        moveDirection = moveAction.ReadValue<Vector2>();
 
        // Flip del sprite + animación de correr (según apuntes)
        if (moveDirection.x > 0)
        {
            renderer.flipX = false;
            animator.SetBool("IsRunning", true);
        }
        else if (moveDirection.x < 0)
        {
            renderer.flipX = true;
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
 
        // ---------------------------------------------------------------
        //  SALTO — usa sensor.IsGrounded() igual que en tus apuntes
        // ---------------------------------------------------------------
        if (jumpAction.WasPressedThisFrame() && sensor.IsGrounded())
        {
            rBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            ReproducirSonido(sonidoSalto);
        }
 
        // Animación salto: !sensor.IsGrounded() (según apuntes)
        animator.SetBool("IsJumping", !sensor.IsGrounded());
 
        // Ataque con Z
        if (Input.GetKeyDown(KeyCode.Z) && timerAtaque <= 0f)
        {
            timerAtaque = cooldownAtaque;
            ReproducirSonido(sonidoAtaque);
 
            if (puntoAtaque != null)
            {
                Collider2D[] golpeados = Physics2D.OverlapCircleAll(
                    puntoAtaque.position, radioAtaque, capaEnemigos);
 
                foreach (Collider2D col in golpeados)
                {
                    EnemyController ec = col.GetComponent<EnemyController>();
                    if (ec != null) ec.RecibirDaño(dañoAtaque);
                }
            }
        }
 
        // Muerte por caída
        if (transform.position.y < limiteCaida)
            Morir();
 
        // Parpadeo invulnerabilidad
        if (esInvulnerable)
        {
            timerInvulnerabilidad -= Time.deltaTime;
            renderer.enabled = (Mathf.Sin(Time.time * 20f) > 0f);
            if (timerInvulnerabilidad <= 0f)
            {
                esInvulnerable = false;
                renderer.enabled = true;
            }
        }
    }
 
    // ==================================================================
    //  FIXED UPDATE — linearVelocity (según apuntes)
    // ==================================================================
    void FixedUpdate()
    {
        if (estaMuerto) return;
        rBody2D.linearVelocity = new Vector2(
            moveDirection.x * movementSpeed,
            rBody2D.linearVelocity.y);
    }
 
    // ==================================================================
    //  API PÚBLICA
    // ==================================================================
    public void RecibirDanio(int cantidad = 1)
    {
        if (estaMuerto || esInvulnerable) return;
 
        vidasActuales -= cantidad;
        ReproducirSonido(sonidoDaño);
        // UIManager.Instancia?.ActualizarVidas(vidasActuales);
 
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
 
        ReproducirSonido(sonidoMuerte);
        rBody2D.linearVelocity = Vector2.zero;
        rBody2D.bodyType = RigidbodyType2D.Kinematic;
        animator.SetTrigger("Morir");
        renderer.enabled = true;
 
        GameManager.Instancia?.GameOver();
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