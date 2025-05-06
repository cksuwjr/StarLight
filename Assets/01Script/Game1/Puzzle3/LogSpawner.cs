using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    private GameObject log;
    [SerializeField] private Vector3 spawnPosition;
    private float endTime;
    private float logSpeed = 2.5f;

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
        for(int i = logs.Count - 1; i >= 0; i--)
            Destroy(logs[i]);
    }

    private IEnumerator Spawn()
    {
        endTime = Time.time + 60;
        GameManager.Instance.SetTimer(60, 
            () =>
            {
                UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("tae3"); });
                StopSpawn();
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

            logSpeed += Random.Range(1, 4f);

            var after = 3.5f - logSpeed * 0.05f;

            after = after < 1f ? 1f : after;
            //if (endTime - Time.time > 30f)

            logSpeed = logSpeed < 35f ? logSpeed : 35f;
            if (logSpeed > 25f)
                after *= Random.Range(0.5f, 0.7f);
            else
                after *= Random.Range(0.7f, 0.9f);


            //Debug.Log(logSpeed);
            //Debug.Log(after + "초 뒤");
            yield return YieldInstructionCache.WaitForSeconds(after);
        }

        //Debug.Log("소환 끝");
    }

}
