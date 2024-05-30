using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public Button btn;
    public Image icon;
    public TextMeshProUGUI quantityTxt;
    private Outline outLine;
    public UIInventory inventory;
    public int idx;
    public bool equipped;
    public int quantity;
    // Start is called before the first frame update
    void Awake()
    {
        outLine = GetComponent<Outline>();
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityTxt.text = quantity >1? quantity.ToString() :string.Empty;
    }
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityTxt.text = string.Empty;
    }

    public void OnClickBtn()
    {
        inventory.SelectItem(idx);
    }
}
