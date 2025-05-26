using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DutorialStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartDutorial();
        if (PlayerPrefs.GetString("Dutorial") == "Done")
        {
        }
        else
        {
            StartDutorial();
            PlayerPrefs.SetString("Dutorial", "Done");
        }
    }

    private void StartDutorial()
    {
        ScenarioManager.Instance.SetChapter("Lobby");
        ScenarioManager.Instance.StartStory(1);
    }

}
