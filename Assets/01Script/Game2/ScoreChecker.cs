using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChecker : MonoBehaviour
{
    private List<PianoNode> nodes = new List<PianoNode>();

    private int score;

    private IInteract key1;
    private IInteract key2;
    private IInteract key3;
    private IInteract key4;

    private void Awake()
    {
        GameObject.Find("Key_White1").TryGetComponent<IInteract>(out key1);
        GameObject.Find("Key_White2").TryGetComponent<IInteract>(out key2);
        GameObject.Find("Key_White3").TryGetComponent<IInteract>(out key3);
        GameObject.Find("Key_White4").TryGetComponent<IInteract>(out key4);
    }

    public void CheckNode(int n)
    {
        PianoNode checkNode = null;
        for(int i = nodes.Count-1; i >= 0; i--)
        {
            if (nodes[i].id == n)
            {
                score += 1;
                checkNode = nodes[i];
                break;
                
            }
        }

        if (checkNode)
        {
            nodes.Remove(checkNode);
            checkNode.ReturnToPool();
            string text = "";
            if (checkNode.transform.position.y > transform.position.y + 3)
                text = "<color=green>Good</color>";
            else if (checkNode.transform.position.y < transform.position.y - 3)
            {
                text = "<color=red>Bad</color>";
                score = 0;
            }
            else
                text = "<color=blue>Perfect</color>";



            if (score >= 10)
            {
                text += "\n<size=30>COMBO</size>";
                text += $"\n{score : 000}";
            }
            UIManager.Instance.OpenPianoScorePanel(text);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Alpha1))
            key1.Interaction();
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Alpha2))
            key2.Interaction();
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Alpha3))
            key3.Interaction();
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Alpha4))
            key4.Interaction();
    }

    private void OnTriggerEnter(Collider other)
    {
        var node = other.GetComponent<PianoNode>();
        if (node) nodes.Add(node);
    }

    private void OnTriggerExit(Collider other)
    {
        var node = other.GetComponent<PianoNode>();
        if (node)
        {
            nodes.Remove(node);
            UIManager.Instance.OpenPianoScorePanel("<color=black>Worst !!</color>");
            score = 0;

            if (GameManager.Instance.Player.TryGetComponent<PlayerController>(out var da))
                da.GetDamage(1);
        }
    }
}
