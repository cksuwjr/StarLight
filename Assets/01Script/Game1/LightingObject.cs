using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingObject : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveTime = 1.5f;
    private float moveSpeed = 10f;

    private void Start()
    {
        StartCoroutine("Lighting");
    }

    private IEnumerator Lighting()
    {
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        var x = Random.Range(-13f, 13f);
        var z = Random.Range(-13f, 13f);

        var nowPosition = targetPosition;
        targetPosition = new Vector3(x, 1.5f, z);

        var distance = Vector3.Distance(nowPosition, targetPosition);

        //StartCoroutine("Chasing");

        var timer = 0f;
        while (timer < moveTime)
        {
            timer += (Time.deltaTime / distance) * moveSpeed;
            transform.LookAt(Vector3.Lerp(nowPosition, targetPosition, timer / moveTime));

            yield return null;
        }

        StartCoroutine("Lighting");
    }

    //private IEnumerator Chasing()
    //{
    //    var player = GameManager.Instance.Player;

    //    var nowPosition = player.transform.position;
    //    var timer = 0f;

    //    var distance = Vector3.Distance(nowPosition, targetPosition);
    //    yield return YieldInstructionCache.WaitForSeconds(0.5f);

    //    while (timer < moveTime)
    //    {
    //        timer += (Time.deltaTime / distance) * moveSpeed;
    //        player.transform.position = Vector3.Lerp(nowPosition, targetPosition, timer / moveTime);

    //        yield return null;
    //    }
    //}
}
