using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarSpawner : MonoBehaviour
{
    [Header("자동차 프리팹")]
    public GameObject carPrefab;

    [Header("생성 간격 (초)")]
    public float spawnInterval = 2f;

    [Header("좌우 랜덤 범위")]
    public float xRange = 3f;

    public UnityEvent OnFaze1End;
    public UnityEvent OnFaze2End;

    private List<GameObject> cars = new List<GameObject>();

    private float endTime;
    private Vector3 spawnPosition;
    private GameObject car;

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
        StopCoroutine("SpawnFaze2");
        for (int i = cars.Count - 1; i >= 0; i--)
            cars[i].GetComponent<PoolObject>().ReturnToPool();
    }

    private IEnumerator Spawn()
    {
        if(GameObject.Find("ClearZone").TryGetComponent<ClearZone>(out var clearZone))
        {
            clearZone.ClearEvent();
            clearZone.OnEnter += () =>
            {
                OnFaze1End?.Invoke();
                ScenarioManager.Instance.OnStoryEnd += () => { StartCoroutine("SpawnFaze2"); };
                GameManager.Instance.Player.GetComponent<PlayerController>().SetHp(3);
                StopSpawn();
            };
        }

        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                spawnPosition = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
                car = PoolManager.Instance.enemyPool.GetPoolObject();
                cars.Add(car);
                car.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                car.transform.position = spawnPosition;
            }
            yield return YieldInstructionCache.WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnFaze2()
    {
        GameManager.Instance.Player.transform.position = new Vector3(23f, -1.183f, 110f);

        if (GameObject.Find("ClearZone").TryGetComponent<ClearZone>(out var clearZone))
        {
            clearZone.ClearEvent();
            clearZone.OnEnter += () =>
            {
                OnFaze2End?.Invoke();
                ScenarioManager.Instance.OnStoryEnd += () =>
                {
                    UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("2-1Stage"); });
                };

                StopSpawn();
            };
        }

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {

                spawnPosition = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
                car = PoolManager.Instance.enemyPool.GetPoolObject();
                cars.Add(car);
                car.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                car.transform.position = spawnPosition;

            }
            yield return YieldInstructionCache.WaitForSeconds(spawnInterval);
        }
    }
}
