using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColorManager : MonoBehaviour
{
    private LineRenderer playerLiner;
    private List<LineRenderer> otherLiner = new List<LineRenderer>();

    private Dictionary<Vector3Int, LineRenderer> lineOwner = new Dictionary<Vector3Int, LineRenderer>();

    private Transform playerTransform;

    private void Awake()
    {
        transform.GetChild(0).TryGetComponent<LineRenderer>(out playerLiner);

        for (int i = 1; i < transform.childCount; i++)
            otherLiner.Add(transform.GetChild(i).GetComponent<LineRenderer>());

        playerLiner.positionCount = 1;
        playerLiner.SetPosition(0, new Vector3(0, 0.6f, 0));
    }

    private void Update()
    {
        if(!playerTransform)
            playerTransform = GameManager.Instance.Player.transform;

        Vector3Int paintPos = new Vector3Int((int)playerTransform.position.x, 0, (int)playerTransform.position.z);

        if (!lineOwner.ContainsKey(paintPos))
        {
            lineOwner.Add(paintPos, playerLiner);
            playerLiner.positionCount++;
            playerLiner.SetPosition(playerLiner.positionCount - 1, paintPos + Vector3.up * 0.6f);
        }
        else
            if (lineOwner[paintPos] == playerLiner) return;


    }
}
