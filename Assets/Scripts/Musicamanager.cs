using UnityEngine;
 
// Coloca este script en un GameObject vacío llamado "MusicaManager".
// Asígnale un AudioSource en el mismo objeto y configura los clips desde el Inspector.
 
public class MusicaManager : MonoBehaviour
{
    public static MusicaManager Instancia { get; private set; }
 
    [Header("Fuente de audio")]
    private AudioSource audioSource;
 
    [Header("Clips de música")]
    public AudioClip musicaNivel;     // Música principal del nivel
    public AudioClip musicaGameOver;  // Música / jingle de Game Over (opcional)
 
    [Header("Ajustes")]
    [Range(0f, 1f)] public float volumen = 0.5f;
 
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
        DontDestroyOnLoad(gameObject); // La música no se interrumpe al cambiar escena
 
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
 
        audioSource.loop   = true;
        audioSource.volume = volumen;
    }
 
    private void Start()
    {
        ReproducirMusica(musicaNivel);
    }
 
    // -------------------------------------------------------
    //  MÉTODOS PÚBLICOS
    // -------------------------------------------------------
 
    /// <summary>Reproduce un clip en bucle (banda sonora de nivel).</summary>
    public void ReproducirMusica(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;
 
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
 
    /// <summary>Para la música actual.</summary>
    public void PararMusica()
    {
        audioSource.Stop();
    }
 
    /// <summary>Cambia a la música de Game Over (sin bucle).</summary>
    public void ReproducirGameOver()
    {
        if (musicaGameOver == null) return;
 
        audioSource.loop = false;
        audioSource.clip = musicaGameOver;
        audioSource.Play();
    }
 
    /// <summary>Ajusta el volumen en tiempo de ejecución (útil para opciones).</summary>
    public void SetVolumen(float nuevoVolumen)
    {
        volumen = Mathf.Clamp01(nuevoVolumen);
        audioSource.volume = volumen;
    }
}