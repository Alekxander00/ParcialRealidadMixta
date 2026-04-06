using System.Collections;
using UnityEngine;

public class DoorWithKey : MonoBehaviour
{
    [Header("Configuración de rotación")]
    public Transform doorTransform;
    public Vector3 closedRotation = Vector3.zero;
    public Vector3 openRotation = new Vector3(0, -100, 0);
    public float openSpeed = 2f;

    [Header("Efecto de intento fallido")]
    public float shakeAngle = 10f;
    public float shakeDuration = 0.3f;

    [Header("Requisitos")]
    public bool requiresKey = true;
    public bool hasKey = false;

    [Header("Sonidos")]
    public AudioClip openSound;        // Sonido al abrir la puerta
    public AudioClip failSound;        // Sonido opcional al intentar sin llave

    private bool isOpen = false;
    private bool isAnimating = false;
    private Quaternion targetClosedRot;
    private Quaternion targetOpenRot;
    private AudioSource audioSource;

    void Start()
    {
        if (doorTransform == null)
            doorTransform = transform;

        targetClosedRot = Quaternion.Euler(closedRotation);
        targetOpenRot = Quaternion.Euler(openRotation);
        doorTransform.localRotation = targetClosedRot;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (openSound != null || failSound != null))
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerClickXR()
    {
        if (isAnimating) return;

        if (requiresKey && !hasKey)
        {
            if (failSound != null && audioSource != null)
                audioSource.PlayOneShot(failSound);
            StartCoroutine(ShakeCoroutine());
        }
        else
        {
            if (requiresKey && hasKey)
            {
                ConsumeKey();
                if (TutorialManager.Instance != null)
                    TutorialManager.Instance.CompleteDoor();
            }
            // Reproducir sonido de apertura (solo si no está ya abierta)
            if (!isOpen && openSound != null && audioSource != null)
                audioSource.PlayOneShot(openSound);
            StartCoroutine(RotateDoor(!isOpen));
        }
    }

    IEnumerator RotateDoor(bool open)
    {
        isAnimating = true;
        Quaternion startRot = doorTransform.localRotation;
        Quaternion endRot = open ? targetOpenRot : targetClosedRot;
        float duration = Vector3.Angle(startRot.eulerAngles, endRot.eulerAngles) / openSpeed;
        if (duration <= 0) duration = 0.5f;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            doorTransform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        doorTransform.localRotation = endRot;
        isOpen = open;
        isAnimating = false;
    }

    IEnumerator ShakeCoroutine()
    {
        isAnimating = true;
        Quaternion originalRot = doorTransform.localRotation;
        float halfDuration = shakeDuration / 2f;
        Quaternion shakeRot = Quaternion.Euler(closedRotation.x, closedRotation.y + shakeAngle, closedRotation.z);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / halfDuration;
            doorTransform.localRotation = Quaternion.Slerp(originalRot, shakeRot, t);
            yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / halfDuration;
            doorTransform.localRotation = Quaternion.Slerp(shakeRot, originalRot, t);
            yield return null;
        }
        doorTransform.localRotation = originalRot;
        isAnimating = false;
    }

    private void ConsumeKey()
    {
        Key key = FindObjectOfType<Key>();
        if (key != null)
        {
            if (GrabManager.Instance != null && GrabManager.Instance.heldItem == key.gameObject)
                GrabManager.Instance.heldItem = null;
            Destroy(key.gameObject);
            Debug.Log("🗝️ Llave consumida al abrir la puerta.");
        }
        else
        {
            Debug.LogWarning("No se encontró la llave para destruir.");
        }
    }

    public void SetHasKey(bool value)
    {
        hasKey = value;
    }
}