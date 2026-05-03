using UnityEngine;
using UnityEngine.SceneManagement;
 
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
 
    [Header("Puntuacion")]
    public int monedas = 0;
 
    [Header("Game Over")]
    public float tiempoEsperaGameOver = 1.5f;
    public string escenaGameOver = "GameOver";
 
    private bool tieneLlave = false;
 
    //  SINGLETON
    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Debug.LogWarning("GAMEMANAGER: ya existe una instancia, destruyendo duplicado en " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        Debug.Log("GAMEMANAGER: instancia creada correctamente en " + gameObject.name);
    }
 
    //  MONEDAS
    public void AñadirMoneda(int valor)
    {
        monedas += valor;
        Debug.Log("GAMEMANAGER: Monedas = " + monedas);
        UIManager.Instancia?.ActualizarMonedas(monedas);
    }
 
    //  LLAVE
    public void RecogerLlave()
    {
        tieneLlave = true;
        Debug.Log("GAMEMANAGER: Llave recogida. tieneLlave = " + tieneLlave);
    }
 
    public bool TieneLlave()
    {
        Debug.Log("GAMEMANAGER: TieneLlave consultado = " + tieneLlave);
        return tieneLlave;
    }
 
    public void UsarLlave()
    {
        tieneLlave = false;
        Debug.Log("GAMEMANAGER: Llave usada. tieneLlave = " + tieneLlave);
    }
 
    //  SIGUIENTE NIVEL
    public void CargarSiguienteNivel(string nombreEscena)
    {
        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
        else
            Debug.LogWarning("GAMEMANAGER: No hay escena asignada en la Puerta.");
    }
 
    //  GAME OVER
    public void GameOver()
    {
        Debug.Log("GAMEMANAGER: GAME OVER");
        MusicaManager.Instancia?.PararMusica();
        MusicaManager.Instancia?.ReproducirGameOver();
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