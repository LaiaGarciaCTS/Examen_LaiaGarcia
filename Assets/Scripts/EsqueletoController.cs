using UnityEngine;
using UnityEngine.InputSystem;
 
public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rBody2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
 
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
 
    public GroundSensor sensor;
 
    // --- MUERTE ---
    [Header("Muerte")]
    public float limiteCaida = -10f; // Coordenada Y por debajo de la cual el jugador muere
 
    // --- SONIDOS ---
    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoRecoger;
 
    private Vector2 moveDirection;
    private bool estaMuerto = false;
 
    void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
 
        // Si no se asigna el AudioSource en el Inspector, lo buscamos en el mismo objeto
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
 
    void Update()
    {
        // Si el personaje está muerto no hacemos nada más
        if (estaMuerto) return;
 
        // 1. MOVIMIENTO
        float horizontal = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(horizontal, 0);
 
        // 2. FLIP
        if (horizontal > 0) spriteRenderer.flipX = false;
        else if (horizontal < 0) spriteRenderer.flipX = true;
 
        // 3. ANIMACIONES
        if (sensor != null)
        {
            bool tocandoSuelo = sensor.IsGrounded();
 
            if (!tocandoSuelo)
            {
                animator.SetBool("Saltar", true);
                animator.SetBool("Caminar", false);
            }
            else
            {
                animator.SetBool("Saltar", false);
                animator.SetBool("Caminar", horizontal != 0);
            }
        }
 
        // 4. SALTO
        if (Input.GetKeyDown(KeyCode.Space) && sensor != null && sensor.IsGrounded())
        {
            rBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            ReproducirSonido(sonidoSalto);
        }
 
        // 5. COMPROBACIÓN DE CAÍDA (muerte por salir del mapa)
        if (transform.position.y < limiteCaida)
        {
            Morir();
        }
    }
 
    void FixedUpdate()
    {
        if (estaMuerto) return;
 
        rBody2D.linearVelocity = new Vector2(moveDirection.x * movementSpeed, rBody2D.linearVelocity.y);
    }
 
    // -------------------------------------------------------
    //  MUERTE
    // -------------------------------------------------------
    public void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
 
        // Sonido de muerte
        ReproducirSonido(sonidoMuerte);
 
        // Paramos físicas y animaciones
        rBody2D.linearVelocity = Vector2.zero;
        rBody2D.bodyType = RigidbodyType2D.Kinematic;
 
        // Animación de muerte (si existe en el Animator)
        animator.SetTrigger("Morir");
 
        // Avisamos al GameManager para que gestione el Game Over
        if (GameManager.Instancia != null)
            GameManager.Instancia.GameOver();
    }
 
    // -------------------------------------------------------
    //  COLECCIONABLES  (llamado desde el script Coleccionable)
    // -------------------------------------------------------
    public void RecogerColeccionable()
    {
        ReproducirSonido(sonidoRecoger);
    }
 
    // -------------------------------------------------------
    //  UTILIDAD
    // -------------------------------------------------------
    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}