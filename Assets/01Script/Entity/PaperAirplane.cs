using UnityEngine;

public class PaperAirplane : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.GetInt(name) == 1)
            Destroy(gameObject);
    }

    public void Dissappear()
    {
        PlayerPrefs.SetInt(name, 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
