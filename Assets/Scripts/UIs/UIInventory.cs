using System;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject invenWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    // Selected Item
    public TextMeshProUGUI SelectedItemName;
    public TextMeshProUGUI SelectedItemDescription;
    public GameObject useBtn;
    public GameObject dropBtn;

    private PlayerController ctrl;
    private PlayerCondition cond;

    ItemData selectedItemData;
    int selectedItemIdx;

    private void Start()
    {
        ctrl = PlayerManager.Instance.Player.ctrl;
        cond = PlayerManager.Instance.Player.cond;
        dropPosition = PlayerManager.Instance.Player.GetComponent<Transform>();
        invenWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];
        for(int a=0; a<slots.Length; a++)
        {
            slots[a] = slotPanel.GetChild(a).GetComponent<ItemSlot>();
            slots[a].idx = a;
            slots[a].inventory = this;
        }
        ctrl.Inventory += Toggle;
        PlayerManager.Instance.Player.addItem += AddItem;

        ClearSelectedItemWindow();
    }

    private void ClearSelectedItemWindow()
    {
        SelectedItemName.text = "";
        SelectedItemDescription.text = "";
        useBtn.SetActive(false);
        dropBtn.SetActive(false);
    }

    private void Toggle()
    {
        invenWindow.SetActive(!IsOpen());
    }

    private bool IsOpen()
    {
        return invenWindow.activeInHierarchy;
    }

    private void AddItem()
    {
        ItemData item = PlayerManager.Instance.Player.itemData;

        if(item.canStack)
        {
            ItemSlot slot = GetItemStack(item);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                PlayerManager.Instance.Player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptyItemSlot();
        if(emptySlot != null )
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            PlayerManager.Instance.Player.itemData = null;
            return;
        }
        ThrowItem(item);

    }
    private void UpdateUI()
    {
        for(int a=0; a<slots.Length; a++)
        {
            if (slots[a].item != null)
            {
                slots[a].Set();
            }
            else
            {
                slots[a].Clear();
            }
        }
    }
    private ItemSlot GetItemStack(ItemData item)
    {
        foreach(ItemSlot slot in  slots)
        {
            if(slot.item == item && slot.quantity < item.maxStackAmount)
                return slot;
        }
        return null;
    }
    private ItemSlot GetEmptyItemSlot()
    {
        foreach(ItemSlot slot in slots)
        {
            if (slot.item == null)
                return slot;
        }
        return null;
    }

    private void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }

    public void SelectItem(int idx)
    {
        if (slots[idx].item == null) return;
        selectedItemData = slots[idx].item;
        selectedItemIdx = idx;

        SelectedItemName.text = selectedItemData.name;
        SelectedItemDescription.text = selectedItemData.desc;

        for(int a=0; a<selectedItemData.consumables?.Length; a++)
        {
            SelectedItemDescription.text += $"\n{selectedItemData.consumables[a].type}: ";
            SelectedItemDescription.text += $"+{selectedItemData.consumables[a].value}\n";
        }
        useBtn.SetActive(selectedItemData.type == ItemType.Consumable);
        dropBtn.SetActive(true);
    }

    public void OnUse()
    {
        if (selectedItemData.type != ItemType.Consumable) return;
        for(int a=0; a<selectedItemData.consumables.Length;a++)
        {
            switch(selectedItemData.consumables[a].type) 
            {
                case ConsumableType.Health:
                    cond.Heal(selectedItemData.consumables[a].value);
                    break;
                case ConsumableType.Hunger:
                    cond.Eat(selectedItemData.consumables[a].value);
                    break;
            }
        }
        RemoveSelectedItem();
    }

    public void OnDrop()
    {
        ThrowItem(selectedItemData);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        slots[selectedItemIdx].quantity--;
        if (slots[selectedItemIdx].quantity <= 0 )
        {
            selectedItemData = null;
            slots[selectedItemIdx].item = null;
            selectedItemIdx = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }
}