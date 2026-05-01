using UnityEngine;
using UnityEngine.SceneManagement;
 
// Coloca este script en un GameObject vacío llamado "GameManager" en la escena.
// Marca ese GameObject como "No destruir al cargar escena" si lo necesitas persistente.
 
public class GameManager : MonoBehaviour

{
    // Patrón Singleton para acceder desde cualquier script
    public static GameManager Instancia { get; private set; }
 
    [Header("Puntuación")]
    public int monedas = 0;
    public int puntos  = 0;
 
    [Header("Game Over")]
    public float tiempoEsperaGameOver = 1.5f; // Segundos antes de recargar/ir al menú
    public string escenaGameOver = "GameOver"; // Nombre de la escena de Game Over (opcional)
 
    // -------------------------------------------------------
    //  SINGLETON
    // -------------------------------------------------------
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        // DontDestroyOnLoad(gameObject); // Descomenta si quieres que persista entre escenas
    }
 
    // -------------------------------------------------------
    //  COLECCIONABLES
    // -------------------------------------------------------
    public void AñadirPuntos(TipoColeccionable tipo, int valor)
    {
        switch (tipo)
        {
            case TipoColeccionable.Moneda:
                monedas += valor;
                Debug.Log($"Monedas: {monedas}");
                break;
            default:
                puntos += valor;
                Debug.Log($"Puntos: {puntos}");
                break;
        }
 
        // Aquí puedes actualizar tu UI (texto de monedas, puntuación, etc.)
        // UIManager.Instancia?.ActualizarUI(monedas, puntos);
    }
 
    // -------------------------------------------------------
    //  GAME OVER
    // -------------------------------------------------------
    public void GameOver()
    {
        Debug.Log("GAME OVER");
 
        // Paramos la banda sonora
        MusicaManager.Instancia?.PararMusica();
 
        // Esperamos un poco y luego gestionamos la pantalla de Game Over
        Invoke(nameof(CargarGameOver), tiempoEsperaGameOver);
    }
 
    private void CargarGameOver()
    {
        if (!string.IsNullOrEmpty(escenaGameOver))
            SceneManager.LoadScene(escenaGameOver);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
    }
}