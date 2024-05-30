using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public event Action onTakeDamage;
    public float hungerDecay;
    public float healthDecay;
    public UIConditions uiCondition;

    Condition health { get { return uiCondition.Health; } }
    Condition hunger { get { return uiCondition.Hunger; } }

    private void Update()
    {
        hunger.Subtract(hungerDecay * Time.deltaTime);
        if(hunger.curValue <= 0)
        {
            health.Subtract(healthDecay * Time.deltaTime);
        }
        if (health.curValue <= 0)
            Die();

    }

    public void Heal(float value)
    {
        health.Add(value);
    }
    public void Eat(float value)
    {
        hunger.Add(value);
    }

    private void Die()
    {
        Debug.Log("Die");
    }

    public bool UseHunger(float value)
    {
        if (hunger.curValue < value)
            return false;
        hunger.Subtract(value);
        return true;
    }
}