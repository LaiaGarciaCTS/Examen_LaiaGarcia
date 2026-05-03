using UnityEngine;
 
//  PLAYER CONTROLLER
 
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    //  MOVIMIENTO
    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 12f;
 
    //  DETECTOR DE SUELO
    [Header("Detección de suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.15f;
    public LayerMask capaSuelo;
 
    //  SALUD / VIDAS
    [Header("Salud")]
    public int vidasMaximas = 3;
    private int vidasActuales;
    public float tiempoInvulnerabilidad = 1f;
    private float timerInvulnerabilidad = 0f;
    private bool esInvulnerable = false;
 
    //  MUERTE POR CAÍDA
    [Header("Límite de caída")]
    public float limiteCaida = -10f;
 
    //  ATAQUE
    [Header("Ataque")]
    public Transform puntoAtaque;
    public float radioAtaque = 0.5f;
    public int dañoAtaque = 1;
    public LayerMask capaEnemigos;
    public float cooldownAtaque = 0.4f;
    private float timerAtaque = 0f;
 
    //  SONIDOS
    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoDaño;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoLlave;
 
    //  PRIVADO
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;
 
    private float horizontal;
    private bool saltarEsteFrame = false;   // Flag: saltar en el próximo FixedUpdate
    private bool enSuelo = false;
    private bool estaMuerto = false;
 
    //  AWAKE / START
 
    void Awake()
    {
        rb          = GetComponent<Rigidbody2D>();
        sr          = GetComponent<SpriteRenderer>();
        anim        = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        vidasActuales = vidasMaximas;
    }
 
    void Start()
    {
        UIManager.Instancia?.ActualizarVidas(vidasActuales);
    }
 
    //  UPDATE  — lectura de input y animaciones
 
    void Update()
    {
        if (estaMuerto) return;
 
        // Suelo
        ComprobarSuelo();
 
        // Movimiento horizontal + flip de sprite
        horizontal = Input.GetAxisRaw("Horizontal");
        if      (horizontal > 0f) sr.flipX = false;
        else if (horizontal < 0f) sr.flipX = true;
 
        // Animación caminar / idle (solo cuando pisa el suelo)
        if (enSuelo)
            anim.SetFloat("Velocidad", Mathf.Abs(horizontal));
 
        // Salto: detectado aquí, aplicado en FixedUpdate
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            saltarEsteFrame = true;
            enSuelo = false;                  // evita doble salto en el mismo frame
            anim.SetBool("EnSuelo", false);
            anim.SetTrigger("Saltando");
            ReproducirSonido(sonidoSalto);
        }
 
        // Ataque
        GestionarAtaque();
 
        // Muerte por caída al vacío
        if (transform.position.y < limiteCaida)
            Morir();
 
        // Parpadeo invulnerabilidad
        GestionarInvulnerabilidad();
    }
 
    //  FIXED UPDATE — física
 
    void FixedUpdate()
    {
        if (estaMuerto) return;
 
        if (saltarEsteFrame)
        {
            // Resetea Y para que la fuerza sea siempre la misma sin importar caídas previas
            rb.linearVelocity = new Vector2(horizontal * velocidadMovimiento, fuerzaSalto);
            saltarEsteFrame = false;
        }
        else
        {
            // Solo toca la X; conserva la Y de la gravedad
            rb.linearVelocity = new Vector2(horizontal * velocidadMovimiento, rb.linearVelocity.y);
        }
    }
 
    //  LÓGICA PRIVADA
 
    void ComprobarSuelo()
    {
        if (puntoSuelo == null) return;
 
        bool anteriorEnSuelo = enSuelo;
        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);
        anim.SetBool("EnSuelo", enSuelo);
 
        // Al aterrizar: actualiza Velocidad para que Idle/Walk se reactive
        if (!anteriorEnSuelo && enSuelo)
            anim.SetFloat("Velocidad", Mathf.Abs(horizontal));
    }
 
    void GestionarAtaque()
    {
        timerAtaque -= Time.deltaTime;
 
        if (Input.GetKeyDown(KeyCode.Z) && timerAtaque <= 0f)
        {
            timerAtaque = cooldownAtaque;
            anim.SetTrigger("Atacar");
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
    }
 
    void GestionarInvulnerabilidad()
    {
        if (!esInvulnerable) return;
 
        timerInvulnerabilidad -= Time.deltaTime;
        sr.enabled = (Mathf.Sin(Time.time * 20f) > 0f);
 
        if (timerInvulnerabilidad <= 0f)
        {
            esInvulnerable = false;
            sr.enabled = true;
        }
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
            timerInvulnerabilidad = tiempoInvulnerabilidad;
        }
    }
 
    public void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
 
        ReproducirSonido(sonidoMuerte);
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetTrigger("Morir");
        sr.enabled = true;
 
        if (GameManager.Instancia != null)
            GameManager.Instancia.GameOver();
    }
 
    public void ReproducirSonidoMoneda() => ReproducirSonido(sonidoMoneda);
    public void ReproducirSonidoLlave()  => ReproducirSonido(sonidoLlave);
 
    public void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
 
    //  GIZMOS
 
    void OnDrawGizmosSelected()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
        if (puntoAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
        }
    }
}