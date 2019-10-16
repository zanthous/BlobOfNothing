﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float baseMaxHealth = 100.0f;

    private float currentHealth = 100.0f;
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    
    //min time before an entity can take damage again after being damaged
    private float damageInterval = .2f;
    private float timer;
    
    [SerializeField] private AudioSource hurt;

    private EntityStats stats;

    private void Start()
    {
        stats = GetComponent<EntityBase>().Stats;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public float ChangeHealth(float amount)
    {
        //Not enough time passed since last time damage was taken
        if(timer < .2f)
            return currentHealth;

        currentHealth += amount;
        if(amount<0)
        {
            timer = 0.0f;
            if(hurt != null)
            {
                hurt.Play();
            }
        }
        if(currentHealth<=0)
        {
            GetComponent<IDie>().Die();
        }
        return currentHealth;
    }

    //public void UpdateMaxHealth()
    //{
    //    var healthMods = GetComponentsInChildren<HealthModifier>();
    //    maxHealth = defaultMaxHealth;
    //    if(healthMods.Length == 0)
    //    {
    //        return;
    //    }
    //    for(int i = 0; i < healthMods.Length; i++)
    //    {
    //        maxHealth += healthMods[i].Amount;
    //    }
    //}

    public void RestoreToMax()
    {
        currentHealth = GetMaxHealth();
    }

    public float GetMaxHealth()
    {
        return baseMaxHealth + stats.BonusMaxHealth;
    }
}
