using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ScenarioManager : SingletonDestroy<ScenarioManager>
{
    private Dictionary<int, ChatData> story;
    private int page = 0;

    public Action OnStoryEnd;

    private int star = 0;


    public void StartStory(string typeName)
    {
        var type = StoryType.ContaminatedMushrooms;
        switch (typeName)
        {
            case "ContaminatedMushrooms": type = StoryType.ContaminatedMushrooms     ;             break;
            case "Virus"                : type = StoryType.Virus                     ;             break;
            case "MysteriousTree"       : type = StoryType.MysteriousTree            ;             break;
            case "DollClawMachine"      : type = StoryType.DollClawMachine           ;             break;
            case "RabbitDoll"           : type = StoryType.RabbitDoll                ;             break;
            case "FoodTruck"            : type = StoryType.FoodTruck                 ;             break;
            case "FallenLeaves"         : type = StoryType.FallenLeaves              ;             break;
            case "Log"                  : type = StoryType.Log                       ;             break;
            case "RumiHouse"            : type = StoryType.RumiHouse                 ;             break;
            default:
                break;
        }

        StartStory(type);

    }

    public void StartStory(StoryType type)
    {
        story = DataManager.Instance.GetStoryData(type);
        page = 0;

        AddStar();

        var imgSprite = Resources.Load<UnityEngine.Sprite>(story[page].imgSrc);
        var sound = Resources.Load<AudioClip>(story[page].ttsSrc);

        UIManager.Instance.SetScenarioPannel(imgSprite, story[page].name, story[page].chat);
        SoundManager.Instance.PlaySound(sound, false);
        UIManager.Instance.OpenScenarioPannel();


        switch (type.ToString())
        {
           case "ContaminatedMushrooms": type = StoryType.ContaminatedMushrooms     ;                 break;
           case "Virus"                : OnStoryEnd += ()=> { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-1Puzzle"); };break;
           case "MysteriousTree"       : type = StoryType.MysteriousTree            ;             break;
           case "DollClawMachine"      : type = StoryType.DollClawMachine           ;             break;
           case "RabbitDoll"           : OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-2Puzzle"); }; break;
           case "FoodTruck"            : type = StoryType.FoodTruck                 ;             break;
           case "FallenLeaves"         : type = StoryType.FallenLeaves              ;             break;
           case "Log"                  : OnStoryEnd += () => { GameManager.Instance.SavePosition(); GameManager.Instance.LoadScene("1-3Puzzle"); }; break;
           case "RumiHouse"            : OnStoryEnd += () => 
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
        OnStoryEnd?.Invoke();
        OnStoryEnd = null;

        story = null;
        page = 0;
        UIManager.Instance.CloseScenarioPannel();
    }

    public void NextPage()
    {
        if (page + 1 < story.Count)
        {
            page++;
            var imgSprite = Resources.Load<Sprite>(story[page].imgSrc);
            var sound = Resources.Load<AudioClip>(story[page].ttsSrc);

            UIManager.Instance.SetScenarioPannel(imgSprite, story[page].name, story[page].chat);
            SoundManager.Instance.StopSound();
            SoundManager.Instance.PlaySound(sound, false);
        }
        else
            StopStory();
    }

    public void AddStar()
    {
        star++;
        UIManager.Instance.SetStar(star);
    }
}
