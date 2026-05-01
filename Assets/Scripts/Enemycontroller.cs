using UnityEngine;
 
// =====================================================================
//  ENEMY CONTROLLER
//  Requiere en el mismo GameObject:
//    - Rigidbody2D  (Gravity Scale 1, Collision Detection: Continuous)
//    - Collider2D   (el cuerpo físico del enemigo)
//    - SpriteRenderer
//    - Animator
//    - AudioSource
//
//  Layer del GameObject: "Enemigo"
//
//  Parámetros del Animator que DEBES crear:
//    - "Velocidad"  → Float
//    - "Morir"      → Trigger
//    - "Atacar"     → Trigger  (solo si usas animación de ataque)
//
//  Cómo funciona:
//    - Patrulla entre puntoA y puntoB.
//    - Si el jugador entra en el rango de detección, lo persigue.
//    - Al contacto con el jugador, le hace daño.
//    - Si el jugador le salta encima (viene de arriba), muere.
// =====================================================================
 
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    // ------------------------------------------------------------------
    //  SALUD
    // ------------------------------------------------------------------
    [Header("Salud")]
    public int vida = 2;
 
    // ------------------------------------------------------------------
    //  PATRULLA
    // ------------------------------------------------------------------
    [Header("Patrulla")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadPatrulla = 2f;
 
    // ------------------------------------------------------------------
    //  PERSECUCIÓN
    // ------------------------------------------------------------------
    [Header("Persecución")]
    public float rangoDeteccion = 4f;
    public float velocidadPersecucion = 3.5f;
 
    // ------------------------------------------------------------------
    //  DAÑO AL JUGADOR
    // ------------------------------------------------------------------
    [Header("Daño")]
    public int dañoAlJugador = 1;
    public float cooldownDanio = 1f;
    private float timerDanio = 0f;
 
    // ------------------------------------------------------------------
    //  MUERTE POR PISADA
    // ------------------------------------------------------------------
    [Header("Muerte por pisada")]
    public float fuerzaRebote = 6f;
 
    // ------------------------------------------------------------------
    //  SONIDOS
    // ------------------------------------------------------------------
    [Header("Sonidos")]
    public AudioClip sonidoMuerte;
    public AudioClip sonidoGolpe;
 
    // ------------------------------------------------------------------
    //  REFERENCIAS
    // ------------------------------------------------------------------
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;
 
    private Transform jugador;
    private bool estaMuerto = false;
    private bool yendoHaciaB = true;
 
    // ==================================================================
    //  UNITY MESSAGES
    // ==================================================================
 
    void Awake()
    {
        rb          = GetComponent<Rigidbody2D>();
        sr          = GetComponent<SpriteRenderer>();
        anim        = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
 
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) jugador = go.transform;
    }
 
    void Update()
    {
        if (estaMuerto) return;
 
        timerDanio -= Time.deltaTime;
 
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) < rangoDeteccion)
            Perseguir();
        else
            Patrullar();
    }
 
    // ==================================================================
    //  MOVIMIENTO
    // ==================================================================
 
    void Patrullar()
    {
        if (puntoA == null || puntoB == null) return;
 
        Transform objetivo = yendoHaciaB ? puntoB : puntoA;
        MoverHacia(objetivo.position, velocidadPatrulla);
 
        if (Vector2.Distance(transform.position, objetivo.position) < 0.2f)
            yendoHaciaB = !yendoHaciaB;
    }
 
    void Perseguir()
    {
        if (jugador == null) return;
        MoverHacia(jugador.position, velocidadPersecucion);
    }
 
    void MoverHacia(Vector2 destino, float velocidad)
    {
        Vector2 direccion = ((Vector2)destino - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(direccion.x * velocidad, rb.linearVelocity.y);
 
        if (direccion.x > 0.01f) sr.flipX = false;
        else if (direccion.x < -0.01f) sr.flipX = true;
 
        anim.SetFloat("Velocidad", Mathf.Abs(direccion.x));
    }
 
    // ==================================================================
    //  CONTACTO CON EL JUGADOR
    // ==================================================================
 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaMuerto) return;
        if (!collision.collider.CompareTag("Player")) return;
 
        PlayerController jugadorCtrl = collision.collider.GetComponent<PlayerController>();
        if (jugadorCtrl == null) return;
 
        bool pisadaDesdeArriba = collision.transform.position.y > transform.position.y + 0.3f;
 
        if (pisadaDesdeArriba)
        {
            Rigidbody2D rbJugador = collision.collider.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
                rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaRebote);
 
            RecibirDaño(vida);
        }
        else
        {
            if (timerDanio <= 0f)
            {
                timerDanio = cooldownDanio;
                jugadorCtrl.RecibirDaño(dañoAlJugador);
 
                if (sonidoGolpe != null) audioSource.PlayOneShot(sonidoGolpe);
            }
        }
    }
 
    // ==================================================================
    //  DAÑO AL ENEMIGO
    // ==================================================================
 
    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;
 
        vida -= cantidad;
 
        if (vida <= 0)
            Morir();
    }
 
    void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
 
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
 
        foreach (Collider2D col in GetComponents<Collider2D>())
            col.enabled = false;
 
        anim.SetTrigger("Morir");
 
        if (sonidoMuerte != null) audioSource.PlayOneShot(sonidoMuerte);
 
        Destroy(gameObject, 1f);
    }
 
    // ==================================================================
    //  GIZMOS
    // ==================================================================
 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
 
        if (puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(puntoA.position, puntoB.position);
            Gizmos.DrawSphere(puntoA.position, 0.1f);
            Gizmos.DrawSphere(puntoB.position, 0.1f);
        }
    }
}