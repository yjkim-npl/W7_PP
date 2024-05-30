using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string name;
    public string desc;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
