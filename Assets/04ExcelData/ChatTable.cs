using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoryType
{
    Lobby,
    Tutorial,
    Ch_1,
    Ch_2,
    Loading,
}

[ExcelAsset]
public class ChatTable : ScriptableObject
{
    public List<ChatData> Lobby;
    public List<ChatData> Tutorial;
    public List<ChatData> Ch_1;
    public List<ChatData> Ch_2;
    public List<ChatData> Loading;
}
