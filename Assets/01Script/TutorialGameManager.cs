using System.Collections;
using UnityEngine;

public class TutorialGameManager : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine("Spawn");
    }

    private void StopSpawn()
    {
        StopCoroutine("Spawn");
    }


    private IEnumerator Spawn()
    {
        GameManager.Instance.SetTimer(61,
            () =>
            {
                PlayerPrefs.SetInt("Tuto", 1);
                PlayerPrefs.Save();
                GameManager.Instance.LoadScene("Tutorial");
            }
            , "60�� �ȿ� ���� ���� ���� ��ȹ�ϼ���!"
        );
        yield return null;
    }

}
