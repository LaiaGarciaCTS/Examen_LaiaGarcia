using UnityEngine;



//  PUERTA
public class Puerta : MonoBehaviour
{
    [Header("Sonido")]
    public AudioClip sonidoAbrirPuerta;
 
    [Header("Siguiente nivel")]
    public string nombreSiguienteEscena = "Nivel2";
 
    [Header("Animator")]
    public Animator animator;
 
    [Header("Duracion animacion Entrecerrada (segundos)")]
    public float duracionAnimacionAbriendo = 1f;
 
    //  PRIVADO
    private AudioSource audioSource;
    private bool jugadorCerca = false;
    private bool estaAbierta  = false;
    private bool abriendo     = false;
 
    //  AWAKE
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
 
        if (animator == null)
            animator = GetComponent<Animator>();
    }
 
    //  UPDATE — flecha arriba cuando el jugador está cerca
    void Update()
    {
        if (!jugadorCerca) return;
        if (abriendo) return;
 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (estaAbierta)
            {
                PasarAlSiguienteNivel();
                return;
            }
 
            if (GameManager.Instancia == null)
            {
                Debug.LogWarning("PUERTA: GameManager no encontrado.");
                return;
            }
 
            if (GameManager.Instancia.TieneLlave())
                IniciarApertura();
            else
                Debug.Log("PUERTA: No tienes la llave.");
        }
    }
 
    //  TRIGGER — jugador entra o sale de la zona
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorCerca = true;
        Debug.Log("PUERTA: Jugador cerca. Tiene llave: "
            + (GameManager.Instancia != null && GameManager.Instancia.TieneLlave()));
    }
 
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
 
    //  INICIAR APERTURA
    void IniciarApertura()
    {
        abriendo = true;
 
        // Desactiva todos los colliders fisicos (no trigger) del objeto y sus hijos
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            if (!col.isTrigger)
            {
                col.enabled = false;
                Debug.Log("PUERTA: Collider fisico desactivado en " + col.gameObject.name);
            }
        }
 
        // Sonido
        if (audioSource != null && sonidoAbrirPuerta != null)
            audioSource.PlayOneShot(sonidoAbrirPuerta);
 
        // Animacion: usa SetTrigger en lugar de SetBool — más fiable
        if (animator != null)
            animator.SetTrigger("Abriendo");
 
        // Consumir la llave
        GameManager.Instancia.UsarLlave();
 
        // Esperar a que termine "Entrecerrada" y pasar a "Abierta"
        Invoke(nameof(TerminarApertura), duracionAnimacionAbriendo);
    }
 
    //  TERMINAR APERTURA
    void TerminarApertura()
    {
        estaAbierta = true;
        abriendo    = false;
 
        // Animacion final: Entrecerrada → Abierta
        if (animator != null)
            animator.SetTrigger("Abierta");
 
        Debug.Log("PUERTA: Abierta. Pulsa arriba para entrar.");
    }
 
    //  SIGUIENTE NIVEL
    void PasarAlSiguienteNivel()
    {
        if (GameManager.Instancia != null)
            GameManager.Instancia.CargarSiguienteNivel(nombreSiguienteEscena);
    }
}