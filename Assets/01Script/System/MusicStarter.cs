using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip BGM;
    [SerializeField] private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartMusic", time);
    }

    private void StartMusic()
    {
        SoundManager.Instance.ChangeBGM(BGM);   
    }
}
