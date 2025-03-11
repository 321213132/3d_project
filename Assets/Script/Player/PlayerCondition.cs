using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhtsicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour , IDamagalbe
{
    public UiCondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina {  get { return uiCondition.stamina; } }

    public event Action onTakeDamage;
    public bool isBoost = false;

    void FixedUpdate()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void Die()
    {

    }

    public void TakePhtsicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }
    public IEnumerator BoostSpeed(float amount)
    {
        if(isBoost)
        {
            yield break;
        }

        isBoost = true;
        CharacterManager.Instance.Player.controller.moveSpeed += amount;
        Debug.Log("Speed boost started: " + CharacterManager.Instance.Player.controller.moveSpeed);
        
        yield return new WaitForSeconds(3f);
        
        CharacterManager.Instance.Player.controller.moveSpeed -= amount;
        isBoost = false;
        Debug.Log("Speed boost ended: " + CharacterManager.Instance.Player.controller.moveSpeed);
    }

    public void StartSpeedBoost(float amount)
    {
        if(!isBoost)
        {
            StartCoroutine(BoostSpeed(amount));
        }
    }
}
