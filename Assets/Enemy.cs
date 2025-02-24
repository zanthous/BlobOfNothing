﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase, IDie
{
    private Animator animator;
    
    private void Awake()
    {
        base.Init();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Die()
    {
        animator.SetBool("Dead", true);
        GetComponent<CircleCollider2D>().enabled = false;
        EnemyManager.Instance.RemoveEnemy(gameObject);
        var parts = GetComponentsInChildren<Part>();
        for(int i = 0; i < parts.Length; i++)
        {
            parts[i].DetachFromTarget();
        }
        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
