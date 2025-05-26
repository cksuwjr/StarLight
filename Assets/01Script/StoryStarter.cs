using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryStarter : MonoBehaviour
{
    [SerializeField] private string key;

    public UnityEvent OnStart;
    public UnityEvent OnStoryEnd;

    public UnityEvent OnStart2;

    private void Start()
    {
        if (PlayerPrefs.GetString(key) != "True")
        {
            OnStart?.Invoke();
            ScenarioManager.Instance.OnStoryEnd += () =>
            {
                OnStoryEnd?.Invoke();
                PlayerPrefs.SetString(key, "True");
                PlayerPrefs.Save();
            };
        }
        else
        {
            OnStart2?.Invoke();
        }
    }
}
