using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase, IDie
{
    private GameObject lastCheckpoint;
    public GameObject LastCheckpoint { get => lastCheckpoint; set => lastCheckpoint = value; }

    private bool partOrbiting = false;
    public bool PartOrbiting
    {
        get
        {
            return partOrbiting;
        }

        set
        {
            if(value == true)
                pickup?.Play();
            partOrbiting = value;
        }
    }
    
    private bool dead = false;
    public bool Dead { get => dead; set => dead = value; }


    private int nStars = 0;
    public int NStars { get => nStars; set => nStars = value; }
    
    private float respawnTimer = 2.0f;

    private Animator animator;
    private Health health;
    
    [SerializeField] private AudioSource pickup;

    private void Awake()
    {
        base.Init();
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    public void Die()
    {
        dead = true;
        animator.SetBool("Dead",true);
        StartCoroutine(RevivePlayer());
    }

    private IEnumerator RevivePlayer()
    {
        yield return new WaitForSeconds(respawnTimer);
        dead = false;
        transform.position = lastCheckpoint.transform.position;
        animator.SetBool("Dead", false);
        health.RestoreToMax();
    }
}
