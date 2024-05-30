using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController ctrl;
    public PlayerCondition cond { get; set; }

//    public ItemData itemData;
    public Action addItem;
//    public Equipment equip;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        ctrl = GetComponent<PlayerController>();
        cond = GetComponent<PlayerCondition>();
    }
}