using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mission : UIClicker
{
    void Start()
    {
        if (PlayerPrefs.GetInt("Stage1Clear", 0) == 1)
        {
            if (transform.GetChild(1).TryGetComponent<TextMeshProUGUI>(out var tmp))
            {
                tmp.text = "<< 전하지 못한 진심 >>";
            }

            if (transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out var tmp2))
            {
                tmp2.text = "로시의 편지를 그녀의 남자친구에게 전달하고\n로시의 성불을 도와주세요.";
            }
        }

        OnClick += UIManager.Instance.CloseMission;
    }
}
