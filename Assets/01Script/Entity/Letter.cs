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
                tmp.text = "\n안녕~ 스텔, 로시야.\n난 사랑하는 사람과 결혼을 앞두고 있었어,\r\n근데 결혼식을 준비하면서 사소한 다툼이 있었지 뭐야?\r\n싸우고 돌아오니 엄청 후회되더라고...\r\n그래서 데이트 약속을 잡고 나름의 이벤트도 준비했는데...\r\n지금도 그날의 말들이 가시처럼 남아있어,\r\n아마 그 사람도 나만큼 힘들고 괴로울거야.\r\n부탁이야 스텔.\r\n내가 끝내 전하지 못한 진심을 그에게 전해줄 수 있을까?\r\n미안하다고... 그리고 정말 사랑한다고 말이야...\r\n";
            }
        }

        OnClick += UIManager.Instance.CloseMail;
        OnClick += UIManager.Instance.OpenMission;
    }
}
