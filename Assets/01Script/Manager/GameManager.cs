using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    private DataManager dataManager;
    private UIManager uiManager;
    private PoolManager poolManager;
    private SoundManager soundManager;
    private Volume volume;

    [SerializeField] private GameObject player;
    public GameObject Player 
    {
        get 
        {
            if(player == null)
                player = GameObject.Find("Player");
            return player; 
        } 
    }

    public Action OnTimerStop;

    protected override void DoAwake()
    {
        SceneManager.sceneLoaded += (scene, mode) => {
            AssignManagers();
            InitManagers();

            if (!Player) return;
            if(Camera.main.GetComponent<CameraMove>())
                Camera.main.GetComponent<CameraMove>().Init();

            if(player.TryGetComponent<PlayerController>(out var p))
                p.Init();

            Time.timeScale = 1f;
        };
    }

    private void AssignManagers()
    {
        GameObject.Find("DataManager")?.TryGetComponent<DataManager>(out dataManager);
        GameObject.Find("SoundManager")?.TryGetComponent<SoundManager>(out soundManager);
        GameObject.Find("UIManager")?.TryGetComponent<UIManager>(out uiManager);
        GameObject.Find("PoolManager")?.TryGetComponent<PoolManager>(out poolManager);
        GameObject.Find("Global Volume")?.TryGetComponent<Volume>(out volume);
    }

    private void InitManagers()
    {
        dataManager?.Init();
        uiManager?.Init();
        poolManager?.Init();
        soundManager?.Init();
    }

    public void Blur(bool tf)
    {
        //volume.enabled = tf;
    }

    public void GameOver()
    {
        StopTimer();
        UIManager.Instance.OpenClearPopup(false, () => 
        { 
            LoadingSceneManager.SetNextScene(PlayerPrefs.GetString("Scene"));
            SceneManager.LoadScene("LoadingScene");
        });
    }

    public void SetTimer(float second, Action action = null, string goal = "")
    {
        if(action != null)
            OnTimerStop += action;
        uiManager.SetGoal(goal);
        StartCoroutine("Timer", second);
    }

    public void StopTimer()
    {
        OnTimerStop = null;
        StopCoroutine("Timer");
        uiManager.SetTimer(-1);
    }

    private IEnumerator Timer(float second)
    {
        var timer = second;

        while(timer > 0)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            timer -= 1f;
            uiManager.SetTimer(timer);
        }
        OnTimerStop?.Invoke();
        OnTimerStop = null;
    }

    public void MusicOnOFF(bool tf)
    {
        soundManager?.SoundOnOFF(tf);
    }

    public void StopResume(bool tf)
    {
        if(!tf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        LoadingSceneManager.SetNextScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LoadingScene");
    }

    public void ReturnToLobby()
    {
        LoadingSceneManager.SetNextScene("LobbyScene");
        SceneManager.LoadScene("LoadingScene");
    }

    public void GameQuit()
    {
        Application.Quit();
    }


    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        LoadingSceneManager.SetNextScene(sceneName);
        SceneManager.LoadScene("LoadingScene");
    }


    public void SavePosition()
    {
        PlayerPrefs.SetString("SaveScene", SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

        PlayerPrefs.Save();
    }

    public void LoadPosition()
    {
        var x = PlayerPrefs.GetFloat("PlayerX");
        var y = PlayerPrefs.GetFloat("PlayerY");
        var z = PlayerPrefs.GetFloat("PlayerZ");

        if (new Vector3(x, y, z) == Vector3.zero) return;
        if (new Vector3(x, y, z) == new Vector3(-1f, -1f, -1f)) return;

        Debug.Log("불러오기" + new Vector3(x, y, z));


        player.transform.position = new Vector3(x, y, z);
        
        PlayerPrefs.SetFloat("PlayerX", -1);
        PlayerPrefs.SetFloat("PlayerY", -1);
        PlayerPrefs.SetFloat("PlayerZ", -1);

        PlayerPrefs.Save();


    }
}
