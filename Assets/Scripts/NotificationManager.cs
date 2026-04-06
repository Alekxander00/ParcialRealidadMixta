using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("UI References")]
    public GameObject notificationPanel;      // El mismo panel del tutorial o uno nuevo
    public TextMeshProUGUI notificationText;  // El mismo texto del tutorial o uno específico

    [Header("Configuración")]
    public float displayDuration = 3f;
    public AudioClip notificationSound;

    private AudioSource audioSource;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.Log("AudioSource creado automáticamente en " + gameObject.name);
        }

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        // Reproducir sonido
        if (audioSource != null && notificationSound != null)
            audioSource.PlayOneShot(notificationSound);

        // Mostrar panel y mensaje
        if (notificationPanel != null)
            notificationPanel.SetActive(true);
        
        if (notificationText != null)
            notificationText.text = message;

        // Esperar y ocultar
        yield return new WaitForSeconds(displayDuration);

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
        
        currentCoroutine = null;
    }
}