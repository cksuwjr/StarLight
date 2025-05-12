using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ButtonType
{
    Slot1,
    //Slot2,
}

public class UIManager : SingletonDestroy<UIManager> 
{
    private Image bloodScreen;
    private GameObject informPannel;
    private TextMeshProUGUI[] informTexts;
    private OutLineDrawer lineDrawer;

    private GameObject mail;

    private GameObject chapter;
    private GameObject chapterContent;

    private GameObject mission;

    private GameObject chat;
    private Image chatterImage;
    private TextMeshProUGUI chatterNameText;
    private TextMeshProUGUI chatText;

    private GameObject status;
    private GameObject hpStatus;
    private TextMeshProUGUI timer;

    private GameObject playMenu;

    private TwoStateButton resumeBtn;
    private TwoStateButton musicBtn;


    private GameObject star;

    private GameObject menuPopup;

    private GameObject popup;
    private TextMeshProUGUI popupTitleText;
    private TextMeshProUGUI popupText;
    private TextMeshProUGUI popupSubText;
    private Button popupOKBtn;
    private Button popupCancelBtn;


    private GameObject clearPopup;
    private TextMeshProUGUI clearPopupTitle;
    private GameObject clearPopupStar;
    private GameObject clearPopupSlot;
    private GameObject puzzleClearPopup;

    private GameObject video;

    private Camera uiCamera;

    public bool touchBlocking = false;

    public static event Action OnPressBtnSlot1;

    private GameObject obj;


    public bool useTouchPad = false;

    public void Init()
    {
        var ui = GameObject.Find("UI");
        var uiCam = GameObject.Find("UI Camera");


        if (!ui) return;

        GameObject.Find("BloodScreen").TryGetComponent<Image>(out bloodScreen);
        informPannel = GameObject.Find("InformPannel");
        informTexts = informPannel.GetComponentsInChildren<TextMeshProUGUI>();
        GameObject.Find("LineDrawer").TryGetComponent<OutLineDrawer>(out lineDrawer);


        uiCam.TryGetComponent<Camera>(out uiCamera);

        int num = 0;
        Button btn;

        mail = ui.transform.GetChild(num++).gameObject;
        if (mail.transform.GetChild(4).TryGetComponent<Button>(out btn))
        {
            btn.onClick.AddListener(CloseMail);
            btn.onClick.AddListener(OpenMission);
        }

        chapter = ui.transform.GetChild(num++).gameObject;
        if (chapter.transform.GetChild(1).TryGetComponent<Button>(out btn))
            btn.onClick.AddListener(CloseChapter);

        chapterContent = chapter.GetComponentInChildren<ContentSizeFitter>().gameObject;
        
        mission = ui.transform.GetChild(num++).gameObject;
        if(mission.transform.GetChild(3).TryGetComponent<Button>(out btn))
            btn.onClick.AddListener(CloseMission);

        chat = ui.transform.GetChild(num++).gameObject;
        chat.transform.GetChild(0).TryGetComponent<Image>(out  chatterImage);
        chat.transform.GetChild(1).TryGetComponent<TextMeshProUGUI>(out chatterNameText);
        chat.transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out chatText);

        if (ScenarioManager.Instance)
        {
            if (chat.transform.GetChild(3).TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(ScenarioManager.Instance.NextPage);
            }

            if (chat.transform.GetChild(4).TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(ScenarioManager.Instance.StopStory);
            }
        }


        status = ui.transform.GetChild(num++).gameObject;
        hpStatus = status.transform.GetChild(0).gameObject;
        status.transform.GetChild(1).GetChild(0).TryGetComponent<TextMeshProUGUI>(out timer);

        playMenu = ui.transform.GetChild(num++).gameObject;
        if(playMenu.transform.GetChild(0).TryGetComponent<Button>(out btn))
            btn.onClick.AddListener(() => OpenMenuPopup());

        if (playMenu.transform.GetChild(1).TryGetComponent<TwoStateButton>(out musicBtn))
        {
            musicBtn.OnClick += () => GameManager.Instance.MusicOnOFF(musicBtn.onT_offF);

            var n = PlayerPrefs.GetInt("Sound", 1);
            if (n == 0)
            {
                musicBtn.onT_offF = false;
                GameManager.Instance.MusicOnOFF(musicBtn.onT_offF);
                Debug.Log("설정변경");
            }
            else
            {

            }
        }


        star = ui.transform.GetChild(num++).gameObject;

        menuPopup = ui.transform.GetChild(num++).gameObject;

