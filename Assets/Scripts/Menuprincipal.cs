using UnityEngine;
using UnityEngine.SceneManagement;
 
public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombre exacto de la escena del nivel (archivo .unity)")]
    public string nombreEscenaNivel = "Nivel2";
 
    [Header("Sonido boton")]
    public AudioClip sonidoBoton;
    private AudioSource audioSource;
 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
 
    public void Play()
    {
        ReproducirSonido();
        Invoke(nameof(CargarNivel), 0.15f);
    }
 
    public void Quit()
    {
        ReproducirSonido();
        Invoke(nameof(SalirJuego), 0.15f);
    }
 
    void CargarNivel()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaNivel))
            SceneManager.LoadScene(nombreEscenaNivel);
        else
            Debug.LogError("MENU: Escena '" + nombreEscenaNivel + "' no encontrada en Build Settings.");
    }
 
    void SalirJuego()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
 
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoBoton != null)
            audioSource.PlayOneShot(sonidoBoton);
    }
}