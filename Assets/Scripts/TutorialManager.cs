using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI tutorialText;
    public GameObject tutorialPanel;

    [Header("Tracking")]
    private bool teleportUsed = false;
    private bool keyGrabbed = false;
    private bool doorOpened = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateTutorialUI();
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    // Métodos públicos que llamarán desde otros scripts
    public void CompleteTeleport()
    {
        if (!teleportUsed)
        {
            teleportUsed = true;
            UpdateTutorialUI();
            Debug.Log("✅ Tutorial: Teletransporte completado");
        }
    }

    public void CompleteKey()
    {
        if (!keyGrabbed)
        {
            keyGrabbed = true;
            UpdateTutorialUI();
            Debug.Log("✅ Tutorial: Llave agarrada");
        }
    }

    public void CompleteDoor()
    {
        if (!doorOpened)
        {
            doorOpened = true;
            UpdateTutorialUI();
            Debug.Log("✅ Tutorial: Puerta abierta");
        }
    }

    private void UpdateTutorialUI()
    {
        if (tutorialText == null) return;

        string teleportStatus = teleportUsed ? "✓" : "◌";
        string keyStatus = keyGrabbed ? "✓" : "◌";
        string doorStatus = doorOpened ? "✓" : "◌";

        // Dentro de UpdateTutorialUI, usando <color> tags
tutorialText.text = $"📜 TUTORIAL:\n" +
    $"{(teleportUsed ? "<color=green>✓</color>" : "◌")} Mira los círculos para teletransportarte\n" +
    $"{(keyGrabbed ? "<color=green>✓</color>" : "◌")} Busca la llave de la puerta\n" +
    $"{(doorOpened ? "<color=green>✓</color>" : "◌")} Mira la puerta para abrirla";

        // Opcional: si todo está completado, ocultar el panel después de 3 segundos
        if (teleportUsed && keyGrabbed && doorOpened)
        {
            Invoke("HideTutorial", 3f);
        }
    }

    private void HideTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
}