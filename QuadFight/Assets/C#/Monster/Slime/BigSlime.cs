using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlime : Monster
{
    public override void Monster_Hit(int damage)
    {
        M_Hp -= damage;
    }
}
