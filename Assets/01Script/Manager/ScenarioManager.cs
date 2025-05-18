using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class ScenarioManager : SingletonDestroy<ScenarioManager>
{
    private Dictionary<int, ChatData> story;
    private int page = 0;

    public Action OnStoryEnd;

    private int star = 0;


    private bool texting = false;

    public void SetChapter(string name)
    {
        var type = StoryType.Lobby;
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

        AddStar();

        LoadPage();
        UIManager.Instance.OpenScenarioPannel();

        switch (num)
        {
           case 3                : OnStoryEnd += ()=> { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-1Puzzle"); };break;
           case 13           : OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-2Puzzle"); }; break;
           case 23                  : OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-3Puzzle"); }; break;
           case 30:
                OnStoryEnd += () =>
                {
                    var videoPlay = GameObject.Find("Video Player").GetComponent<VideoPlay>();
                    videoPlay.Play();
                    videoPlay.OnVideoEnd += () => { UIManager.Instance.OpenClearPopup(true); };
                };
                break;
            default:
                break;
        }
    }

    public void StopStory()
    {
        SoundManager.Instance.StopSound();

        OnStoryEnd?.Invoke();
        OnStoryEnd = null;

        story = null;
        page = 0;
        UIManager.Instance.CloseScenarioPannel();
    }

    public void NextPage()
    {
        if (texting)
        {
            texting = false;
            return;
        }

        if (story.Count <= page + 1)
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

        SoundManager.Instance.StopSound();
        if (sound) 
            SoundManager.Instance.PlaySound(sound, false);

        StartCoroutine(LoadText(imgSprite, story[page].name, story[page].chat));
        //UIManager.Instance.SetScenarioPannel(imgSprite, story[page].name, story[page].chat);
    }

    private IEnumerator LoadText(Sprite sprite, string name, string chat)
    {
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


    public void AddStar()
    {
        star++;
        UIManager.Instance.SetStar(star);
    }
}
