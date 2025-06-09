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
                tmp.text = "<< ������ ���� ���� >>";
            }

            if (transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out var tmp2))
            {
                tmp2.text = "�ν��� ������ �׳��� ����ģ������ �����ϰ�\n�ν��� ������ �����ּ���.";
            }
        }

        OnClick += UIManager.Instance.CloseMission;
    }
}
