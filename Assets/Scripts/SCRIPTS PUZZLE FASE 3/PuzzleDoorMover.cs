using UnityEngine;

public class PuzzleDoorMover : MonoBehaviour
{
    public Vector3 targetOffset = new Vector3(2f, 0f, 0f); // Quanto mover (X negativo)
    public float moveSpeed = 0.5f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool moveDoor = false;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + targetOffset;
    }

    private void Update()
    {
        if (moveDoor)
        {
            Debug.Log("[PuzzleDoorMover] Movendo porta...");
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }

    public void OpenDoor()
    {
        moveDoor = true;
        Debug.Log("[PuzzleDoorMover] Porta ativada para abrir.");
    }
}
