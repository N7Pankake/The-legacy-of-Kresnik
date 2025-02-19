﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;

    private NPC npc;

    public virtual void CloseWindow()
    {
        npc.IsInteracting = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        npc = null;
    }

    public virtual void OpenWindow(NPC npc)
    {
        this.npc = npc;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
