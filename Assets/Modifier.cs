using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierCriteria
{

}
public enum ModifierType
{
    speed,
    health
}

public class Modifier : MonoBehaviour
{
    [SerializeField] private float amount;
    public float Amount { get => amount; set => amount = value; }

    [SerializeField] private ModifierCriteria criteria;
    public ModifierCriteria Criteria { get => criteria; set => criteria = value; }

    [SerializeField] private ModifierType type;
    public ModifierType Type { get => type; set => type = value; }
    
}

