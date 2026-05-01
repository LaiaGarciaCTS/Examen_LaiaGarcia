using UnityEngine;

// =====================================================================
//  PUERTA
//  Necesita DOS Collider2D:
//    1. Sin Is Trigger  → pared física
//    2. Con Is Trigger  → zona de detección
//
//  Animator con:
//    - Estado por defecto: "Cerrada" (animación estática, SIN loop)
//    - Trigger "Abrir" → transición a animación de apertura (SIN loop)
// =====================================================================
public class Puerta : MonoBehaviour
{
    [Header("Sonido")]
    public AudioClip sonidoAbrirPuerta;
 
    [Header("Siguiente nivel")]
    public string nombreSiguienteEscena = "Nivel2";
 
    [Header("Componentes")]
    public Animator animator;
 
    private Collider2D colisionFisica;
    private AudioSource audioSource;
    private bool estaAbierta = false;
 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
 
        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            if (!col.isTrigger) { colisionFisica = col; break; }
        }
    }
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        if (!estaAbierta)
        {
            if (GameManager.Instancia != null && GameManager.Instancia.TieneLlave())
                AbrirPuerta();
            else
                Debug.Log("Necesitas la llave para abrir esta puerta.");
        }
        else
        {
            PasarAlSiguienteNivel();
        }
    }
 
    void AbrirPuerta()
    {
        estaAbierta = true;
 
        if (colisionFisica != null) colisionFisica.enabled = false;
        if (audioSource != null && sonidoAbrirPuerta != null)
            audioSource.PlayOneShot(sonidoAbrirPuerta);
        if (animator != null) animator.SetTrigger("Abrir");
        if (GameManager.Instancia != null) GameManager.Instancia.UsarLlave();
 
        Debug.Log("¡Puerta abierta!");
    }
 
    void PasarAlSiguienteNivel()
    {
        if (GameManager.Instancia != null)
            GameManager.Instancia.CargarSiguienteNivel(nombreSiguienteEscena);
    }
}
