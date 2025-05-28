using UnityEngine;
using UnityEngine.UI;

public class ClimbableWire : MonoBehaviour
{
    public Transform topPoint;
    public float climbSpeed = 2f;
    public GameObject messageUI;
    public Button exitButton; // <-- Bot�o para sair da corda
    private bool playerInRange = false;
    private bool isClimbing = false;
    private bool isAtTop = false;
    private GameObject player;
    private PlayerController playerController;
    private Rigidbody playerRb;
    private Animator playerAnimator;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(false); // Esconde o bot�o no come�o
            exitButton.onClick.AddListener(ExitClimb); // Liga o bot�o com a fun��o de sair
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
            playerRb = player.GetComponent<Rigidbody>();
            playerAnimator = player.GetComponent<Animator>();
            playerInRange = true;

            if (messageUI != null)
                messageUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isClimbing = false;
            isAtTop = false;

            if (playerRb != null)
                playerRb.useGravity = true;

            if (playerAnimator != null)
                playerAnimator.SetBool("isClimbing", false);

            if (messageUI != null)
                messageUI.SetActive(false);

            if (exitButton != null)
                exitButton.gameObject.SetActive(false);
        }
    }

    private void Update()
{
    if (playerInRange && playerController != null && playerController.IsHoldingInteract() && !isClimbing)
    {
        isClimbing = true;

        if (messageUI != null)
            messageUI.SetActive(false);

        if (playerRb != null)
            playerRb.useGravity = false;

        if (playerAnimator != null)
            playerAnimator.SetBool("isClimbing", true);
    }

    if (isClimbing && player != null && !isAtTop)
    {
        player.transform.position = Vector3.SmoothDamp(player.transform.position, topPoint.position, ref velocity, 0.3f, climbSpeed);

        if (Vector3.Distance(player.transform.position, topPoint.position) < 0.05f)
        {
            isAtTop = true;
            isClimbing = false;

            if (exitButton != null)
                exitButton.gameObject.SetActive(true); // Mostra o botão para sair
        }
    }
}


    public void ExitClimb()
    {
        if (player != null)
        {
            if (playerRb != null)
                playerRb.useGravity = true;

            if (playerAnimator != null)
                playerAnimator.SetBool("isClimbing", false);

            if (exitButton != null)
                exitButton.gameObject.SetActive(false);

            isAtTop = false;
            playerInRange = false;
        }
    }
}
