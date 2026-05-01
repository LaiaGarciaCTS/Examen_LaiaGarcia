using UnityEngine;
using UnityEngine.SceneManagement;
 
// =====================================================================
//  GAME MANAGER
//  Coloca este script en un GameObject vacío llamado "GameManager".
//  NO uses DontDestroyOnLoad aquí (un GameManager por escena).
// =====================================================================
 
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
 
    [Header("Puntuación")]
    public int monedas = 0;
 
    [Header("Llave")]
    private bool tieneLlave = false;
 
    [Header("Game Over")]
    public float tiempoEsperaGameOver = 1.5f;
    public string escenaGameOver = "GameOver";
 
    // ------------------------------------------------------------------
    //  SINGLETON
    // ------------------------------------------------------------------
    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }
 
    // ------------------------------------------------------------------
    //  MONEDAS
    // ------------------------------------------------------------------
    public void AñadirMoneda(int valor)
    {
        monedas += valor;
        Debug.Log($"Monedas: {monedas}");
        UIManager.Instancia?.ActualizarMonedas(monedas);
    }
 
    // ------------------------------------------------------------------
    //  LLAVE
    // ------------------------------------------------------------------
    public void RecogerLlave()
    {
        tieneLlave = true;
        Debug.Log("¡Llave recogida!");
    }
 
    public bool TieneLlave() => tieneLlave;
 
    public void UsarLlave() => tieneLlave = false;
 
    // ------------------------------------------------------------------
    //  SIGUIENTE NIVEL
    // ------------------------------------------------------------------
    public void CargarSiguienteNivel(string nombreEscena)
    {
        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
        else
            Debug.LogWarning("No se ha asignado escena de siguiente nivel en la Puerta.");
    }
 
    // ------------------------------------------------------------------
    //  GAME OVER
    // ------------------------------------------------------------------
    public void GameOver()
    {
        Debug.Log("GAME OVER");
        MusicaManager.Instancia?.PararMusica();
        MusicaManager.Instancia?.ReproducirGameOver(); // ← Música de Game Over
        Invoke(nameof(CargarGameOver), tiempoEsperaGameOver);
    }
 
    void CargarGameOver()
    {
        SceneManager.LoadScene(
            !string.IsNullOrEmpty(escenaGameOver)
                ? escenaGameOver
                : SceneManager.GetActiveScene().name
        );
    }
}
 