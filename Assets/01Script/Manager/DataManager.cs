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



    private ChatTable datas;
    private Dictionary<int, ChatData> Lobby = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> Tutorial = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> Ch_1 = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> Ch_2 = new Dictionary<int, ChatData>();
    private Dictionary<int, ChatData> Loading = new Dictionary<int, ChatData>();



    public void Init()
    {
        // '일반' 계정 불러오기

        if (!testSkip)
            if (!loadData)
                LoadData();

        if(!datas)
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
        datas = Resources.Load<ChatTable>("ChatData/ChatTable");

        Debug.Log("스토리불러오기");

        for (int i = 0; i < datas.Lobby.Count; i++)
            Lobby.Add(datas.Lobby[i].id, datas.Lobby[i]);
        for (int i = 0; i < datas.Tutorial.Count; i++)
            Tutorial.Add(datas.Tutorial[i].id, datas.Tutorial[i]);
        for (int i = 0; i < datas.Ch_1.Count; i++)
            Ch_1.Add(datas.Ch_1[i].id, datas.Ch_1[i]);
        for (int i = 0; i < datas.Ch_2.Count; i++)
            Ch_2.Add(datas.Ch_2[i].id, datas.Ch_2[i]);
        for (int i = 0; i < datas.Loading.Count; i++)
            Loading.Add(datas.Loading[i].id, datas.Loading[i]);
    }


    #endregion

    #region ___ReturnData___

    public Dictionary<int, ChatData> GetStoryData(StoryType type)
    {
        switch (type)
        {
                case StoryType.                 Lobby               :     return Lobby;
                case StoryType.                 Tutorial                               :     return Tutorial;
                case StoryType.                 Ch_1                      :     return Ch_1;
                case StoryType.                 Ch_2                     :     return Ch_2;
                case StoryType.                 Loading                          :     return Loading;
        }

        return null;
    }

    #endregion
}
