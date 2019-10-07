using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierBase : MonoBehaviour
{
    [SerializeField]
    protected float amount;
    public float Amount { get => amount; set => amount = value; }
    
    public abstract void UpdateEntityParameter();
}
