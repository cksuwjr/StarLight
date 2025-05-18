using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>
{
    public Pool enemyPool;
    public Pool enemyPool2;
    public Pool enemyPool3;
    public Pool enemyPool4;
    public Pool enemyPool5;
    public Pool enemyPool6;
    public Pool enemyPool7;

    public void Init()
    {
        //transform.GetChild(0).TryGetComponent<Pool>(out enemyPool);
    }
}
