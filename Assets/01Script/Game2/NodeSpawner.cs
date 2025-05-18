using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private int[,] nodes = new int[,]
    {
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,5,0},
        { 0,0,5,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},

    };
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
    }

    private IEnumerator Spawn()
    {
        Debug.Log(nodes.Length);
        int count = 0;
        GameObject pianoNode;

        while(count < nodes.Length / 4)
        {

            if (nodes[count, 0] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool.GetPoolObject();
                pianoNode.transform.position = new Vector3(-17.9f, 77f, 16.8f);
                pianoNode.GetComponent<PianoNode>().Init(1);
            }
            if (nodes[count, 1] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool2.GetPoolObject();
                pianoNode.transform.position = new Vector3(-5.7f, 77f, 16.8f);
                pianoNode.GetComponent<PianoNode>().Init(1);
            }
            if (nodes[count, 2] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool3.GetPoolObject();
                pianoNode.transform.position = new Vector3(5.9f, 77f, 16.8f);
                pianoNode.GetComponent<PianoNode>().Init(1);
            }
            if (nodes[count, 3] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool4.GetPoolObject();
                pianoNode.transform.position = new Vector3(17.7f, 77f, 16.8f);
                pianoNode.GetComponent<PianoNode>().Init(1);
            }
            Debug.Log("家券");
            yield return YieldInstructionCache.WaitForSeconds(speed);
            count++;
        }
        
        //while ()
        //{
        //    log = PoolManager.Instance.enemyPool.GetPoolObject();
        //    logs.Add(log);
        //    log.transform.rotation = Quaternion.Euler(0, 0, -90);
        //    log.transform.position = spawnPosition;
        //    if (log.TryGetComponent<LogMove>(out var logComponent))
        //        logComponent.Init(logSpeed);

        //    logSpeed = 21f;

        //    //logSpeed = logSpeed < 35f ? logSpeed : 35f;
        //    //if (logSpeed > 25f)
        //    //    after *= Random.Range(0.5f, 0.7f);
        //    //else
        //    //    after *= Random.Range(0.7f, 0.9f);



        //    Debug.Log(logSpeed);
        //    //Debug.Log(after + "檬 第");
        //    yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 1.5f));
        //}
        yield return null;
        //Debug.Log("家券 场");
    }

    //private IEnumerator SpawnFaze2()
    //{
    //    endTime = Time.time + 60;
    //    GameManager.Instance.SetTimer(60,
    //        () =>
    //        {
    //            OnFaze2End?.Invoke();
    //            ScenarioManager.Instance.OnStoryEnd += () =>
    //            {
    //                UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-3Stage"); });
    //            };

    //            StopSpawn();
    //        }
    //    );
    //    //var player = GameManager.Instance.Player;
    //    while (Time.time < endTime - 3)
    //    {
    //        log = PoolManager.Instance.enemyPool.GetPoolObject();
    //        logs.Add(log);
    //        log.transform.rotation = Quaternion.Euler(0, 0, -90);
    //        log.transform.position = spawnPosition;
    //        if (log.TryGetComponent<LogMove>(out var logComponent))
    //            logComponent.Init(logSpeed);

    //        logSpeed += Random.Range(1, 4f);

    //        var after = 3.5f - logSpeed * 0.05f;

    //        after = after < 1f ? 1f : after;
    //        //if (endTime - Time.time > 30f)

    //        logSpeed = logSpeed < 50f ? logSpeed : 50f;
    //        if (logSpeed > 25f)
    //            after *= Random.Range(0.5f, 0.7f);
    //        else
    //            after *= Random.Range(0.7f, 0.9f);


    //        //Debug.Log(logSpeed);
    //        //Debug.Log(after + "檬 第");
    //        yield return YieldInstructionCache.WaitForSeconds(after);
    //    }

    //    for (int i = 0; i < 100; i++)
    //    {
    //        log = PoolManager.Instance.enemyPool.GetPoolObject();
    //        logs.Add(log);
    //        log.transform.rotation = Quaternion.Euler(0, 0, -90);
    //        log.transform.position = spawnPosition;
    //        if (log.TryGetComponent<LogMove>(out var logComponent))
    //            logComponent.Init(logSpeed);
    //    }
    //    //Debug.Log("家券 场");
    //}
}
