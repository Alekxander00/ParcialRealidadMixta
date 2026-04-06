using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private string nombreEscenaJuego = "EscenaJuego";
    [SerializeField] private string nombreEscenaMenu = "EscenaMenu";
    [SerializeField] private string nombreSiguienteNivel = "Nivel2"; // Agrega el nombre de tu siguiente nivel

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CargarJuego()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void CargarMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    // Método que puedes llamar desde el TeleportPoint
    public void CargarSiguienteNivel()
    {
        // Opción 1: Por nombre (recomendado, más claro)
        if (!string.IsNullOrEmpty(nombreSiguienteNivel))
        {
            SceneManager.LoadScene(nombreSiguienteNivel);
        }
        else
        {
            // Opción 2: Por índice (alternativa)
            int siguienteIndice = SceneManager.GetActiveScene().buildIndex + 1;
            if (siguienteIndice < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(siguienteIndice);
            else
                Debug.Log("No hay más niveles. ¡Felicidades!");
        }
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}