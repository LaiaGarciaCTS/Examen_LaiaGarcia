using UnityEngine;
using UnityEngine.SceneManagement;
 
public class GameOverScene : MonoBehaviour
{
    [Header("Nombres exactos de escenas")]
    public string nombreEscenaNivel = "Nivel2";
    public string nombreEscenaMenu  = "MenuPrincipal";
 
    [Header("Sonido boton")]
    public AudioClip sonidoBoton;
    private AudioSource audioSource;
 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
 
    void Start()
    {
        Time.timeScale = 1f;
    }
 
    public void Retry()
    {
        ReproducirSonido();
        Invoke(nameof(CargarNivel), 0.15f);
    }
 
    public void Menu()
    {
        ReproducirSonido();
        Invoke(nameof(CargarMenu), 0.15f);
    }
 
    void CargarNivel()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaNivel))
            SceneManager.LoadScene(nombreEscenaNivel);
        else
            Debug.LogError("GAMEOVER: Escena '" + nombreEscenaNivel + "' no encontrada en Build Settings.");
    }
 
    void CargarMenu()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaMenu))
            SceneManager.LoadScene(nombreEscenaMenu);
        else
            Debug.LogError("GAMEOVER: Escena '" + nombreEscenaMenu + "' no encontrada en Build Settings.");
    }
 
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoBoton != null)
            audioSource.PlayOneShot(sonidoBoton);
    }
}