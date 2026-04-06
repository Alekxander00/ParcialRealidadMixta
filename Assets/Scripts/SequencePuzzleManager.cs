using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencePuzzleManager : MonoBehaviour
{
    [Header("Configuración")]
    public List<SequencePedestal> pedestals;
    public float delayBetweenSteps = 0.8f;
    public int sequenceLength = 4;
    public UnityEvent OnPuzzleSolved;

    [Header("Estado")]
    public bool puzzleActive = false;

    private List<int> currentSequence = new List<int>();
    private int playerStepIndex = 0;
    private bool isPlayingSequence = false;
    private bool waitingForPlayer = false;

    void Start()
    {
        // Inicialmente el puzzle está inactivo, los pedestales no reaccionan
        SetPedestalsInteractable(false);
    }

    // Método público para activar el puzzle (lo llamará el TeleportPoint)
    public void ActivatePuzzle()
    {
        if (puzzleActive) return;
        puzzleActive = true;
        SetPedestalsInteractable(true);
        GenerateNewSequence();
        StartCoroutine(PlaySequence());
        Debug.Log("🎮 Puzzle de secuencia activado");
    }

    private void SetPedestalsInteractable(bool interactable)
    {
        foreach (var pedestal in pedestals)
        {
            if (pedestal != null)
                pedestal.SetInteractable(interactable);
        }
    }

    private void GenerateNewSequence()
    {
        currentSequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomPedestalIndex = Random.Range(0, pedestals.Count);
            currentSequence.Add(randomPedestalIndex);
        }
        Debug.Log($"Secuencia generada: {string.Join(", ", currentSequence)}");
    }

    private IEnumerator PlaySequence()
    {
        isPlayingSequence = true;
        waitingForPlayer = false;
        Debug.Log("🎵 Reproduciendo secuencia...");

        foreach (int pedestalIndex in currentSequence)
        {
            pedestals[pedestalIndex].Highlight();
            yield return new WaitForSeconds(delayBetweenSteps);
        }

        Debug.Log("👆 Ahora es tu turno");
        isPlayingSequence = false;
        waitingForPlayer = true;
        playerStepIndex = 0;
    }

    

    public void PedestalActivated(int activatedIndex)
    {
        if (!puzzleActive || isPlayingSequence || !waitingForPlayer) return;

        pedestals[activatedIndex].Highlight();
        Debug.Log($"Jugador activó pedestal {activatedIndex}. Esperaba {currentSequence[playerStepIndex]}");

        if (activatedIndex == currentSequence[playerStepIndex])
        {
            playerStepIndex++;
            if (playerStepIndex >= currentSequence.Count)
            {
                Debug.Log("🎉 Secuencia correcta. ¡Puzzle resuelto!");
                waitingForPlayer = false;
                OnPuzzleSolved?.Invoke();
                // Opcional: desactivar el puzzle para que no se pueda volver a interactuar
                puzzleActive = false;
                SetPedestalsInteractable(false);
            }
        }
        else
        {
            Debug.Log("❌ Secuencia incorrecta. Reiniciando...");
            waitingForPlayer = false;
            StartCoroutine(RestartPuzzle());
        }
    }

    private IEnumerator RestartPuzzle()
    {
        yield return new WaitForSeconds(1f);
        playerStepIndex = 0;
        StartCoroutine(PlaySequence());
    }
}