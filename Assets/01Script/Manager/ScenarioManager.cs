using System;
using System.Collections;
using System.Collections.Generic;
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
                //case 1: OnStoryEnd += () => { StartStory(2); }; break;
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
                case 29:
                    OnStoryEnd += () =>
                    {
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

        SoundManager.Instance.StopSound();
        if (sound) 
            SoundManager.Instance.PlaySound(sound, 0.7f, false);

        if(size == "large")
            StartCoroutine(LoadText(imgSprite, story[page].name, story[page].chat));
        else
            StartCoroutine(LoadTextSmall(imgSprite, story[page].name, story[page].chat));

        //UIManager.Instance.SetScenarioPannel(imgSprite, story[page].name, story[page].chat);
    }

    private IEnumerator LoadText(Sprite sprite, string name, string chat)
    {
        UIManager.Instance.OpenScenarioPannel();
        UIManager.Instance.CloseSmallScenarioPannel();

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
}
