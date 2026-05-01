using UnityEngine;
 
// Adjunta este script a la puerta del mapa.
// La puerta necesita DOS Collider2D:
//   1. Un Collider2D normal (no trigger) → pared física que bloquea el paso.
//   2. Un Collider2D en modo "Is Trigger" → detecta cuando el jugador toca la puerta.
// Ambos en el mismo GameObject.
// Asigna el Animator si quieres animación de apertura.
 
public class Puerta : MonoBehaviour
{
    [Header("Sonido")]
    public AudioClip sonidoAbrirPuerta;
 
    [Header("Siguiente nivel")]
    public string nombreSiguienteEscena = "Nivel2"; // Nombre exacto de la escena en Build Settings
 
    [Header("Componentes")]
    public Animator animator;              // Animator de la puerta (opcional)
    private Collider2D colisionFisica;     // El Collider2D que bloquea el paso
    private AudioSource audioSource;
 
    private bool estaAbierta = false;
 
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
 
        // Buscamos el Collider2D que NO es trigger (la pared física)
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D col in cols)
        {
            if (!col.isTrigger)
            {
                colisionFisica = col;
                break;
            }
        }
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        if (!estaAbierta)
        {
            // Intentar abrir: solo si el jugador tiene la llave
            if (GameManager.Instancia != null && GameManager.Instancia.TieneLlave())
            {
                AbrirPuerta();
            }
            else
            {
                Debug.Log("Necesitas la llave para abrir esta puerta.");
            }
        }
        else
        {
            // La puerta ya está abierta → pasar al siguiente nivel
            PasarAlSiguienteNivel();
        }
    }
 
    private void AbrirPuerta()
    {
        estaAbierta = true;
 
        // Quitar la colisión física para que el jugador pueda pasar
        if (colisionFisica != null)
            colisionFisica.enabled = false;
 
        // Sonido de apertura
        if (audioSource != null && sonidoAbrirPuerta != null)
            audioSource.PlayOneShot(sonidoAbrirPuerta);
 
        // Animación de apertura (necesita un trigger "Abrir" en el Animator)
        if (animator != null)
            animator.SetTrigger("Abrir");
 
        // Consumir la llave del GameManager
        if (GameManager.Instancia != null)
            GameManager.Instancia.UsarLlave();
 
        Debug.Log("¡Puerta abierta!");
    }
 
    private void PasarAlSiguienteNivel()
    {
        if (GameManager.Instancia != null)
            GameManager.Instancia.CargarSiguienteNivel(nombreSiguienteEscena);
    }
}