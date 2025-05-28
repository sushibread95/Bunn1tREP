using UnityEngine;
using System.Collections;

public class CablePuzzleManager : MonoBehaviour
{
    public static CablePuzzleManager Instance;

    public CableConnector[] components;
    public CableMonitorFeedback[] indicators;

    [Header("Referências da Fase 2")]
    public PuzzleWall2 linkedPuzzleWall2; // Referência à parede/restrição da Fase 2

    [Header("Delay de Mudança")]
    public float finalColorDelay = 1.5f; // Tempo antes de mudar para a cor final e mostrar mensagem
    public int blinkCount = 3; // Quantidade de piscadas antes da cor final
    public float blinkInterval = 0.3f; // Intervalo entre as piscadas

    private void Awake()
    {
        Instance = this;
    }

    public void CheckSolution()
    {
        bool allCorrect = true;

        foreach (CableConnector component in components)
        {
            if (component.currentSlot != null && component.isConnected && component.cableID == component.currentSlot.expectedID)
            {
                // Atualiza o monitor correspondente IMEDIATAMENTE
                foreach (CableMonitorFeedback indicator in indicators)
                {
                    if (indicator.monitorID == component.cableID)
                    {
                        indicator.SetConnected(true);
                    }
                }
            }
            else
            {
                allCorrect = false;
            }
        }

        if (allCorrect && components.Length == indicators.Length)
        {
            Debug.Log("\u2705 Puzzle RGB resolvido!");

            StartCoroutine(FinalizePuzzleWithBlink());
        }
    }

    private IEnumerator FinalizePuzzleWithBlink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            foreach (CableMonitorFeedback indicator in indicators)
            {
                indicator.SetConnected(false); // Desliga a cor
            }
            yield return new WaitForSeconds(blinkInterval);

            foreach (CableMonitorFeedback indicator in indicators)
            {
                indicator.SetConnected(true); // Liga a cor RGB novamente
            }
            yield return new WaitForSeconds(blinkInterval);
        }

        yield return new WaitForSeconds(finalColorDelay);

        foreach (CableMonitorFeedback indicator in indicators)
        {
            indicator.SetAllConnected();
        }

        // Remove restrição da Fase 2
        if (linkedPuzzleWall2 != null)
        {
            linkedPuzzleWall2.HandlePuzzleSolved();
        }
    }
}

