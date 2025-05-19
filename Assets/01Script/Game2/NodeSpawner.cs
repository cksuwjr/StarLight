using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    public UnityEvent OnFaze1End;
    public UnityEvent OnFaze2End;

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
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,5,0,0},
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
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,5,0,0},
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
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},

    };

    private int[,] nodesFaze2 = new int[,]
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
        { 0,5,0,0},
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
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
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
                pianoNode.transform.position = new Vector3(-22.05f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(1, 20);
            }
            if (nodes[count, 1] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool2.GetPoolObject();
                pianoNode.transform.position = new Vector3(-7.35f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(2, 20);
            }
            if (nodes[count, 2] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool3.GetPoolObject();
                pianoNode.transform.position = new Vector3(7.35f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(3, 20);
            }
            if (nodes[count, 3] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool4.GetPoolObject();
                pianoNode.transform.position = new Vector3(22.05f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(4, 20);
            }
            Debug.Log("社発");
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
        //    //Debug.Log(after + "段 及");
        //    yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 1.5f));
        //}
        yield return YieldInstructionCache.WaitForSeconds(3f);
        //Debug.Log("社発 魁");

        OnFaze1End?.Invoke();
        ScenarioManager.Instance.OnStoryEnd += () => { StartCoroutine("SpawnFaze2"); };
        GameManager.Instance.Player.GetComponent<PlayerController>().SetHp(3);

        PlayerPrefs.SetInt("2-2", 1);
        PlayerPrefs.Save();
    }

    private IEnumerator SpawnFaze2()
    {
        int count = 0;
        GameObject pianoNode;

        while (count < nodesFaze2.Length / 4)
        {

            if (nodesFaze2[count, 0] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool.GetPoolObject();
                pianoNode.transform.position = new Vector3(-22.05f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(1, 45);
            }
            if (nodesFaze2[count, 1] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool2.GetPoolObject();
                pianoNode.transform.position = new Vector3(-7.35f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(2, 45);
            }
            if (nodesFaze2[count, 2] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool3.GetPoolObject();
                pianoNode.transform.position = new Vector3(7.35f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(3, 45);
            }
            if (nodesFaze2[count, 3] == 5)
            {
                pianoNode = PoolManager.Instance.enemyPool4.GetPoolObject();
                pianoNode.transform.position = new Vector3(22.05f, 77f, 20.9f);
                pianoNode.GetComponent<PianoNode>().Init(4, 45);
            }
            Debug.Log("社発");
            yield return YieldInstructionCache.WaitForSeconds(speed);
            count++;
        }
        yield return YieldInstructionCache.WaitForSeconds(3f);

        OnFaze2End?.Invoke();
        ScenarioManager.Instance.OnStoryEnd += () =>
        {
            UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("2-2Stage"); });
        };

        StopSpawn();
    }
}
