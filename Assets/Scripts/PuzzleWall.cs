using UnityEngine;
using System.Collections;

public class PuzzleWall : MonoBehaviour
{
    [Header("Mensagens")]
    [SerializeField] private GameObject successMessageUI;
    [SerializeField] private GameObject darknessMessageUI;

    [Header("Teleporte")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float teleportDelay = 1.0f;
    [SerializeField] private float messageDuration = 3.0f;

    private bool puzzleSolved = false;
    private bool isTeleporting = false;

    public void HandlePuzzleSolved()
    {
        puzzleSolved = true;
        gameObject.SetActive(false);

        if (successMessageUI != null)
        {
            successMessageUI.SetActive(true);
            Invoke(nameof(HideSuccessMessage), messageDuration);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!puzzleSolved && !isTeleporting && other.CompareTag("Player"))
        {
            Debug.Log("Jogador dentro da zona escura, iniciando teleporte.");
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        isTeleporting = true;

        if (darknessMessageUI != null)
            darknessMessageUI.SetActive(true);

        if (ScreenFader.Instance != null)
            ScreenFader.Instance.FadeIn();

        yield return new WaitForSeconds(teleportDelay);

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.MovePosition(respawnPoint.position);
            }
        }

        yield return new WaitForSeconds(0.2f);

        if (ScreenFader.Instance != null)
            ScreenFader.Instance.FadeOut();

        yield return new WaitForSeconds(messageDuration - teleportDelay);

        if (darknessMessageUI != null)
            darknessMessageUI.SetActive(false);

        isTeleporting = false;
    }

    private void HideSuccessMessage()
    {
        if (successMessageUI != null)
            successMessageUI.SetActive(false);
    }
}
