using UnityEngine;

public class TeleportToPuzzleZone : MonoBehaviour
{
    [Header("Referencias")]
    public SequencePuzzleManager puzzleManager;  // Arrastra el objeto con el SequencePuzzleManager
    public TeleportPoint teleportPoint;          // Opcional: si quieres usar el teleport existente

    private void Start()
    {
        // Si no asignaste teleportPoint, intenta obtener el componente del mismo GameObject
        if (teleportPoint == null)
            teleportPoint = GetComponent<TeleportPoint>();
        
        // Suscribirse al evento de teleport si existe (puedes modificar TeleportPoint para que tenga un UnityEvent)
        if (teleportPoint != null)
        {
            // Opción A: si TeleportPoint tiene un UnityEvent OnTeleport, lo usamos
            teleportPoint.OnTeleport.AddListener(ActivatePuzzle);
        }
    }

    // Método público para activar el puzzle (puede llamarse desde OnPointerClickXR del teleport)
    public void ActivatePuzzle()
    {
        if (puzzleManager != null)
        {
            puzzleManager.ActivatePuzzle();
            Debug.Log("🔓 Puzzle activado desde el punto de teletransporte");
        }
        else
        {
            Debug.LogWarning("No se ha asignado el SequencePuzzleManager en TeleportToPuzzleZone");
        }
    }

    // Si tu TeleportPoint no tiene evento, puedes llamar a este método desde su OnPointerClickXR
    // Para eso, modifica tu TeleportPoint existente para que llame a este método.
}