using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : ModifierBase
{
    private IHaveSpeedModifier target = null;

    public override void UpdateEntityParameter()
    {
        if(target != null)//ondetach
        {
            target.UpdateMoveSpeedModifier();
            target = null;
        }
        else//onattach
        {
            var a = GetComponentInParent<IHaveSpeedModifier>();
            if(a != null)
            {
                a.UpdateMoveSpeedModifier();
                target = a;
            }
            else
            {
                var b = GetComponentInParent<IHaveSpeedModifier>();
                if(b == null)
                {
                    Debug.Log("error - speedmodifier");
                    return;
                }
                b.UpdateMoveSpeedModifier();
                target = b;
            }
        }
    }
}
