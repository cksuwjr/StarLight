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
                PlayerPrefs.SetString(key, "True");
                PlayerPrefs.Save();
                OnStoryEnd?.Invoke();
            };
        }
        else
        {
            PlayerPrefs.SetString("Dutorial", "Done");
            PlayerPrefs.Save();
            OnStart2?.Invoke();
        }
    }
}
