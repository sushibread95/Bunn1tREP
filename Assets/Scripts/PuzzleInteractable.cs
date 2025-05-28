using UnityEngine;

public class PuzzleInteractable : MonoBehaviour
{
    private bool isSolved = false;

    public void Interact()
    {
        if (isSolved) return;

        Debug.Log("Puzzle resolvido!");
        isSolved = true;

        // Aqui vamos avisar quem precisa saber que o puzzle foi resolvido
        PuzzleController controller = FindFirstObjectByType<PuzzleController>();
        if (controller != null)
        {
            controller.SolvePuzzle();
        }
    }
}
