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
        string lastCorrectID = "";

        foreach (ComponentConnector component in components)
        {
            if (component.currentSlot != null && component.isConnected && component.componentID == component.currentSlot.expectedID)
            {
                lastCorrectID = component.componentID; // guarda o último ID correto
            }
            else
            {
                allCorrect = false;
            }
        }

        // Atualiza o terminal com a cor do último correto
        foreach (Puzzle3IndicatorFeedback indicator in indicators)
        {
            if (indicator.indicatorID == lastCorrectID)
            {
                indicator.SetConnected(true);
            }
            else
            {
                indicator.SetConnected(false);
            }
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
