using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
 
    [Header("Puntuacion")]
    public int monedas = 0;
 
    [Header("Nombres exactos de las escenas — deben coincidir con los archivos .unity")]
    public string escenaNivel      = "Nivel2";
    public string escenaVictoria   = "Victoria";
    public string escenaGameOver   = "GameOver";
    public string escenaMenu       = "MenuPrincipal";
 
    [Header("Tiempo espera antes de cargar Game Over (segundos)")]
    public float tiempoEsperaGameOver = 1.5f;
 
    private bool tieneLlave = false;
 
    //  SINGLETON
    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }
 
    //  MONEDAS
    public void AñadirMoneda(int valor)
    {
        monedas += valor;
        UIManager.Instancia?.ActualizarMonedas(monedas);
    }
 
    //  LLAVE
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
 
    //  VICTORIA
    public void CargarSiguienteNivel(string nombreEscena)
    {
        MusicaManager.Instancia?.PararMusica();
        MusicaManager.Instancia?.ReproducirVictoria();
        CargarEscena(escenaVictoria);
    }
 
    //  GAME OVER
    public void GameOver()
    {
        Debug.Log("GAMEMANAGER: GAME OVER — cargando escena: " + escenaGameOver);
        MusicaManager.Instancia?.PararMusica();
        MusicaManager.Instancia?.ReproducirGameOver();
        Invoke(nameof(CargarEscenaGameOver), tiempoEsperaGameOver);
    }
 
    void CargarEscenaGameOver()
    {
        CargarEscena(escenaGameOver);
    }
 
    //  METODO CENTRAL para cargar escenas — con debug si falla
    void CargarEscena(string nombre)
    {
        Debug.Log("GAMEMANAGER: Intentando cargar escena '" + nombre + "'");
 
        // Comprueba si la escena existe en Build Settings
        if (Application.CanStreamedLevelBeLoaded(nombre))
        {
            SceneManager.LoadScene(nombre);
        }
        else
        {
            Debug.LogError("GAMEMANAGER: La escena '" + nombre + "' NO está en Build Settings. " +
                "Ve a File → Build Settings y añádela, o comprueba que el nombre coincide exactamente con el archivo .unity");
        }
    }

    //EXAMEN

    public List<GameObject> enemiesInScreen;

    void Update
    {
	    if(Input.GetKeyDown(KeyCode.K))
	    {
		    KillEnemiesInScreen();
	    }
    }

    void KillEnemiesInScreen()
    {
	    foreach (GameObject enemy in enemiesInScreen)
	    {
		    Destroy(enemy);
	    }
    }

    }