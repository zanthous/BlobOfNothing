using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModifier : ModifierBase
{
    public override void UpdateEntityParameter()
    {
        var a = GetComponentInParent<Health>();
        a?.UpdateMaxHealth();
    }
}
