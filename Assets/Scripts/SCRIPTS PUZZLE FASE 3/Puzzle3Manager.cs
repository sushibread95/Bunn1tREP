using UnityEngine;

public class Puzzle3Manager : MonoBehaviour
{
    public static Puzzle3Manager Instance;

    public ComponentConnector[] components;
    public Puzzle3IndicatorFeedback[] indicators;
    public PuzzleDoorMover doorMover;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckSolution()
    {
        bool allCorrect = true;

        foreach (ComponentConnector component in components)
        {
            if (component.currentSlot == null || !component.isConnected || component.componentID != component.currentSlot.expectedID)
            {
                allCorrect = false;
                break;
            }
        }

        // Atualiza os indicadores individualmente
        foreach (Puzzle3IndicatorFeedback indicator in indicators)
        {
            bool isCorrect = false;

            foreach (ComponentConnector component in components)
            {
                if (component.currentSlot != null &&
                    component.isConnected &&
                    component.componentID == component.currentSlot.expectedID &&
                    indicator.indicatorID == component.componentID)
                {
                    isCorrect = true;
                    break;
                }
            }

            indicator.SetConnected(isCorrect);
        }

        if (allCorrect && components.Length == indicators.Length)
        {
            Debug.Log("✅ Todos os componentes conectados corretamente!");

            foreach (Puzzle3IndicatorFeedback indicator in indicators)
            {
                indicator.SetAllConnected();
            }

            if (doorMover != null)
                doorMover.OpenDoor();
        }
    }
}
