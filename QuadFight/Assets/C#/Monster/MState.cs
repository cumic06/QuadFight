using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface MState<T> where T : MonoBehaviour
{
    void OnEnter(T instance);
    void Update();
    void OnExit();
}

public class MonsterState : MState<Monster>
{
    protected Monster monster;
    public Transform slimeTransform;
    public Slime slime;

    public bool M_isIdle = false;
    public bool M_isWalk = false;
    public bool M_isAttack = false;
    public bool M_isDead = false;

    public virtual void OnEnter(Monster monster)
    {
        slimeTransform = slime.Anim.GetComponent<Transform>();

        if (this.monster == null)
        {
            this.monster = monster;
        }
    }

    public virtual void Update()
    {

    }

    public virtual void OnExit()
    {

    }
}
public class MonsterIdle : MonsterState
{
    public override void OnEnter(Monster monster)
    {
        if (Vector2.Distance(slimeTransform.position, slime.player.position) <= 6)
        {
            slime.Anim.SetBool("S_isReady", true);
        }
        M_isIdle = true;

        base.OnEnter(monster);
    }
    public override void Update()
    {
        if (Vector2.Distance(slimeTransform.position, slime.player.position) <= 4)
        {
            slime.Anim.SetBool("S_isFollow", true);
            monster.SetState<MonsterFollow>(nameof(MonsterFollow));
        }
        base.Update();
    }
}
public class MonsterFollow : MonsterState
{

    public override void OnEnter(Monster monster)
    {
        M_isWalk = true;
        base.OnEnter(monster);
    }
    public override void Update()
    {
        if (Vector2.Distance(slime.player.position, slimeTransform.position) > 4f)
        {
            slime.Anim.SetBool("isReady", true);
            slime.Anim.SetBool("isFollow", false);
        }
        else if (Vector2.Distance(slime.player.position, slimeTransform.position) > 1f)
        {
            slimeTransform.position = Vector2.MoveTowards(slimeTransform.position, slime.player.position, Time.deltaTime * slime.M_speed);
            slime.Anim.SetBool("isReady", false);
            slime.Anim.SetBool("isFollow", true);
        }
        else if (Vector2.Distance(slime.player.position, slimeTransform.position) > 0.5f)
        {
            monster.SetState<MonsterAttack>(nameof(MonsterAttack));
        }
    }
}

public class MonsterAttack : MonsterState
{
    public override void OnEnter(Monster monster)
    {
        M_isAttack = true;
        base.OnEnter(monster);
    }
}