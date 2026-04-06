using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance;
    public GameObject Player;
    private GameObject lastTeleportPoint;

    private void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void DisableTeleportPoint(GameObject teleportPoint)
{
    if(lastTeleportPoint != null)
    {
        lastTeleportPoint.SetActive(true);
    }

    teleportPoint.SetActive(false);
    lastTeleportPoint = teleportPoint;
    
#if UNITY_EDITOR
    if (Player != null)
    {
        CardboardSimulator simulator = Player.GetComponent<CardboardSimulator>();
        if (simulator != null)
        {
            simulator.UpdatePlayerPositonSimulator(); // Mantén el nombre original si existe
        }
        else
        {
            Debug.LogWarning("CardboardSimulator no encontrado en el Player. Ignorando actualización.");
        }
    }
    else
    {
        Debug.LogWarning("Player no asignado en TeleportManager. Ignorando actualización del simulador.");
    }
#endif
}

}
