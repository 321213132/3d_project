using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum EffectType//포션 효과
{
    Health,
    Speed
}

[Serializable]

public class ItemDataEffect 
{
    public EffectType type;
    public float value;
}


[CreateAssetMenu(fileName = "Itme", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStack;

    [Header("Effect")]
    public ItemDataEffect[] effects;

}
