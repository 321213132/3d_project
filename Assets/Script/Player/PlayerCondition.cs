using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
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
        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)//체력 회복
    {
        health.Add(amout);
    }

    public void Die()//죽으면 종료
    {
        Application.Quit();
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    //스피드코인 사용시 일정시간동안 속도 증가
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
