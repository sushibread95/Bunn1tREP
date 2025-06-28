using UnityEngine;

public class FlashRelampago : MonoBehaviour
{
    public Light luz;
    public float minDelay = 3f;
    public float maxDelay = 8f;
    public float flashDuration = 0.1f;

    private void Start()
    {
        StartCoroutine(SimulaRelampagos());
    }

    private System.Collections.IEnumerator SimulaRelampagos()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Pode simular um ou mais flashes por raio
            int quantidadeFlashes = Random.Range(1, 4);
            for (int i = 0; i < quantidadeFlashes; i++)
            {
                luz.intensity = Random.Range(40f, 80f);
                luz.enabled = true;
                yield return new WaitForSeconds(flashDuration);
                luz.enabled = false;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }
        }
    }
}
