using UnityEngine;

public class Piano : MonoBehaviour
{
    private Vector3 pos;

    void Awake()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < pos.y)
            transform.position += Vector3.up * 20f * Time.deltaTime;
        else
            transform.position = pos;
    }

    public void Press()
    {
        transform.position = pos + (Vector3.down * 5f);
    }
}
