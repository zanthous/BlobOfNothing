//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpeedModifier : ModifierBase, IModifier
//{
//    private IModifier target = null;
    
//    public void ApplyModifier()
//    {
//        if(target != null)//ondetach
//        {
//            target.ApplyModifier();
//            target = null;
//        }
//        else//onattach
//        {
//            var a = GetComponentInParent<EnemyMovement>();
//            if(a != null)
//            {
//                a.ApplyModifier();
//                target = a;
//            }
//            else
//            {
//                var b = GetComponentInParent<IModifier>();
//                if(b == null)
//                {
//                    Debug.Log("error - speedmodifier");
//                    return;
//                }
//                b.ApplyModifier();
//                target = b;
//            }
//        }
//    }
//}
