using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LogSpawner : MonoBehaviour
{
    private GameObject log;
    [SerializeField] private Vector3 spawnPosition;
    private float endTime;
    private float logSpeed = 2.5f;

    public UnityEvent OnFaze1End;
    public UnityEvent OnFaze2End;

    private List<GameObject> logs = new List<GameObject>();

    private void Start()
    {
        StartSpawn();
    }

    private void StartSpawn()
    {
        StartCoroutine("Spawn");
    }

    private void StopSpawn()
    {
        StopCoroutine("Spawn");
        for (int i = logs.Count - 1; i >= 0; i--)
            logs[i].GetComponent<LogMove>().ReturnToPool();
    }

    private IEnumerator Spawn()
    {
        UIManager.Instance.FloatImage(Resources.Load<Sprite>("Image/Stell_Log"));

        yield return YieldInstructionCache.WaitForSeconds(3f);


        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Log"), LayerMask.NameToLayer("Log"), false);


        endTime = Time.time + 60;
        GameManager.Instance.SetTimer(60, 
            () =>
            {
                OnFaze1End?.Invoke();
                ScenarioManager.Instance.OnStoryEnd += () => { StartCoroutine("SpawnFaze2"); };
                GameManager.Instance.Player.GetComponent<PlayerController>().SetHp(3);
                StopSpawn();

                //UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-3Stage"); });
                //StopSpawn();
            }
            , "굴러오는 통나무들을 점프로 피하세요!"
        );
        //var player = GameManager.Instance.Player;
        logSpeed = 21f;
        while (Time.time < endTime)
        {

            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                log = PoolManager.Instance.enemyPool.GetPoolObject();
                logs.Add(log);
                log.transform.rotation = Quaternion.Euler(0, 0, -90);
                log.transform.position = spawnPosition;
                if (log.TryGetComponent<LogMove>(out var logComponent))
                    logComponent.Init(logSpeed);

                yield return YieldInstructionCache.WaitForSeconds(0.02f);
            }

            //logSpeed = logSpeed < 35f ? logSpeed : 35f;
            //if (logSpeed > 25f)
            //    after *= Random.Range(0.5f, 0.7f);
            //else
            //    after *= Random.Range(0.7f, 0.9f);



            Debug.Log(logSpeed);
            //Debug.Log(after + "초 뒤");
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 1.5f));
        }

        //Debug.Log("소환 끝");
    }

    private IEnumerator SpawnFaze2()
    {
        endTime = Time.time + 60;
        GameManager.Instance.SetTimer(60,
            () =>
            {
                OnFaze2End?.Invoke();
                ScenarioManager.Instance.GetPicture("Picture1-12");
                ScenarioManager.Instance.CheckPicture();

                ScenarioManager.Instance.OnStoryEnd += () =>
                {
                    UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-3Stage"); });
                };
                PlayerPrefs.SetInt("1-3", 1);
                PlayerPrefs.Save();
                StopSpawn();
            }
            , "쏟아지는 통나무들을 점프로 피하세요!"
        );
        //var player = GameManager.Instance.Player;
        logSpeed = 35f;
        while (Time.time < endTime - 5f)
        {

            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                log = PoolManager.Instance.enemyPool.GetPoolObject();
                logs.Add(log);
                log.transform.rotation = Quaternion.Euler(0, 0, -90);
                log.transform.position = spawnPosition;
                if (log.TryGetComponent<LogMove>(out var logComponent))
                    logComponent.Init(logSpeed);

                yield return YieldInstructionCache.WaitForSeconds(0.02f);
            }

            //logSpeed = logSpeed < 35f ? logSpeed : 35f;
            //if (logSpeed > 25f)
            //    after *= Random.Range(0.5f, 0.7f);
            //else
            //    after *= Random.Range(0.7f, 0.9f);



            Debug.Log(logSpeed);
            //Debug.Log(after + "초 뒤");
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 1f));
        }
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Log"), LayerMask.NameToLayer("Log"), false);


        for (int i = 0; i < 100; i++)
        {
            log = PoolManager.Instance.enemyPool.GetPoolObject();
            logs.Add(log);
            log.transform.rotation = Quaternion.Euler(0, 0, -90);
            log.transform.position = spawnPosition;
            if (log.TryGetComponent<LogMove>(out var logComponent))
                logComponent.Init(logSpeed);
        }
        //Debug.Log("소환 끝");
    }

}
