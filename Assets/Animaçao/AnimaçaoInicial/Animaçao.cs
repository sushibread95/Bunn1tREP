using UnityEngine;

    public class PlayAnimInstant : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Animator>().Play("Olhos", 0, 0f);
        }
    }
