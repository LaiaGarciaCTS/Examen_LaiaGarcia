using UnityEngine;
 
 
public class MusicaManager : MonoBehaviour
{
    public static MusicaManager Instancia { get; private set; }
 
    private AudioSource audioSource;
 
    public AudioClip musicaNivel;
    public AudioClip musicaGameOver;
 
    [Range(0f, 1f)] public float volumen = 0.5f;
 
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
 
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
 
    public void ReproducirMusica(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;
 
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
 
    public void PararMusica() => audioSource.Stop();
 
    public void ReproducirGameOver()
    {
        if (musicaGameOver == null) return;
        audioSource.loop = false;
        audioSource.clip = musicaGameOver;
        audioSource.Play();
    }
 
    public void SetVolumen(float nuevoVolumen)
    {
        volumen = Mathf.Clamp01(nuevoVolumen);
        audioSource.volume = volumen;
    }
}