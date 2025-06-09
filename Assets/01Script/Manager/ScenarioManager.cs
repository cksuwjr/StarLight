using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class ScenarioManager : SingletonDestroy<ScenarioManager>
{
    private Dictionary<int, ChatData> story;
    private int page = 0;

    public Action OnStoryEnd;

    private int star = 0;


    private bool texting = false;
    private string chapter;

    public UnityEvent OnStarAllCollected;

    private TMP_FontAsset fontRegular;
    private TMP_FontAsset fontBold;

    public UnityEvent OnStart;

    protected override void DoAwake()
    {
         fontBold = Resources.Load<TMP_FontAsset>("Font/PyeongChang-Bold SDF");
         fontRegular = Resources.Load<TMP_FontAsset>("Font/PyeongChang-Regular SDF");
    }

    private void Start()
    {
        OnStart?.Invoke();
    }

    public void SetChapter(string name)
    {
        var type = StoryType.Lobby;
        chapter = name;
        switch (name)
        {
            case "Lobby": type = StoryType.Lobby; break;
            case "Tutorial": type = StoryType.Tutorial; break;
            case "Ch_1": type = StoryType.Ch_1; break;
            case "Ch_2": type = StoryType.Ch_2; break;
            case "Loading": type = StoryType.Loading; break;
            default:
                break;
        }
        SetChapter(type);
    }

    public void SetChapter(StoryType name)
    {
        story = DataManager.Instance.GetStoryData(name);
    }

    public void StartStory(int num)
    {
        page = num;

        LoadPage();
        if(story[page].windowType == "large")
            UIManager.Instance.OpenScenarioPannel();
        else
            UIManager.Instance.OpenSmallScenarioPannel(new Vector2(story[page].x, story[page].y));

        if (chapter == "Lobby")
        {
            switch (num)
            {
                case 1: OnStoryEnd += () => { GameManager.Instance.LoadScene("Tutorial"); }; break;
            }
        }

        if (chapter == "Tutorial")
        {
            switch (num)
            {
                case 12: OnStoryEnd += () => { GameManager.Instance.SavePosition();}; break;
            }
        }





        if (chapter == "Ch_1")
        {
            switch (num)
            {
                case 3: OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-1Puzzle"); }; break;
                case 13: OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-2Puzzle"); }; break;
                case 23: OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-3Puzzle"); }; break;
                case 29:
                    OnStoryEnd += () =>
                    {
                        PlayerPrefs.SetInt("Stage1Clear", 1);
                        PlayerPrefs.Save();
                        var videoPlay = GameObject.Find("Video Player").GetComponent<VideoPlay>();
                        videoPlay.Play();
                        videoPlay.OnVideoEnd += () => { UIManager.Instance.OpenClearPopup(true, null); };
                    };
                    break;
                default:
                    break;
            }
        }

        if (chapter == "Ch_2")
        {
            switch (num)
            {
                case 3: OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("2-1Puzzle"); }; break;
                case 13: OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("2-2Puzzle"); }; break;
                case 23: OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("2-3Puzzle"); }; break;


                case 25: OnStoryEnd += () => 
                {
                    GameManager.Instance.Player.transform.position = new Vector3(13.27f, 282.19f, 2.10f);
                    PlayerPrefs.SetInt("2-3", 1);
                    PlayerPrefs.Save();
                }; break;

                case 27:
                    OnStoryEnd += () =>
                    {
                        UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("2-3Stage"); });
                    }; break;

                case 29:
                    OnStoryEnd += () =>
                    {
                        PlayerPrefs.SetInt("Stage2Clear", 1);
                        PlayerPrefs.Save();
                        var videoPlay = GameObject.Find("Video Player").GetComponent<VideoPlay>();
                        videoPlay.Play();
                        videoPlay.OnVideoEnd += () => { UIManager.Instance.OpenClearPopup(true, null); };
                    };
                    break;
                default:
                    break;
            }
        }

    }

    public void StopStory()
    {
        UIManager.Instance.CloseScenarioPannel();
        UIManager.Instance.CloseSmallScenarioPannel();

        SoundManager.Instance.StopSound();

        OnStoryEnd?.Invoke();
        OnStoryEnd = null;

        story = null;
        page = 0;
    }

    public void RemoveStory()
    {
        SoundManager.Instance.StopSound();
        UIManager.Instance.CloseScenarioPannel();
        UIManager.Instance.CloseSmallScenarioPannel();
    }

    public void HideStopBtn(bool tf)
    {
        UIManager.Instance.HideSeekScenarioCloseBtn(tf);
    }

    public void NextPage()
    {
        if (texting)
        {
            texting = false;
            return;
        }

        if (story == null) return;

        Debug.Log(story.Count + "<" + (page + 1));
        if (story.Count < page + 1)
        {
            StopStory();
            return;
        }

        if (story[page].category == story[page + 1].category && story[page].target == story[page + 1].target)
        {
            page++;
            LoadPage();
        }
        else
            StopStory();
    }


    private void LoadPage()
    {
        var imgSprite = Resources.Load<Sprite>(story[page].imgSrc);
        var sound = Resources.Load<AudioClip>(story[page].ttsSrc);
        var size = story[page].windowType;
        var font = story[page].font;
        SoundManager.Instance.StopSound();
        if (sound) 
            SoundManager.Instance.PlaySound(sound, 0.7f, false);

        if (font == "Bold")
            UIManager.Instance.SetFont(fontBold);
        else if (font == "Regular")
            UIManager.Instance.SetFont(fontRegular);


        if (size == "large")
            StartCoroutine(LoadText(imgSprite, story[page].name, story[page].chat));
        else
            StartCoroutine(LoadTextSmall(imgSprite, story[page].name, story[page].chat));

        //UIManager.Instance.SetScenarioPannel(imgSprite, story[page].name, story[page].chat);
    }

    private IEnumerator LoadText(Sprite sprite, string name, string chat)
    {
        UIManager.Instance.CloseSmallScenarioPannel();
        UIManager.Instance.OpenScenarioPannel();

        texting = true;
        string chatText = "";

        int i = 0;
        while (i < chat.Length && texting) {
            chatText += chat[i];
            UIManager.Instance.SetScenarioPannel(sprite, name, chatText);
            yield return YieldInstructionCache.WaitForSeconds(0.03f);
            i++;
        }

        UIManager.Instance.SetScenarioPannel(sprite, name, chat);
        texting = false;
    }

    private IEnumerator LoadTextSmall(Sprite sprite, string name, string chat)
    {
        UIManager.Instance.CloseScenarioPannel();
        UIManager.Instance.OpenSmallScenarioPannel(new Vector2(story[page].x, story[page].y));

        texting = true;
        string chatText = "";

        int i = 0;
        while (i < chat.Length && texting)
        {
            chatText += chat[i];
            UIManager.Instance.SetSmallScenarioPanel(sprite, name, chatText);
            yield return YieldInstructionCache.WaitForSeconds(0.03f);
            i++;
        }

        UIManager.Instance.SetSmallScenarioPanel(sprite, name, chat);
        texting = false;
    }


    public void AddStarWithStory()
    {
        OnStoryEnd += AddStarFunc;
    }

    public void AddStarFunc()
    {
        star++;
        UIManager.Instance.AddStar(star);

        if (star > 2)
            OnStarAllCollected?.Invoke();
    }

    public void AddStar()
    {
        star++;
        UIManager.Instance.SetStar(star);
        OnStoryEnd -= AddStar;

        if(star > 2)
            OnStarAllCollected?.Invoke();
    }

    public void SetActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void SetActiveFalse(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void GetPicture(string name)
    {
        PlayerPrefs.SetInt(name, 1);
        PlayerPrefs.Save();

        CheckPicture();
        UIManager.Instance.AcquirePictureEffect();
    }

    public void CheckPicture()
    {
        int count = 0;
        string name;

        if (chapter == "Tutorial")
        {
            if (PlayerPrefs.GetInt("DutorialPicture", 0) == 1) count++;

            UIManager.Instance.SetPicture(count, 1);
        }

        if (chapter == "Ch_1")
        {
            name = "Picture1";

            for (int i = 1; i <= 12; i++)
                if (PlayerPrefs.GetInt(name + "-" + i, 0) == 1) count++;
            
            UIManager.Instance.SetPicture(count, 12);
        }

        if (chapter == "Ch_2")
        {
            name = "Picture2";

            for (int i = 1; i <= 12; i++)
                if (PlayerPrefs.GetInt(name + "-" + i, 0) == 1) count++;

            UIManager.Instance.SetPicture(count, 12);
        }

    }

    public void GiveUp()
    {
        GameManager.Instance.GameOver();
    }
}
