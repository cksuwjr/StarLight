using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

// 데이터 형태는 SaveDatas.cs 참고

public class DataManager : Singleton<DataManager>, IManager
{
    private bool loadData = false;
    public bool isDataLoad => loadData;

    private string dataPath;


    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    public bool testSkip = false;



    private ChatTable storyDatas;
    private Dictionary<int, ChatData> ContaminatedMushrooms = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> Virus = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> MysteriousTree = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> DollClawMachine = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> RabbitDoll = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> FoodTruck = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> FallenLeaves = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> Log = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> RumiHouse = new Dictionary<int, ChatData>();



    public void Init()
    {
        // '일반' 계정 불러오기

        if (testSkip)
            if (!loadData)
                LoadData();

        if(!storyDatas)
            LoadStoryData();
    }


    #region ___LoadPlayer___

    public bool LoadData()
    {
        loadData = false;

        // To do : 구글 로드


        // 없으면 일반 로드

        dataPath = Application.persistentDataPath + "/Save";
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            playerData = JsonUtility.FromJson<PlayerData>(data);
            loadData = true;
            return loadData;
        }

        return loadData;
    }

    public void SaveData()
    {
        dataPath = Application.persistentDataPath + "/Save";

        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(dataPath, data);
        loadData = true;
    }

    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    public void CreateData(string nickName)
    {
        playerData = new PlayerData();
        playerData.nickName = nickName;
    }

    #endregion

    #region ___LoadStorys___

    private void LoadStoryData()
    {
        storyDatas = Resources.Load<ChatTable>("ChatData/ChatTable");

        Debug.Log("스토리불러오기");

        for (int i = 0; i < storyDatas.ContaminatedMushrooms.Count; i++)
            ContaminatedMushrooms.Add(storyDatas.ContaminatedMushrooms[i].id, storyDatas.ContaminatedMushrooms[i]);
        for (int i = 0; i < storyDatas.Virus.Count; i++)
            Virus.Add(storyDatas.Virus[i].id, storyDatas.Virus[i]);
        for (int i = 0; i < storyDatas.MysteriousTree.Count; i++)
            MysteriousTree.Add(storyDatas.MysteriousTree[i].id, storyDatas.MysteriousTree[i]);
        for (int i = 0; i < storyDatas.DollClawMachine.Count; i++)
            DollClawMachine.Add(storyDatas.DollClawMachine[i].id, storyDatas.DollClawMachine[i]);
        for (int i = 0; i < storyDatas.RabbitDoll.Count; i++)
            RabbitDoll.Add(storyDatas.RabbitDoll[i].id, storyDatas.RabbitDoll[i]);
        for (int i = 0; i < storyDatas.FoodTruck.Count; i++)
            FoodTruck.Add(storyDatas.FoodTruck[i].id, storyDatas.FoodTruck[i]);
        for (int i = 0; i < storyDatas.FallenLeaves.Count; i++)
            FallenLeaves.Add(storyDatas.FallenLeaves[i].id, storyDatas.FallenLeaves[i]);
        for (int i = 0; i < storyDatas.Log.Count; i++)
            Log.Add(storyDatas.Log[i].id, storyDatas.Log[i]);
        for (int i = 0; i < storyDatas.RumiHouse.Count; i++)
            RumiHouse.Add(storyDatas.RumiHouse[i].id, storyDatas.RumiHouse[i]);



        //PlayerPrefs.SetInt(  "ContaminatedMushrooms"   , PlayerPrefs.GetInt(  "ContaminatedMushrooms"     , 0));
        //PlayerPrefs.SetInt(  "Virus"                   , PlayerPrefs.GetInt(  "Virus"                     , 0));
        //PlayerPrefs.SetInt(  "MysteriousTree"          , PlayerPrefs.GetInt(  "MysteriousTree"            , 0));
        //PlayerPrefs.SetInt(  "DollClawMachine"         , PlayerPrefs.GetInt(  "DollClawMachine"           , 0));
        //PlayerPrefs.SetInt(  "RabbitDoll"              , PlayerPrefs.GetInt(  "RabbitDoll"                , 0));
        //PlayerPrefs.SetInt(  "FoodTruck"               , PlayerPrefs.GetInt(  "FoodTruck"                 , 0));
        //PlayerPrefs.SetInt(  "FallenLeaves"            , PlayerPrefs.GetInt(  "FallenLeaves"              , 0));
        //PlayerPrefs.SetInt(  "Log"                     , PlayerPrefs.GetInt(  "Log"                       , 0));
        //PlayerPrefs.SetInt(  "RumiHouse"               , PlayerPrefs.GetInt(  "RumiHouse"                 , 0));

        //PlayerPrefs.Save();
    }


    #endregion

    #region ___ReturnData___

    public Dictionary<int, ChatData> GetStoryData(StoryType type)
    {
        switch (type)
        {
                case StoryType.                 ContaminatedMushrooms               :     return ContaminatedMushrooms;
                case StoryType.                 Virus                               :     return Virus;
                case StoryType.                 MysteriousTree                      :     return MysteriousTree;
                case StoryType.                 DollClawMachine                     :     return DollClawMachine;
                case StoryType.                 RabbitDoll                          :     return RabbitDoll;
                case StoryType.                 FoodTruck                           :     return FoodTruck;
                case StoryType.                 FallenLeaves                        :     return FallenLeaves;
                case StoryType.                 Log                                 :     return Log;
                case StoryType.                 RumiHouse                           :     return RumiHouse;
        }

        return null;
    }

    #endregion
}
