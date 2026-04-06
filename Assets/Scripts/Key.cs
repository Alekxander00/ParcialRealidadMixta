using UnityEngine;

public class Key : GrabObject
{
    public bool isCollected = false;
    private bool hasNotifiedDoor = false;
    public DoorWithKey targetDoor;  // Asigna la puerta en el Inspector

    public override void Grab()
    {
        if (isCollected) return;

        // Ejecutar el agarre físico (sonido, desactivar collider, etc.)
        base.Grab();

        // Notificar a la puerta
        if (!hasNotifiedDoor && targetDoor != null)
        {
            targetDoor.SetHasKey(true);
            isCollected = true;
            hasNotifiedDoor = true;
            Debug.Log("🔑 Llave recogida. Puerta desbloqueada.");
        }

        // 👇 NOTIFICAR AL TUTORIAL (esto es lo que faltaba)
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.CompleteKey();
            Debug.Log("📢 Tutorial: tarea de llave completada");
        }
        else
        {
            Debug.LogWarning("TutorialManager.Instance es nulo, no se pudo notificar.");
        }
    }
}