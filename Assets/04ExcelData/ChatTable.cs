using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoryType
{
    ContaminatedMushrooms,
    Virus,
    MysteriousTree,
    DollClawMachine,
    RabbitDoll,
    FoodTruck,
    FallenLeaves,
    Log,
    RumiHouse,
}

[ExcelAsset]
public class ChatTable : ScriptableObject
{
    public List<ChatData> ContaminatedMushrooms;
    public List<ChatData> Virus;
    public List<ChatData> MysteriousTree;
    public List<ChatData> DollClawMachine;
    public List<ChatData> RabbitDoll;
    public List<ChatData> FoodTruck;
    public List<ChatData> FallenLeaves;
    public List<ChatData> Log;
    public List<ChatData> RumiHouse;
}
