using System;
using TMPro;
using UnityEngine;

public class Letter : UIClicker
{
    private void Start()
    {
        if(PlayerPrefs.GetInt("Stage1Clear", 0) == 1)
        {
            if(transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out var tmp))
            {
                tmp.text = "\n�ȳ�~ ����, �νþ�.\n�� ����ϴ� ����� ��ȥ�� �յΰ� �־���,\r\n�ٵ� ��ȥ���� �غ��ϸ鼭 ����� ������ �־��� ����?\r\n�ο�� ���ƿ��� ��û ��ȸ�Ǵ����...\r\n�׷��� ����Ʈ ����� ��� ������ �̺�Ʈ�� �غ��ߴµ�...\r\n���ݵ� �׳��� ������ ����ó�� �����־�,\r\n�Ƹ� �� ����� ����ŭ ����� ���ο�ž�.\r\n��Ź�̾� ����.\r\n���� ���� ������ ���� ������ �׿��� ������ �� ������?\r\n�̾��ϴٰ�... �׸��� ���� ����Ѵٰ� ���̾�...\r\n";
            }
        }

        OnClick += UIManager.Instance.CloseMail;
        OnClick += UIManager.Instance.OpenMission;
    }
}
