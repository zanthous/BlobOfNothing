using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    private EntityStats stats;
    public EntityStats Stats { get => stats; set => stats = value; }

    protected void Init()
    {
        stats = new EntityStats();
    }
}
