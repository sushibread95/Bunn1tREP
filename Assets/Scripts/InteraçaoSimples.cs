using UnityEngine;

public class ObjetoInterativo : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private string triggerAnim = "Ativar";
    [SerializeField] private float tempoAtivo = 3f;
    [SerializeField] private int maxAtivosAoMesmoTempo = 1;

    [Header("Referências")]
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] private AudioSource som;

    private static int ativosNoMomento = 0;
    private bool emUso = false;
    private bool jaInteragiu = false;
    private bool playerNaArea = false;

    public void BotaoInteragir()
    {
        if (playerNaArea && !emUso && !jaInteragiu && ativosNoMomento < maxAtivosAoMesmoTempo)
        {
            Ativar();
        }
    }

    // NOVO método para o botão HUD chamar SEMPArecer fora da área
    public void BotaoInteragirViaHUD()
    {
        if (playerNaArea)
        {
            BotaoInteragir();
        }
        else
        {
            Debug.Log("Player não está na área, botão ignorado");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaArea = false;
        }
    }

    private void Ativar()
    {
        emUso = true;
        jaInteragiu = true;
        ativosNoMomento++;

        // Animações
        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            if (!string.IsNullOrEmpty(triggerAnim))
                anim.SetTrigger(triggerAnim);
        }

        // VFX e som
        if (vfx != null) vfx.Play();
        if (som != null) som.Play();

        Invoke(nameof(Resetar), tempoAtivo);
    }

    private void Resetar()
    {
        emUso = false;
        ativosNoMomento--;
    }
}
