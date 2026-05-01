using UnityEngine;
 
public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rBody2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
 
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
 
    public GroundSensor sensor;
 
    [Header("Muerte")]
    public float limiteCaida = -10f;
 
    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoLlave;
 
    private Vector2 moveDirection;
    private bool estaMuerto = false;
 
    void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
 
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
 
    void Update()
    {
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
 
        // 5. MUERTE POR CAÍDA
        if (transform.position.y < limiteCaida)
            Morir();
    }
 
    void FixedUpdate()
    {
        if (estaMuerto) return;
        rBody2D.linearVelocity = new Vector2(moveDirection.x * movementSpeed, rBody2D.linearVelocity.y);
    }
 
    //  MUERTE
    public void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
 
        ReproducirSonido(sonidoMuerte);
        rBody2D.linearVelocity = Vector2.zero;
        rBody2D.bodyType = RigidbodyType2D.Kinematic;
        animator.SetTrigger("Morir");
 
        if (GameManager.Instancia != null)
            GameManager.Instancia.GameOver();
    }
 
    //  SONIDOS COLECCIONABLES (llamados desde cada coleccionable)
    public void ReproducirSonidoMoneda() => ReproducirSonido(sonidoMoneda);
    public void ReproducirSonidoLlave()  => ReproducirSonido(sonidoLlave);
 
    //  UTILIDAD
    public void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}