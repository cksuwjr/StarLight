using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class NodeSpawner : MonoBehaviour
{
    //[SerializeField] private float speed = 0.5f;

    private float speed = 0.3749f;

    public UnityEvent OnFaze1End;
    public UnityEvent OnFaze2End;

    [SerializeField] private AudioClip BGM;

    private int[,] nodes = new int[,]
    {
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0}, // 0
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,0,5}, // 1
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5}, // 2
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0}, // 3
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0}, // 4
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,5,0}, // 5
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,5,0},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0}, //6
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,0}, // 7
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0}, // 8
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,5,0}, // 9
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,5,0},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0}, // 10
        { 0,5,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,5,0}, // 11
        { 5,0,5,0},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0},
        { 5,0,5,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,5,0}, // 12
        { 0,5,0,5},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0}, // 13
        { 5,0,5,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,0},
        { 5,0,5,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,0,0}, // 14
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5}, // 15
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,0,0}, // 16
        { 0,0,5,0},
        { 0,0,0,0},
        { 0,0,5,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0},
        { 0,0,0,0}, // 17
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0}, // 18
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5}, // 19
        { 5,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,5},
        { 0,5,0,0}, // 20
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5}, // 21
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5}, // 23
        { 5,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 5,0,0,0}, // 24
        { 0,5,5,0},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,5,5,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0}, // 25
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,0,0,0}, // 26
        { 5,0,0,5}


    };

    private int[,] nodesFaze2 = new int[,]
    {
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0}, // 0
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,5,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5}, // 1
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,0,5}, // 2
        { 5,0,0,5},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5},
        { 0,5,5,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0}, // 3
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5},
        { 0,0,5,0},
        { 0,5,0,0}, // 4
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,5,0}, // 5
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,5,0},
        { 0,0,0,5},
        { 0,5,0,5},
        { 5,0,0,0}, //6
        { 0,5,5,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 5,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,0}, // 7
        { 5,0,5,0},
        { 0,0,5,0},
        { 0,0,5,0},
        { 0,5,5,0},
        { 0,5,0,0},
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0}, // 8
        { 5,0,0,5},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,5,0}, // 9
        { 5,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,5,0},
        { 0,0,5,0},
        { 0,5,0,5},
        { 0,5,0,0}, // 10
        { 0,5,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 5,0,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,5,5,0}, // 11
        { 5,0,0,5},
        { 0,0,0,0},
        { 0,5,5,0},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,5,0}, // 12
        { 0,5,0,5},
        { 0,5,0,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,5},
        { 0,5,0,0},
        { 0,0,0,0}, // 13
        { 5,0,5,0},
        { 5,0,0,0},
        { 0,0,5,0},
        { 0,0,0,0},
        { 5,0,5,0},
        { 0,0,5,0},
        { 5,0,0,0},
        { 0,0,0,0}, // 14
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 5,0,0,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,0,0,5}, // 15
        { 5,0,0,5},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0},
        { 0,5,0,5},
        { 0,0,0,0}, // 16
        { 5,0,5,0},
        { 0,0,0,0},
        { 5,0,5,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,0,0,0},
        { 0,0,5,0}, // 17
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 5,0,5,0},
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0}, // 18
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,5}, // 19
        { 5,0,0,0},
        { 0,5,0,0},
        { 5,0,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,5,0,0}, // 20
        { 5,0,0,5},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5}, // 21
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,5,0,0},
        { 5,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,5}, // 23
        { 5,0,5,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,0,5},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 5,0,5,0}, // 24
        { 0,5,5,0},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,5,5,0},
        { 0,0,0,0},
        { 0,5,0,0},
        { 0,0,5,0},
        { 0,0,5,0}, // 25
        { 0,5,0,5},
        { 0,0,5,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 0,0,0,0},
        { 5,0,0,5},
        { 0,0,0,0}, // 26
        { 5,0,0,5}

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
        SoundManager.Instance.StopBGM();

        UIManager.Instance.FloatImage(Resources.Load<Sprite>("Image/2-2_tut"));

        yield return YieldInstructionCache.WaitForSeconds(3f);


        UIManager.Instance.SetGoal("떨어지는 음표에 맞춰 피아노를 연주하세요!");

        Debug.Log(nodes.Length);
        int count = 0;
        GameObject pianoNode;

        SoundManager.Instance.PlaySound(BGM);

        float timer = 0f;
        while (count < nodes.Length / 4)
        {
            //if (timer >= speed)
            //{

                if (nodes[count, 0] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool.GetPoolObject();
                    pianoNode.transform.position = new Vector3(-22.05f, 77f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(1, 45);
                }
                if (nodes[count, 1] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool2.GetPoolObject();
                    pianoNode.transform.position = new Vector3(-7.35f, 77f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(2, 45);
                }
                if (nodes[count, 2] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool3.GetPoolObject();
                    pianoNode.transform.position = new Vector3(7.35f, 77f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(3, 45);
                }
                if (nodes[count, 3] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool4.GetPoolObject();
                    pianoNode.transform.position = new Vector3(22.05f, 77f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(4, 45);
                }
                timer = 0f;
                count++;
            //}
            //yield return YieldInstructionCache.waitForFixedUpdate;
            //timer += Time.fixedDeltaTime;
            yield return YieldInstructionCache.WaitForSeconds(speed);

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
        //    yield return YieldInstructionCache.WaitForSeconds(Random.Range(0.5f, 1.5f));
        //}
        yield return YieldInstructionCache.WaitForSeconds(3f);

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
        UIManager.Instance.SetGoal("전하지 못한 마지막 연주를 완성하세요!");

        yield return YieldInstructionCache.WaitForSeconds(3f);
        SoundManager.Instance.PlaySound(BGM);

        float timer = 0f;
        while (count < nodesFaze2.Length / 4)
        {
            //if (timer >= speed)
            //{

                if (nodesFaze2[count, 0] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool.GetPoolObject();
                    pianoNode.transform.position = new Vector3(-22.05f, 73f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(1, 45);
                }
                if (nodesFaze2[count, 1] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool2.GetPoolObject();
                    pianoNode.transform.position = new Vector3(-7.35f, 73f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(2, 45);
                }
                if (nodesFaze2[count, 2] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool3.GetPoolObject();
                    pianoNode.transform.position = new Vector3(7.35f, 73f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(3, 45);
                }
                if (nodesFaze2[count, 3] == 5)
                {
                    pianoNode = PoolManager.Instance.enemyPool4.GetPoolObject();
                    pianoNode.transform.position = new Vector3(22.05f, 73f, 20.9f);
                    pianoNode.GetComponent<PianoNode>().Init(4, 45);
                }
                count++;
                timer = 0f;
            //}
            //yield return YieldInstructionCache.waitForFixedUpdate;
            //timer += Time.fixedDeltaTime;
            yield return YieldInstructionCache.WaitForSeconds(speed);
        }
        yield return YieldInstructionCache.WaitForSeconds(3f);

        OnFaze2End?.Invoke();
        ScenarioManager.Instance.GetPicture("Picture2-11");
        ScenarioManager.Instance.CheckPicture();

        ScenarioManager.Instance.OnStoryEnd += () =>
        {
            UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("2-2Stage"); });
        };

        StopSpawn();
    }
}
