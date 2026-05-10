using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Victoria : MonoBehaviour
{
    [Header("Nombres exactos de escenas")]
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
        Time.timeScale = 0f;
    }
 
    void OnDisable()
    {
        Time.timeScale = 1f;
    }
 
    public void NextLevel()
    {
        ReproducirSonido();
        Time.timeScale = 1f;
        Invoke(nameof(CargarSiguiente), 0.15f);
    }
 
    public void Menu()
    {
        ReproducirSonido();
        Time.timeScale = 1f;
        Invoke(nameof(CargarMenu), 0.15f);
    }
 
    void CargarSiguiente()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreSiguienteEscena))
            SceneManager.LoadScene(nombreSiguienteEscena);
        else
            Debug.LogError("VICTORIA: Escena '" + nombreSiguienteEscena + "' no encontrada en Build Settings.");
    }
 
    void CargarMenu()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaMenu))
            SceneManager.LoadScene(nombreEscenaMenu);
        else
            Debug.LogError("VICTORIA: Escena '" + nombreEscenaMenu + "' no encontrada en Build Settings.");
    }
 
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoBoton != null)
            audioSource.PlayOneShot(sonidoBoton);
    }
}