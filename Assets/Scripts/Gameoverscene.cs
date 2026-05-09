using UnityEngine;
using UnityEngine.SceneManagement;
 
// =====================================================================
//  GAME OVER — escena completa
//
//  Colocar en un GameObject vacío en la escena "GameOver".
//  Fondo con color (Image que cubra toda la pantalla).
//
//  En el Inspector conectar botones:
//    Boton "Retry"  → OnClick() → GameOverScene.Retry()
//    Boton "Menu"   → OnClick() → GameOverScene.Menu()
//
//  Asignar:
//    - sonidoBoton     → AudioClip que suena al pulsar cualquier botón
//    - musicaDerrota   → AudioClip de música de derrota (si no usa MusicaManager)
// =====================================================================
 
public class GameOverScene : MonoBehaviour
{
    [Header("Nombres de escenas")]
    public string nombreEscenaNivel = "Nivel1";
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
        // Si el MusicaManager persiste entre escenas, reproduce la música de derrota
        // (ya se llamó ReproducirGameOver() desde el GameManager antes de cambiar de escena)
        // Si no hay MusicaManager, puedes asignar un AudioSource con tu música aquí
        Time.timeScale = 1f; // por si venía pausado
    }
 
    // Boton "Retry"
    public void Retry()
    {
        ReproducirSonido();
        SceneManager.LoadScene(nombreEscenaNivel);
    }
 
    // Boton "Menu"
    public void Menu()
    {
        ReproducirSonido();
        SceneManager.LoadScene(nombreEscenaMenu);
    }
 
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoBoton != null)
            audioSource.PlayOneShot(sonidoBoton);
    }
}
