using UnityEngine;

public class Cenoura : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(1.5f, 1.0f, 0f);
    public float followSpeed = 3.0f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1.0f;

    private Vector3 targetPosition;
    private float floatTimer;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Cenoura: Nenhum player atribuído!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (player == null) return;

        floatTimer += Time.deltaTime * floatFrequency;
        float floatOffset = Mathf.Sin(floatTimer) * floatAmplitude;

        targetPosition = player.position + offset + new Vector3(0f, floatOffset, 0f);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
