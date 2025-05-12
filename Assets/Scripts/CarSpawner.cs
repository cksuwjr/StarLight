using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("자동차 프리팹")]
    public GameObject carPrefab;

    [Header("생성 간격 (초)")]
    public float spawnInterval = 2f;

    [Header("좌우 랜덤 범위")]
    public float xRange = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        Vector3 pos = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
        Instantiate(carPrefab, pos, Quaternion.identity);
    }
}
