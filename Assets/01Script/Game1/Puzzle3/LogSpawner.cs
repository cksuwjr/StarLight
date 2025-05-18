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
        );
        //var player = GameManager.Instance.Player;
        while (Time.time < endTime)
        {
            log = PoolManager.Instance.enemyPool.GetPoolObject();
            logs.Add(log);
            log.transform.rotation = Quaternion.Euler(0, 0, -90);
            log.transform.position = spawnPosition;
            if (log.TryGetComponent<LogMove>(out var logComponent))
                logComponent.Init(logSpeed);

            logSpeed = 21f;
            
            //logSpeed = logSpeed < 35f ? logSpeed : 35f;
            //if (logSpeed > 25f)
            //    after *= Random.Range(0.5f, 0.7f);
            //else
            //    after *= Random.Range(0.7f, 0.9f);



            Debug.Log(logSpeed);
            //Debug.Log(after + "檬 第");
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 1.5f));
        }

        //Debug.Log("家券 场");
    }

    private IEnumerator SpawnFaze2()
    {
        endTime = Time.time + 60;
        GameManager.Instance.SetTimer(60,
            () =>
            {
                OnFaze2End?.Invoke();
                ScenarioManager.Instance.OnStoryEnd += () =>
                {
                    UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-3Stage"); });
                };

                StopSpawn();
            }
        );
        //var player = GameManager.Instance.Player;
        while (Time.time < endTime - 3)
        {
            log = PoolManager.Instance.enemyPool.GetPoolObject();
            logs.Add(log);
            log.transform.rotation = Quaternion.Euler(0, 0, -90);
            log.transform.position = spawnPosition;
            if (log.TryGetComponent<LogMove>(out var logComponent))
                logComponent.Init(logSpeed);

            logSpeed += Random.Range(1, 4f);

            var after = 3.5f - logSpeed * 0.05f;

            after = after < 1f ? 1f : after;
            //if (endTime - Time.time > 30f)

            logSpeed = logSpeed < 50f ? logSpeed : 50f;
            if (logSpeed > 25f)
                after *= Random.Range(0.5f, 0.7f);
            else
                after *= Random.Range(0.7f, 0.9f);


            //Debug.Log(logSpeed);
            //Debug.Log(after + "檬 第");
            yield return YieldInstructionCache.WaitForSeconds(after);
        }

        for (int i = 0; i < 100; i++)
        {
            log = PoolManager.Instance.enemyPool.GetPoolObject();
            logs.Add(log);
            log.transform.rotation = Quaternion.Euler(0, 0, -90);
            log.transform.position = spawnPosition;
            if (log.TryGetComponent<LogMove>(out var logComponent))
                logComponent.Init(logSpeed);
        }
        //Debug.Log("家券 场");
    }

}
