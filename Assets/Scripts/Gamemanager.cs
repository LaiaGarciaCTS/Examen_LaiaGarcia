using UnityEngine;
using UnityEngine.SceneManagement;
 
// Coloca este script en un GameObject vacío llamado "GameManager" en la escena.
 
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
 
    //  SINGLETON
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }
 
    //  MONEDAS
    public void AñadirMoneda(int valor)
    {
        monedas += valor;
        Debug.Log($"Monedas: {monedas}");
        // UIManager.Instancia?.ActualizarMonedas(monedas); // Descomenta si tienes UI
    }
 
    //  LLAVE
    public void RecogerLlave()
    {
        tieneLlave = true;
        Debug.Log("¡Llave recogida!");
        // UIManager.Instancia?.MostrarIconoLlave(true);
    }
 
    public bool TieneLlave() => tieneLlave;
 
    public void UsarLlave()
    {
        tieneLlave = false;
        // UIManager.Instancia?.MostrarIconoLlave(false);
    }
 
    //  SIGUIENTE NIVEL
    public void CargarSiguienteNivel(string nombreEscena)
    {
        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
        else
            Debug.LogWarning("No se ha asignado una escena de siguiente nivel en la Puerta.");
    }
 
    //  GAME OVER
    public void GameOver()
    {
        Debug.Log("GAME OVER");
        MusicaManager.Instancia?.PararMusica();
        Invoke(nameof(CargarGameOver), tiempoEsperaGameOver);
    }
 
    private void CargarGameOver()
    {
        if (!string.IsNullOrEmpty(escenaGameOver))
            SceneManager.LoadScene(escenaGameOver);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}