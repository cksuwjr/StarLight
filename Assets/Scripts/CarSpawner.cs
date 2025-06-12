using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        cars.Clear();
    }

    private IEnumerator Spawn()
    {
        //UIManager.Instance.FloatImage(Resources.Load<Sprite>("Image/2-1_tut"));
        UIManager.Instance.FloatImage(Addressables.LoadAssetAsync<Sprite>("2-1").WaitForCompletion());

        yield return YieldInstructionCache.WaitForSeconds(3f);


        UIManager.Instance.SetGoal("쏟아지는 차량을 피해 앞으로 전진하세요!");
        if(GameObject.Find("ClearZone").TryGetComponent<ClearZone>(out var clearZone))
        {
            clearZone.ClearEvent();
            clearZone.OnEnter += () =>
            {
                OnFaze1End?.Invoke();
                PlayerPrefs.SetInt("2-1", 1);
                PlayerPrefs.Save();
                ScenarioManager.Instance.OnStoryEnd += () => { StartCoroutine("SpawnFaze2"); };
                GameManager.Instance.Player.GetComponent<PlayerController>().SetHp(3);
                StopSpawn();
            };
        }

        while (true)
        {
            int carNum = 0;
            for (int i = 0; i < 2; i++)
            {
                spawnPosition = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);

                carNum = Random.Range(0, 4);
                switch (carNum)
                {
                    case 0:
                        car = PoolManager.Instance.enemyPool.GetPoolObject();
                        break;
                    case 1:
                        car = PoolManager.Instance.enemyPool4.GetPoolObject();
                        break;
                    case 2:
                        car = PoolManager.Instance.enemyPool5.GetPoolObject();
                        break;
                    case 3:
                        car = PoolManager.Instance.enemyPool6.GetPoolObject();
                        break;
                    default:
                        car = PoolManager.Instance.enemyPool.GetPoolObject();
                        break;
                }

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
        UIManager.Instance.SetGoal("쉴틈없이 쏟아지는 차량을 피해 앞으로 전진하세요!");
        if (GameObject.Find("ClearZone").TryGetComponent<ClearZone>(out var clearZone))
        {
            clearZone.ClearEvent();
            clearZone.OnEnter += () =>
            {
                OnFaze2End?.Invoke();
                ScenarioManager.Instance.GetPicture("Picture2-10");
                ScenarioManager.Instance.CheckPicture();

                ScenarioManager.Instance.OnStoryEnd += () =>
                {
                    UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("2-1Stage"); });
                };
                PlayerPrefs.Save();
                StopSpawn();
            };
        }

        while (true)
        {
            int carNum;
            for (int i = 0; i < 3; i++)
            {

                spawnPosition = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
                car = PoolManager.Instance.enemyPool6.GetPoolObject();
                cars.Add(car);
                car.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                car.transform.position = spawnPosition;

            }
            yield return YieldInstructionCache.WaitForSeconds(spawnInterval);

            for (int i = 0; i < 2; i++)
            {

                spawnPosition = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
                carNum = Random.Range(0, 3);
                switch (carNum)
                {
                    case 0:
                        car = PoolManager.Instance.enemyPool2.GetPoolObject();
                        break;
                    case 1:
                        car = PoolManager.Instance.enemyPool3.GetPoolObject();
                        break;
                    case 2:
                        car = PoolManager.Instance.enemyPool7.GetPoolObject();
                        break;
                    default:
                        car = PoolManager.Instance.enemyPool2.GetPoolObject();
                        break;
                }
                cars.Add(car);
                car.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                car.transform.position = spawnPosition;

            }
            yield return YieldInstructionCache.WaitForSeconds(spawnInterval);
            
            for (int i = 0; i < 8; i++)
            {

                spawnPosition = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
                car = PoolManager.Instance.enemyPool6.GetPoolObject();
                cars.Add(car);
                car.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                car.transform.position = spawnPosition;

            }
            yield return YieldInstructionCache.WaitForSeconds(spawnInterval);

        }
    }
}
