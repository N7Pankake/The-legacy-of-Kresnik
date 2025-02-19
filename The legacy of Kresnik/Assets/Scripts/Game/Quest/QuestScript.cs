﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }

    private bool markedComplete = false;

    public void Select()
    {
        GetComponent<TextMeshProUGUI>().color = Color.yellow;
        QuestLog.MyInstance.ShowDescription(MyQuest);
    }

    public void Deselect()
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void IsComplete()
    {
        if(MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<TextMeshProUGUI>().text += " [" + MyQuest.MyLevel + "] " + MyQuest.MyTitle + "(C)";
            MessageFeedManager.MyInstance.WriteMessage(string.Format("{0} (C)", MyQuest.MyTitle));
        }
        else if (!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<TextMeshProUGUI>().text = " [" + MyQuest.MyLevel + "] " + MyQuest.MyTitle;
        }
    }
}
