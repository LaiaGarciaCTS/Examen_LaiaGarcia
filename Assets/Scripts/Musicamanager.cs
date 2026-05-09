using UnityEngine;
 
// =====================================================================
//  MUSICA MANAGER
//
//  Colocar en un GameObject vacío llamado "MusicaManager".
//  Añadir componente AudioSource en el Inspector.
//
//  En el Inspector asignar:
//    - musicaNivel    → música del nivel (loop)
//    - musicaGameOver → música de derrota
//    - musicaVictoria → música de victoria
// =====================================================================
 
public class MusicaManager : MonoBehaviour
{
    public static MusicaManager Instancia { get; private set; }
 
    [Header("Musica")]
    public AudioClip musicaNivel;
    public AudioClip musicaGameOver;
    public AudioClip musicaVictoria;
 
    [Header("Volumen")]
    [Range(0f, 1f)] public float volumen = 0.5f;
 
    private AudioSource _audioSource;
 
    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
 
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
 
        _audioSource.loop   = true;
        _audioSource.volume = volumen;
    }
 
    void Start() => ReproducirMusica(musicaNivel);
 
    public void ReproducirMusica(AudioClip clip)
    {
        if (clip == null) return;
        if (_audioSource.clip == clip && _audioSource.isPlaying) return;
        _audioSource.loop = true;
        _audioSource.clip = clip;
        _audioSource.Play();
    }
 
    public void PararMusica() => _audioSource.Stop();
 
    public void ReproducirGameOver()
    {
        if (musicaGameOver == null) return;
        _audioSource.loop = false;
        _audioSource.clip = musicaGameOver;
        _audioSource.Play();
    }
 
    public void ReproducirVictoria()
    {
        if (musicaVictoria == null) return;
        _audioSource.loop = false;
        _audioSource.clip = musicaVictoria;
        _audioSource.Play();
    }
 
    public void SetVolumen(float v)
    {
        volumen = Mathf.Clamp01(v);
        _audioSource.volume = volumen;
    }
}