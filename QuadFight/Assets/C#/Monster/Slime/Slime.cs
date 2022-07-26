using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    public float S_Hp = 20;

    public void Update()
    {
        Debug.Log(S_Hp);
        Monster_Dead();
    }
    public override void Monster_Hit()
    {
        S_Hp -= 10;
    }

    public override void Monster_Dead()
    {
        if (S_Hp <= 0)
        {
            S_Hp = 0;
        }
    }
}
