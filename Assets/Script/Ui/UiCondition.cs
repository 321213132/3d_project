using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCondition : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    void Update()
    {
        
    }
}
