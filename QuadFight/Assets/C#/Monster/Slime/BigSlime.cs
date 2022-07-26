using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlime : Monster
{
    SpriteRenderer sprite;


    public float BS_Hp = 50;

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Monster_Dead();
    }

    public override void Monster_Hit()
    {
        BS_Hp -= 10;
    }
    public override void Monster_Dead()
    {
        if (BS_Hp <= 0)
        {
            BS_Hp = 0;
        }
    }
}
