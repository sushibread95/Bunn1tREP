using UnityEngine;

public class SimpleCameraTouch : MonoBehaviour
{
    public Transform target; // O player
    public float rotationSpeed = 0.2f;
    public float resetSpeed = 2f; // Velocidade do reset
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 20f;

    private float defaultYaw = 0f;
    private float defaultPitch = 20f;

    private bool isTouching = false;

    void LateUpdate()
    {
        if (target == null) return;

        // Segue a posição do player
        transform.position = target.position;

        // Controle por toque
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                if (touch.position.x > Screen.width * 0.5f)
                {
                    isTouching = true;
                    Vector2 delta = touch.deltaPosition;

                    yaw += delta.x * rotationSpeed;
                    pitch += delta.y * rotationSpeed;
                    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
        else
        {
            isTouching = false;
        }

        // Se não estiver tocando, faz o reset suave
        if (!isTouching)
        {
            yaw = Mathf.Lerp(yaw, defaultYaw, Time.deltaTime * resetSpeed);
            pitch = Mathf.Lerp(pitch, defaultPitch, Time.deltaTime * resetSpeed);
        }

        // Aplica rotação
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
