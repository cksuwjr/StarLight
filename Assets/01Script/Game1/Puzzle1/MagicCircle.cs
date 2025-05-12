using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    public void Create()
    {
        gameObject.SetActive(true);
        var pos = GameManager.Instance.Player.transform.position;
        pos.y = 1;
        transform.position = pos;
        StartCoroutine("Bigger");
    }

    private IEnumerator Bigger()
    {
        float timer = 0f;
        Vector3 scale;
        while (timer < 2f)
        {
            scale = Vector3.Lerp(Vector3.zero, Vector3.one * 100, timer / 2);
            scale.y = 0.01f;
            transform.localScale = scale;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}
