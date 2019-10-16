using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains all stats used by other scripts
public class EntityStats 
{
    //stat bonuses
    private float bonusMaxHealth;
    private float bonusMoveSpeed;

    private List<Modifier> modifiers;

    private bool initialized = false;

    public float BonusMaxHealth { get => bonusMaxHealth; set => bonusMaxHealth = value; }
    public float BonusMoveSpeed { get => bonusMoveSpeed; set => bonusMoveSpeed = value; }

    public EntityStats()
    {
        modifiers = new List<Modifier>();
        InitializeStats();
    }

    public void AddStat(Modifier modifier)
    {
        Debug.Assert(!modifiers.Contains(modifier));
        modifiers.Add(modifier);
        UpdateStat(modifier,true);
    }
    public void RemoveStat(Modifier modifier)
    {
        Debug.Assert(modifiers.Contains(modifier));
        modifiers.Remove(modifier);
        UpdateStat(modifier, false);
    }

    private void UpdateStat(Modifier modifier, bool add)
    {
        switch(modifier.Type)
        {
            case ModifierType.speed:
                BonusMoveSpeed += add ? modifier.Amount : -modifier.Amount;
                break;
            case ModifierType.health:
                BonusMaxHealth += add ? modifier.Amount : -modifier.Amount;
                break;
        }
    }

    private void InitializeStats()
    {
        ResetStats();
        for(int i = 0; i < modifiers.Count; i++)
        {
            UpdateStat(modifiers[i],true);
        }
        initialized = true;
    }

    private void ResetStats()
    {
        BonusMaxHealth = 0;
        BonusMoveSpeed = 0;
    }
}