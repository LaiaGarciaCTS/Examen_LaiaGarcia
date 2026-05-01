using UnityEngine;
 
// =====================================================================
//  PLAYER CONTROLLER
//  Requiere en el mismo GameObject:
//    - Rigidbody2D
//    - Collider2D (CapsuleCollider2D recomendado)
//    - SpriteRenderer
//    - Animator
//    - AudioSource
//
//  Parámetros del Animator que DEBES crear (exactamente igual):
//    - "Velocidad"  → Float  (para caminar / idle)
//    - "EnSuelo"    → Bool   (true cuando toca el suelo)
//    - "Morir"      → Trigger (para la animación de muerte)
//    - "Atacar"     → Trigger (para la animación de ataque)
// =====================================================================
 
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // ------------------------------------------------------------------
    //  MOVIMIENTO
    // ------------------------------------------------------------------
    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 10f;
 
    // ------------------------------------------------------------------
    //  DETECTOR DE SUELO
    // ------------------------------------------------------------------
    [Header("Detección de suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.15f;
    public LayerMask capaSuelo;
 
    // ------------------------------------------------------------------
    //  SALUD / VIDAS
    // ------------------------------------------------------------------
    [Header("Salud")]
    public int vidasMaximas = 3;
    private int vidasActuales;
    public float tiempoInvulnerabilidad = 1f;
    private float timerInvulnerabilidad = 0f;
    private bool esInvulnerable = false;
 
    // ------------------------------------------------------------------
    //  MUERTE POR CAÍDA
    // ------------------------------------------------------------------
    [Header("Límite de caída")]
    public float limiteCaida = -10f;
 
    // ------------------------------------------------------------------
    //  ATAQUE
    // ------------------------------------------------------------------
    [Header("Ataque")]
    public Transform puntoAtaque;
    public float radioAtaque = 0.5f;
    public int dañoAtaque = 1;
    public LayerMask capaEnemigos;
    public float cooldownAtaque = 0.4f;
    private float timerAtaque = 0f;
 
    // ------------------------------------------------------------------
    //  SONIDOS
    // ------------------------------------------------------------------
    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoDaño;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoLlave;
 
    // ------------------------------------------------------------------
    //  REFERENCIAS PRIVADAS
    // ------------------------------------------------------------------
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;
 
    private float horizontal;
    private bool estaMuerto = false;
    private bool enSuelo = false;
 
    // ==================================================================
    //  UNITY MESSAGES
    // ==================================================================
 
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
        // Actualizar UI al inicio con las vidas iniciales
        UIManager.Instancia?.ActualizarVidas(vidasActuales);
    }
 
    void Update()
    {
        if (estaMuerto) return;
 
        ComprobarSuelo();
        GestionarMovimiento();
        GestionarSalto();
        GestionarAtaque();
        ComprobarCaida();
        GestionarInvulnerabilidad();
    }
 
    void FixedUpdate()
    {
        if (estaMuerto) return;
        rb.linearVelocity = new Vector2(horizontal * velocidadMovimiento, rb.linearVelocity.y);
    }
 
    // ==================================================================
    //  LÓGICA PRIVADA
    // ==================================================================
 
    void ComprobarSuelo()
    {
        if (puntoSuelo == null) return;
        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);
        anim.SetBool("EnSuelo", enSuelo);
    }
 
    void GestionarMovimiento()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
 
        if (horizontal > 0) sr.flipX = false;
        else if (horizontal < 0) sr.flipX = true;
 
        anim.SetFloat("Velocidad", Mathf.Abs(horizontal));
    }
 
    void GestionarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            ReproducirSonido(sonidoSalto);
        }
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
                Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(
                    puntoAtaque.position, radioAtaque, capaEnemigos);
 
                foreach (Collider2D enemigo in enemigosGolpeados)
                {
                    EnemyController ec = enemigo.GetComponent<EnemyController>();
                    if (ec != null) ec.RecibirDaño(dañoAtaque);
                }
            }
        }
    }
 
    void ComprobarCaida()
    {
        if (transform.position.y < limiteCaida)
            Morir();
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
 
    // ==================================================================
    //  API PÚBLICA
    // ==================================================================
 
    public void RecibirDaño(int cantidad = 1)
    {
        if (estaMuerto || esInvulnerable) return;
 
        vidasActuales -= cantidad;
        ReproducirSonido(sonidoDaño);
 
        UIManager.Instancia?.ActualizarVidas(vidasActuales);
 
        if (vidasActuales <= 0)
        {
            Morir();
        }
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