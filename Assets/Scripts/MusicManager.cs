using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Música por escena")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.loop = true;
    }

    private void Start()
    {
        // Reproducir música según la escena actual
        PlayMusicForCurrentScene();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip clipToPlay = null;

        if (sceneName.Contains("Menu") || sceneName == "Menu" || sceneName == "MainMenu")
            clipToPlay = menuMusic;
        else if (sceneName.Contains("Nivel") || sceneName == "Level" || sceneName == "EscenaJuego")
            clipToPlay = levelMusic;

        if (clipToPlay != null && audioSource.clip != clipToPlay)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
            Debug.Log($"🎵 Reproduciendo música: {clipToPlay.name}");
        }
    }
}