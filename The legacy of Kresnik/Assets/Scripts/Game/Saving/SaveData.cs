﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public InventoryData MyInventory { get; set; }

    public PlayerData MyPlayerData { get; set; }

    public List<ChestData> MyChestData { get; set; }

    public List<GearData> MyGearData { get; set; }

    public InventoryData MyInventoryData { get; set; }

    public List<ActionButtonData> MyActionButtonData { get; set; }

    public List<QuestData> MyQuestData { get; set; }

    public List<QuestGiverData> MyQuestGiverData { get; set; }

    public DateTime MyDateTime { get; set; }

    public string MyScene { get; set; }

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyChestData = new List<ChestData>();
        MyGearData = new List<GearData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
        MyDateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }

    public float MyXp { get; set; }

    public float MyMaxXp { get; set; }

    public float MyHealth { get; set; }

    public float MyMaxHealth { get; set; }

    public float MyMana { get; set; }

    public float MyMaxMana { get; set; }

    public int MyCurrentGold { get; set; }

    public float MyX { get; set; }

    public float MyY { get; set; }

    public PlayerData(int level, float xp, float maxXP, float health, float maxHealth, float mana, float maxMana, int currentGold, Vector2 position)
    {
        this.MyLevel = level;
        this.MyXp = xp;
        this.MyMaxXp = maxXP;

        this.MyHealth = health;
        this.MyMaxHealth = maxHealth;

        this.MyMana = mana;
        this.MyMaxMana = maxMana;

        this.MyCurrentGold = currentGold;

        this.MyX = position.x;
        this.MyY = position.y;
    }
}

[Serializable]
public class ItemData
{
    public string MyTitle { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }
    public int MyBagIndex { get; set; }

    public ItemData(string title, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        MyBagIndex = bagIndex;
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
    }
}

[Serializable]
public class ChestData
{
    public string MyName { get; set; }
    public List<ItemData> MyItems { get; set; }

    public ChestData(string name)
    {
        MyName = name;
        MyItems = new List<ItemData>();

    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }

    public List<ItemData> MyItems { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class BagData
{
    public int MySlotCount { get; set; }
    public int MyBagIndex { get; set; }

    public BagData(int count, int index)
    {
        MySlotCount = count;
        MyBagIndex = index;
    }
}

[Serializable]
public class GearData
{
    public string MyTitle { get; set; }
    public string MyType { get; set; }

    public GearData(string title, string type)
    {
        MyTitle = title;
        MyType = type;
    }
}

[Serializable]
public class ActionButtonData
{
    public string MyAction { get; set; }
    
    public bool IsItem { get; set; }

    public int MyIndex { get; set; }

    public ActionButtonData(string action, bool isItem, int index)
    {
        this.MyAction = action;
        this.IsItem = isItem;
        this.MyIndex = index;
    }
}

[Serializable]
public class QuestData
{
    public string MyTitle { get; set; }

    public string MyDescription { get; set; }

    public CollectObjective[] MyCollectObjectives { get; set; }

    public KillObjective[] MyKillObjectives { get; set; }

    public int MyQuestGiverID { get; set; }

    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverID)
    {
        MyTitle = title;

        MyDescription = description;

        MyCollectObjectives = collectObjectives;

        MyKillObjectives = killObjectives;

        MyQuestGiverID = questGiverID;
    }
}

[Serializable]
public class QuestGiverData
{
    public List<string> MyCompletedQuests { get; set; }

    public int MyQuestGiverID { get; set; }

    public QuestGiverData(int questGiverID, List<string> completedQuests)
    {
        this.MyQuestGiverID = questGiverID;
        MyCompletedQuests = completedQuests;
    }
}