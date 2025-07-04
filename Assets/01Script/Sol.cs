using UnityEngine;

public class Sol : MonoBehaviour
{
    private Vector3 dir;
    private GameObject player;

    public void Reposition()
    {
        player = GameManager.Instance.Player;

    }

    private void Update()
    {
        if (player == null) return;

        dir = transform.position - player.transform.position;
        dir.y = 0;
        dir.Normalize();
        transform.forward = dir;

        if (Vector3.Distance(player.transform.position, transform.position) < 10f)
            transform.position += dir * Time.deltaTime * 4.5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.StopTimer();
        PlayerPrefs.SetInt("Tuto", 1);
        PlayerPrefs.Save();
        GameManager.Instance.LoadScene("Tutorial");
    }
}
