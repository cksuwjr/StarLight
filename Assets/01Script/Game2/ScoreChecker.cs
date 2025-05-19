using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChecker : MonoBehaviour
{
    private List<PianoNode> nodes = new List<PianoNode>();

    private int score;


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
