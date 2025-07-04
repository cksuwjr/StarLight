using UnityEngine;

public class DutorialStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Dutorial") == "Done")
        {
        }
        else
        {
            GameObject.Find("Inform1").SetActive(false);
            GameObject.Find("Inform2").SetActive(false);
            GameObject.Find("Inform3").SetActive(false);
            StartDutorial();
        }
    }

    private void StartDutorial()
    {
        ScenarioManager.Instance.SetChapter("Lobby");
        ScenarioManager.Instance.StartStory(1);
    }

}
