using UnityEngine;

public class SairDoJogo : MonoBehaviour
{
    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo (editor não fecha, mas build sim)");
    }
}
