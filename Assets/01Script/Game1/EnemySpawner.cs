using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private GameObject enemy;
    private List<Enemy> enemies = new List<Enemy>();

    public UnityEvent OnFaze1End;
    public UnityEvent OnFaze2End;

    private void Start()
    {
        StartSpawn();
    }

    private void StartSpawn()
    {
        StartCoroutine("Spawn");
        StartCoroutine("Upgrade");
    }

    private void StopSpawn()
    {
        StopEnemys();
        StopCoroutine("Spawn");
        StopCoroutine("Upgrade");
    }

    private void StopEnemys()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Stop();
            //Destroy(enemies[i].gameObject);
        }
        enemies.Clear();
    }

    //private IEnumerator Spawn()
    //{
    //    GameManager.Instance.SetTimer(61,
    //        () =>
    //        {
    //            UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-1Stage"); });
    //            StopSpawn();
    //        }
    //    );

    //    for (int i = 0; i < 18; i++)
    //    {
    //        enemy = PoolManager.Instance.enemyPool.GetPoolObject();
    //        enemy.transform.position = GetSpawnPosition2();
    //        if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
    //        {
    //            enemyComponent.Init(this, 1f);
    //            enemies.Add(enemyComponent);
    //        }

    //    }

    //    yield return null;

    //}Z


    private IEnumerator Spawn()
    {
        //UIManager.Instance.FloatImage(Resources.Load<Sprite>("Image/Stell_Virus"));
        UIManager.Instance.FloatImage(Addressables.LoadAssetAsync<Sprite>("1-1").WaitForCompletion());


        yield return YieldInstructionCache.WaitForSeconds(3f);


        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Log"), LayerMask.NameToLayer("Log"), false);

        GameManager.Instance.SetTimer(61,
            () =>
            {
                //UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-1Stage"); });

                OnFaze1End?.Invoke();
                ScenarioManager.Instance.OnStoryEnd += () => { StartCoroutine("SpawnFaze2"); };
                GameManager.Instance.Player.GetComponent<PlayerController>().SetHp(3);
                StopSpawn();
                PlayerPrefs.SetInt("1-1", 1);
                PlayerPrefs.Save();
            }
            , "몰려오는 바이러스들로부터 60초간 살아남으세요!"
        );

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                enemy = PoolManager.Instance.enemyPool.GetPoolObject();
                enemy.transform.position = GetSpawnPosition();
                if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    enemyComponent.Init(this, Random.Range(3f, 6f));
                    enemies.Add(enemyComponent);
                }
            }
            yield return YieldInstructionCache.WaitForSeconds(5f);
        }

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                enemy = PoolManager.Instance.enemyPool.GetPoolObject();
                enemy.transform.position = GetSpawnPosition();
                if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    enemyComponent.Init(this, Random.Range(3f, 7f));
                    enemies.Add(enemyComponent);
                }
            }
            for (int i = 0; i < 1; i++)
            {
                enemy = PoolManager.Instance.enemyPool2.GetPoolObject();
                enemy.transform.position = GetSpawnPosition();
                if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    enemyComponent.Init(this, 9f);
                    enemies.Add(enemyComponent);

                }
            }
            yield return YieldInstructionCache.WaitForSeconds(5f);
        }


        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                enemy = PoolManager.Instance.enemyPool.GetPoolObject();
                enemy.transform.position = GetSpawnPosition();
                if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    enemyComponent.Init(this, Random.Range(3f, 8f));
                    enemies.Add(enemyComponent);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                enemy = PoolManager.Instance.enemyPool2.GetPoolObject();
                enemy.transform.position = GetSpawnPosition();
                if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    enemyComponent.Init(this, 12f);
                    enemies.Add(enemyComponent);
                }
            }
            yield return YieldInstructionCache.WaitForSeconds(5f);
        }
    }

    private IEnumerator SpawnFaze2()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Log"), LayerMask.NameToLayer("Log"), true);

        for (int i = 0; i < 36; i++)
        {
            enemy = PoolManager.Instance.enemyPool.GetPoolObject();
            enemy.transform.position = GetSpawnPosition2();
            var dmgObj = enemy.GetComponentInChildren<DamageObject>();
            dmgObj.damage = 3;
            dmgObj.isAlwaysDamage = true;

            if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.Init(this, 5f);
                enemies.Add(enemyComponent);
                enemyComponent.ChangeState(EnemyState.Chase2);
                enemyComponent.SetRepositionable(false);
            }
        }

        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetSpeed(0.1f);
            enemies[i].SetAnimSpeed(10f);
            enemies[i].ChangeState(EnemyState.Chase2);
        }


        GameManager.Instance.SetTimer(61,
              () =>
              {
                  OnFaze2End?.Invoke();
                  ScenarioManager.Instance.GetPicture("Picture1-10");
                  ScenarioManager.Instance.CheckPicture();

                  ScenarioManager.Instance.OnStoryEnd += () =>
                  {
                      UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-1Stage"); });
                  };

                  StopSpawn();
              }
              , "좁혀오는 바이러스의 포위망으로부터 60초간 살아남으세요!"
          );

        float spawnTerm = 2f;
        for(int i = 0; i < 60f / spawnTerm; i++)
        {
            enemy = PoolManager.Instance.enemyPool2.GetPoolObject();
            enemy.transform.position = GetSpawnPosition();
            if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.Init(this, 4f + i * 0.2f);
                enemies.Add(enemyComponent);
                enemyComponent.ChangeState(EnemyState.Chase2);
            }

            yield return YieldInstructionCache.WaitForSeconds(spawnTerm);
        }


        yield return null;
    }

    private IEnumerator Upgrade()
    {
        int count = 10;
        int n = 0;
        while (n < count)
        {
            yield return YieldInstructionCache.WaitForSeconds(5f);

            n++;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].UpgradeSpeed(0.2f);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        var player = GameManager.Instance.Player;

        var n = Mathf.Pow(-1, Random.Range(0, 2));

        Vector3 spawnPos = player.transform.position;
        spawnPos.y = 1;

        if(n == -1)
        { // horizon
            spawnPos.x += Random.Range(-18f, 18f);
            spawnPos.z += 15f * Mathf.Pow(-1, Random.Range(0, 2));
        }
        if(n == 1)
        { // vertical
            spawnPos.x += 18f * Mathf.Pow(-1, Random.Range(0, 2));
            spawnPos.z += Random.Range(-15f, 15f);
        }
        return spawnPos;
    }


    float radius = 20f;
    float angle = 0;

    private Vector3 GetSpawnPosition2()
    {
        var player = GameManager.Instance.Player;

        Vector3 spawnPos = player.transform.position;
        spawnPos.y = 1;


        angle += 10f;

        //float angle = Random.Range(0f, 360f);
        float radian = angle * Mathf.Deg2Rad;

        // 각도를 기반으로 위치 계산
        Vector3 spawnOffset = new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * radius;
        spawnPos = spawnPos + spawnOffset;


        return spawnPos;
    }

    public void RePosition(Enemy obje)
    {
        enemy = obje.gameObject;
        enemy.transform.position = GetSpawnPosition();

        obje.Reboot();
    }
}
