using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
    public void MoveToScene(string value)
    {
        PlayerPrefs.DeleteAll();

        LoadingSceneManager.SetNextScene(value);
        SceneManager.LoadScene("LoadingScene");
    }
}
