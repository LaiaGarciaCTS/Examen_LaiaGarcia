using UnityEngine;
using UnityEngine.SceneManagement;
 
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
 
    [Header("Puntuacion")]
    public int monedas = 0;
 
    [Header("Escenas")]
    public string escenaVictoria = "Victoria";
    public string escenaGameOver = "GameOver";
 
    private bool tieneLlave = false;
 
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
        UIManager.Instancia?.ActualizarMonedas(monedas);
    }
 
    // ------------------------------------------------------------------
    //  LLAVE
    // ------------------------------------------------------------------
    public void RecogerLlave()
    {
        tieneLlave = true;
        Debug.Log("GAMEMANAGER: Llave recogida.");
    }
 
    public bool TieneLlave() => tieneLlave;
 
    public void UsarLlave()
    {
        tieneLlave = false;
        Debug.Log("GAMEMANAGER: Llave usada.");
    }
 
    // ------------------------------------------------------------------
    //  VICTORIA — para música y carga escena de victoria directamente
    // ------------------------------------------------------------------
    public void CargarSiguienteNivel(string nombreEscena)
    {
        MusicaManager.Instancia?.PararMusica();
        MusicaManager.Instancia?.ReproducirVictoria();
        SceneManager.LoadScene(escenaVictoria);
    }
 
    // ------------------------------------------------------------------
    //  GAME OVER — para música y carga escena de game over directamente
    // ------------------------------------------------------------------
    public void GameOver()
    {
        MusicaManager.Instancia?.PararMusica();
        MusicaManager.Instancia?.ReproducirGameOver();
        SceneManager.LoadScene(escenaGameOver);
    }
}