using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneKnight : Monster
{
    public override void Monster_Hit(int damage)
    {
        Debug.Log("해골기사 체력");
    }
}
