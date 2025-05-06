using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private Image visualLoadingBar;


    private GameObject runner;
    private GameObject postBox;

    private Vector3 startPos;
    private Vector3 endPos;

    private RectTransform runnerRect;

    private static string nextSceneName;
    private AsyncOperation asyncScene;

    private float speed = 1f;

    private void Awake()
    {
        postBox = visualLoadingBar.transform.GetChild(0).gameObject;
        runner = visualLoadingBar.transform.GetChild(1).gameObject;

        runnerRect = runner.GetComponent<RectTransform>();

        startPos = runnerRect.position;

        endPos = postBox.GetComponent<RectTransform>().position;
        startPos.y = endPos.y;

        StartCoroutine("SetSpeed");
        StartCoroutine("LoadSceneAsync");
    }

    private IEnumerator SetSpeed()
    {
        speed = 80f;
        yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.8f, 1.2f));
        speed = 0;
        yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.1f, 1.0f));
        while (true) 
        {
            speed = Random.Range(1f, 30f);
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 3f));
            speed = 0;
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.1f, 1.0f));
        }
    }


    public static void SetNextScene(string sceneName)
    {
        Time.timeScale = 1f;
        nextSceneName = sceneName;
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(0.3f);
        asyncScene = SceneManager.LoadSceneAsync(nextSceneName);
        asyncScene.allowSceneActivation = false;

        var speedC = 0f;
        var timeC = 0.0f;
        while(!asyncScene.isDone)
        {
            timeC += Time.deltaTime * (0.0001f * speed);
            speedC = speed == 0 ? 0.0001f : timeC;

            //Debug.Log(speedC);

            if(asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, speedC);

                runnerRect.position = new Vector3(Mathf.Lerp(runnerRect.position.x, endPos.x, speedC), runnerRect.position.y, runnerRect.position.z);
                if (loadingBar.fillAmount > 0.99f)
                    SceneLoadEnd();
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, speedC);
                runnerRect.position = new Vector3(Mathf.Lerp(runnerRect.position.x, endPos.x, speedC), runnerRect.position.y, runnerRect.position.z);

                if (loadingBar.fillAmount >= asyncScene.progress)
                    timeC = 0.0f;
            }

            visualLoadingBar.fillAmount = 1 - loadingBar.fillAmount;
            yield return null;
        }
    }

    private void SceneLoadEnd()
    {
        StopCoroutine("LoadSceneAsync");
        StopCoroutine("Speed");

        StartCoroutine("Effect");
    }

    private IEnumerator Effect()
    {
        //var time = 0f;
        //var startX = runnerRect.position.x;
        //while (time < 1f)
        //{
        //    time += Time.deltaTime;
        //    runnerRect.position = new Vector3(Mathf.Lerp(startX, endPos.x, time), runnerRect.position.y, runnerRect.position.z);
        //    yield return null;
        //}
        yield return YieldInstructionCache.WaitForSeconds(0.05f);
        asyncScene.allowSceneActivation = true;
    }
}