        if (menuPopup.transform.GetChild(0).TryGetComponent<Button>(out btn))
        {
            btn.onClick.AddListener(() =>
            {
                OpenPopup("다시 시작", "해당 스테이지를 다시 시작합니다.", "(주의: 모든 진행 상황이 초기화 됩니다.)");
                menuPopup.SetActive(false);
                popupOKBtn.onClick.RemoveAllListeners();
                popupOKBtn.onClick.AddListener(GameManager.Instance.RestartGame);
                popupCancelBtn.onClick.RemoveAllListeners();
                popupCancelBtn.onClick.AddListener(ClosePopup);
            }
            );
        }
        if(menuPopup.transform.GetChild(1).TryGetComponent<Button>(out btn))
            btn.onClick.AddListener(GameManager.Instance.ReturnToLobby);

        if(menuPopup.transform.GetChild(1).TryGetComponent<Button>(out btn))
            btn.onClick.AddListener(GameManager.Instance.GameQuit);



        popup = ui.transform.GetChild(num++).gameObject;
        popup.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out popupTitleText);
        popup.transform.GetChild(1).TryGetComponent<TextMeshProUGUI>(out popupText);
        popup.transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out popupSubText);
        popup.transform.GetChild(3).TryGetComponent<Button>(out popupOKBtn);
        popup.transform.GetChild(4).TryGetComponent<Button>(out popupCancelBtn);

        clearPopup = ui.transform.GetChild(num++).gameObject;

        clearPopup.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out clearPopupTitle);
        clearPopupStar = clearPopup.transform.GetChild(1).gameObject;
        clearPopupSlot = clearPopup.transform.GetChild(2).gameObject;

        clearPopupSlot.transform.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(GameManager.Instance.ReturnToLobby);
        var btns = clearPopupSlot.transform.GetChild(1).GetComponentsInChildren<Button>();

        btns[0].onClick.AddListener(GameManager.Instance.RestartGame);
        btns[1].onClick.AddListener(GameManager.Instance.ReturnToLobby);


        puzzleClearPopup = ui.transform.GetChild(num++).gameObject;

        puzzleClearPopup.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(GameManager.Instance.RestartGame);



        video = ui.transform.GetChild(num++).gameObject;

        SetStar(0);



        if (useTouchPad)
        {
            obj = GameObject.Find("BtnSlot1");
            if (obj.TryGetComponent<Button>(out btn))
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.Slot1));
        }
        //obj = GameObject.Find("BtnSlot2");
        //obj.TryGetComponent<Button>(out btn);
        //btn.onClick.AddListener(() => HandleButtonClick(ButtonType.Slot2));

    }


    private void HandleButtonClick(ButtonType type)
    {
        switch(type)
        {
            case ButtonType.Slot1:
                OnPressBtnSlot1?.Invoke();
                break;
            //case ButtonType.Slot2:
            //    OnPressBtnSlot2?.Invoke();
            //    break;
        }
    }

    public void OpenInformText(string title, string content)
    {
        if (informTexts.Length > 1)
        {
            informTexts[0].text = title;
            informTexts[1].text = content;
        }
        LeanTween.scale(informPannel, Vector3.one, 0.3f);
        Invoke("OpenLine", 0.1f);
    }

    private void OpenLine()
    {
        lineDrawer.enabled = true;
    }


    public void LocateInformText(Vector3 pos, Vector3 offset)
    {
        informPannel.transform.parent.localPosition = pos - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        informPannel.transform.localPosition = offset * 100;
    }

    public void CloseInformText()
    {
        LeanTween.scale(informPannel, Vector3.zero, 0.2f);
        lineDrawer.enabled = false;
        CancelInvoke("OpenLine");
    }

    public void OpenMail()
    {
        TouchBlock(true);

        mail.transform.localScale = Vector3.zero;
        LeanTween.moveLocal(mail, new Vector3(394f, 173f, 0), 0f);


        LeanTween.moveLocal(mail, new Vector3(0, 0, 0), 0.3f);
        LeanTween.scale(mail, new Vector3(1, 1, 1), 0.3f);

        GameManager.Instance.Blur(true);
        mail.SetActive(true);
        uiCamera.enabled = true;
    }

    public void CloseMail()
    {
        TouchBlock(false);

        LeanTween.moveLocal(mail, new Vector3(394f, 173f, 0), 0.3f);
        LeanTween.scale(mail, new Vector3(0, 0, 0), 0.3f);
        //mail.SetActive(false);

        //GameManager.Instance.Blur(false);
        //uiCamera.enabled = false;

        //OpenMission();
    }

    public void OpenChapter()
    {
        TouchBlock(true);

        chapter.GetComponentInChildren<ScrollRect>().horizontal = chapterContent.transform.childCount > 3;

        GameManager.Instance.Blur(true);
        chapter.SetActive(true);
        uiCamera.enabled = true;
    }

    public void CloseChapter()
    {
        TouchBlock(false);

        GameManager.Instance.Blur(false);
        chapter.SetActive(false);
        uiCamera.enabled = false;
    }

    public void OpenMission()
    {
        TouchBlock(true);

        GameManager.Instance.Blur(true);
        mission.SetActive(true);

        LeanTween.scale(mission, Vector3.zero, 0);
        LeanTween.scale(mission, Vector3.one, 0.2f);
        //LeanTween.move(mission, Vector3.one, 1);
        uiCamera.enabled = true;
    }

    public void CloseMission()
    {
        TouchBlock(false);

        GameManager.Instance.Blur(false);
        LeanTween.scale(mission, Vector3.zero, 0.2f);

        //mission.SetActive(false);
        Invoke("UICameraShutDown", 0.2f);
    }

    public void OpenScenarioPannel()
    {
        TouchBlock(true);

        chat.SetActive(true);
    }

    public void CloseScenarioPannel()
    {
        TouchBlock(false);

        chat.SetActive(false);
    }

    public void SetScenarioPannel(Sprite sprite, string name, string chat)
    {
        if (sprite != null) 
        {
            chatterImage.sprite = sprite;

            chatterImage.transform.localScale = new Vector3(sprite.bounds.size.x * 0.3f, sprite.bounds.size.y * 0.3f, 0);
            chatterImage.enabled = true;
        }
        else
            chatterImage.enabled = false;

        chatterNameText.text = name;
        chatText.text = chat;
    }

    public void SetHpUI(int n)
    {
        int i;
        for(i = 0; i < n; i++)
            hpStatus.transform.GetChild(i).gameObject.SetActive(true);

        for(; i < hpStatus.transform.childCount; i++)
            hpStatus.transform.GetChild(i).gameObject.SetActive(false);



        Image image;
        Color color;
        for (i = 0; i < n; i++)
        {
            clearPopupStar.transform.GetChild(i).TryGetComponent<Image>(out image);
            color = image.color;
            color.a = 1;
            image.color = color;
        }

        for (i = n; i < clearPopupStar.transform.childCount; i++)
        {
            clearPopupStar.transform.GetChild(i).TryGetComponent<Image>(out image);
            color = image.color;
            color.a = 33f / 255f;
            image.color = color;
        }
    }

    public void SetTimer(float time)
    {
        status.SetActive(true);
        status.transform.GetChild(1).gameObject.SetActive(true);

        timer.text = "";

        if (time < 10f)
            timer.text += "<color=red>";

        timer.text += $"{(int)time / 60 :0}:{time % 60 :00}";

        if (time < 10f)
            timer.text += "</color>";


        if (time < 1) status.SetActive(false);
    }

    public void SetStar(int count)
    {
        int n;
        Image image;
        Color color;
        for(n = 0; n < count; n++)
        {
            star.transform.GetChild(n).TryGetComponent<Image>(out image);
            color = image.color;
            color.a = 1;
            image.color = color;
        }

        for(n = count; n < star.transform.childCount; n++)
        {
            star.transform.GetChild(n).TryGetComponent<Image>(out image);
            color = image.color;
            color.a = 33f/255f;
            image.color = color;
        }
    }


    private void OpenMenuPopup()
    {

        menuPopup.SetActive(!menuPopup.activeSelf);
        if(menuPopup.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    private void OpenPopup(string titleText, string text, string subText)
    {
        popupTitleText.text = titleText;
        popupText.text = text;
        popupSubText.text = subText;

        popup.SetActive(true);
    }

    private void ClosePopup()
    {
        popup.SetActive(false);
    }

    public void OpenClearPopup(bool tf)
    {
        if (tf)
        {
            clearPopupTitle.text = "VICTORY";
            clearPopupSlot.transform.GetChild(0).gameObject.SetActive(true);
            clearPopupSlot.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            clearPopupTitle.text = "FAIL";
            clearPopupSlot.transform.GetChild(0).gameObject.SetActive(false);
            clearPopupSlot.transform.GetChild(1).gameObject.SetActive(true);
        }


        clearPopup.SetActive(true);
    }

    public void OpenClearPuzzlePopup(bool tf, Action returnTo)
    {
        if (tf)
        {
            puzzleClearPopup.SetActive(true);
            puzzleClearPopup.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            puzzleClearPopup.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => returnTo());
        }
        else
        {
            puzzleClearPopup.SetActive(false);
        }
    }

    public void PlayVideo(float width, float height)
    {
        video.SetActive(true);
        //video.transform.localScale = new Vector3(width / 1000f, height / 1000f, 0);

        //var vid = GameObject.Find("Video Player");

        //if(vid.TryGetComponent<VideoPlay>(out var videoPlay))
        //{
        //    videoPlay.OnVideoEnd += () => { OpenClearPopup(true); };
        //    videoPlay.Play();
        //}
    }

    public void StopVideo()
    {
        video.SetActive(false);
    }


    private void TouchBlock(bool tf)
    {
        touchBlocking = tf;
    }

    private void UICameraShutDown()
    {
        uiCamera.enabled = false;
    }

    public void BloodScreen(float value)
    {
        var color = bloodScreen.color;
        color.a = value;
        bloodScreen.color = color;
    }
}
