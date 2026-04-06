using System.Collections;
using UnityEngine;

public class SequencePedestal : MonoBehaviour
{
    [Header("Identificación")]
    public int pedestalID;

    [Header("Materiales")]
    public Material normalMaterial;
    public Material highlightMaterial;

    [Header("Efectos")]
    public float highlightDuration = 0.3f;
    public AudioClip onHighlightSound;

    private Renderer meshRenderer;
    private SequencePuzzleManager puzzleManager;
    private AudioSource audioSource;
    private bool interactable = true;

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        puzzleManager = FindObjectOfType<SequencePuzzleManager>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && onHighlightSound != null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (meshRenderer != null && normalMaterial != null)
            meshRenderer.material = normalMaterial;

        // Por defecto, el pedestal no es interactuable hasta que el puzzle lo active
        SetInteractable(false);
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
        // Opcional: puedes activar/desactivar el collider también
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = value;
    }

    public void Highlight()
    {
        if (meshRenderer != null && highlightMaterial != null)
            StartCoroutine(HighlightCoroutine());
        if (audioSource != null && onHighlightSound != null)
            audioSource.PlayOneShot(onHighlightSound);
    }

    private IEnumerator HighlightCoroutine()
    {
        Material original = meshRenderer.material;
        meshRenderer.material = highlightMaterial;
        yield return new WaitForSeconds(highlightDuration);
        meshRenderer.material = original;
    }

    // Este método lo llama el sistema de interacción de Cardboard (CameraPointerManager)
    public void OnPointerClickXR()
    {
        if (!interactable) return;
        if (puzzleManager != null)
            puzzleManager.PedestalActivated(pedestalID);
    }
}