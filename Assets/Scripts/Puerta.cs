using UnityEngine;



//  PUERTA

public class Puerta : MonoBehaviour
{
    public AudioClip sonidoAbrirPuerta;
 
    public string nombreSiguienteEscena = "Nivel2";
 
    public Animator animator;
 
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
 
    //  UPDATE
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
                Debug.LogError("PUERTA: GameManager.Instancia es NULL al pulsar arriba.");
                return;
            }
 
            Debug.Log("PUERTA: Arriba pulsado. TieneLlave = " + GameManager.Instancia.TieneLlave());
 
            if (GameManager.Instancia.TieneLlave())
                IniciarApertura();
            else
                Debug.Log("PUERTA: No tienes la llave.");
        }
    }
 
    //  TRIGGER
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorCerca = true;
 
        bool llave = GameManager.Instancia != null && GameManager.Instancia.TieneLlave();
        Debug.Log("PUERTA: Jugador entro en la zona. TieneLlave = " + llave);
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
 
        int desactivados = 0;
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            if (!col.isTrigger)
            {
                col.enabled = false;
                desactivados++;
                Debug.Log("PUERTA: Collider fisico desactivado en → " + col.gameObject.name);
            }
        }
 
        if (desactivados == 0)
            Debug.LogWarning("PUERTA: No se encontro ningun collider fisico (non-trigger). " +
                "Asegurate de tener un Collider2D con Is Trigger = OFF en la puerta.");
 
        if (audioSource != null && sonidoAbrirPuerta != null)
            audioSource.PlayOneShot(sonidoAbrirPuerta);
 
        if (animator != null)
            animator.SetBool("Abriendo", true);
 
        GameManager.Instancia.UsarLlave();
 
        Invoke(nameof(TerminarApertura), duracionAnimacionAbriendo);
    }
 
    //  TERMINAR APERTURA
    void TerminarApertura()
    {
        estaAbierta = true;
        abriendo    = false;
 
        if (animator != null)
        {
            animator.SetBool("Abriendo", false);
            animator.SetBool("Abierta",  true);
        }
 
        Debug.Log("PUERTA: Completamente abierta. Pulsa arriba para entrar.");
    }
 
    //  SIGUIENTE NIVEL
    void PasarAlSiguienteNivel()
    {
        if (GameManager.Instancia != null)
            GameManager.Instancia.CargarSiguienteNivel(nombreSiguienteEscena);
    }
}