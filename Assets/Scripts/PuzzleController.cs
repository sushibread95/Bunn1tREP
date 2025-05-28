using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private PuzzleWall paredeDaFase; // pode ser PuzzleWall ou PuzzleWall2

    private bool isSolved = false;

    public void SolvePuzzle()
    {
        if (isSolved) return;

        isSolved = true;
        Debug.Log("Puzzle resolvido!");

        if (paredeDaFase != null)
        {
            paredeDaFase.HandlePuzzleSolved();
        }
    }
}
