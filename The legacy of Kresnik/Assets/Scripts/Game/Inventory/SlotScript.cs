﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    [SerializeField]
    private Text stackSize;

    public BagScript MyBag { get; set; }

    public int MyIndex { get; set; }

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image cover;
    public Image MyCover
    {
        get
        {
            return cover;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return items.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return items.Peek();
            }

            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return items.Count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable != null)
                {
                    if (HandScript.MyInstance.MyMoveable is Bag)
                    {
                        if (MyItem is Bag)
                        {
                            InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                        }
                    }

                    else if (HandScript.MyInstance.MyMoveable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType && Player.MyInstance.IsAlive)
                        {
                            (MyItem as Armor).Equip();
                            HandScript.MyInstance.Drop();
                        }
                    }
                }

                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }
            }

            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable is Bag)
                {
                    Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                    if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.MySlotCount > 0)
                    {
                        AddItem(bag);
                        bag.MyBagButton.RemoveBag();
                        HandScript.MyInstance.Drop();
                    }
                }

                else if (HandScript.MyInstance.MyMoveable is Armor)
                {
                    Armor armor = (Armor)HandScript.MyInstance.MyMoveable;
                    CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                    AddItem(armor);
                    HandScript.MyInstance.Drop();
                }
            }

            else if (InventoryScript.MyInstance.FromSlot != null)
            {
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.items))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMoveable == null)
        {
            UseItem();
        }
    }

    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        MyCover.enabled = false;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        int initCount = MyItems.Count;
        MyCover.enabled = false;
        if (initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
        }
    }

    public void UseItem()
    {
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }

        else if (MyItem is Armor && Player.MyInstance.IsAlive)
        {
            (MyItem as Armor).Equip();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        MyCover.enabled = false;
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.enabled = true;
            return true;
        }
        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        from.MyCover.enabled = false;
        if (IsEmpty)
        {
            return false;
        }

        if(from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            from.MyItems.Clear();
            from.AddItems(MyItems);

            MyItems.Clear();
            AddItems(tmpFrom);

            return true;
        }
        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull && from.MyItem.MyTitle == MyItem.MyTitle)
        {
            int free = MyItem.MyStackSize - MyCount;

            for(int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0),transform.position, MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideToolTip();
    }
}
