using UnityEngine;
using UnityEngine.SceneManagement;
 
// =====================================================================
//  VICTORIA
//
//  Colocar este script en el GameObject "PanelVictoria" (dentro del Canvas).
//  El panel empieza DESACTIVADO. El UIManager lo activa al ganar.
//
//  En el Inspector conectar botones:
//    Boton "Next Level" → OnClick() → Victoria.NextLevel()
//    Boton "Menu"       → OnClick() → Victoria.Menu()
//
//  Asignar en Inspector:
//    - sonidoBoton → AudioClip que suena al pulsar cualquier botón
// =====================================================================
 
public class Victoria : MonoBehaviour
{
    [Header("Nombres de escenas")]
    public string nombreSiguienteEscena = "Nivel2";
    public string nombreEscenaMenu      = "MenuPrincipal";
 
    [Header("Sonido boton")]
    public AudioClip sonidoBoton;
    private AudioSource audioSource;
 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
 
    void OnEnable()
    {
        // Pausa el juego al mostrar la victoria
        Time.timeScale = 0f;
    }
 
    void OnDisable()
    {
        Time.timeScale = 1f;
    }
 
    // Boton "Next Level"
    public void NextLevel()
    {
        ReproducirSonido();
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
 
    // Boton "Menu"
    public void Menu()
    {
        ReproducirSonido();
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
 
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoBoton != null)
            audioSource.PlayOneShot(sonidoBoton);
    }
}
