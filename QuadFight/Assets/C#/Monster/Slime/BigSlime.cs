using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlime : Monster
{

    public override void Monster_Hit(int damage)
    {
        Debug.Log("빅슬라임 체력" + M_Hp);
        M_Hp -= damage;
        StartCoroutine(BS_changeColor());
        StopCoroutine(BS_changeColor());
    }
    protected override void Awake()
    {
        DeathAction += () => SetState(new BSlimeDead());
        base.Awake();
    }

    IEnumerator BS_changeColor()
    {
        for (int i = 0; i < 2; i++)
        {
            Sprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            Sprite.color = Color.white;
        }
    }
}
#region bigSlime FSM
public class BSlimeIdle : IState<Monster>
{
    public Monster Instance { get; set; }
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
    }

    public virtual void OnUpdate()
    {
        RaycastHit2D hit = Physics2D.CircleCast(Instance.transform.position, 5, Vector2.zero, 5, LayerMask.GetMask("Player"));
        if (hit)
        {
            Instance.target = hit.collider.GetComponent<Player>();
            Instance.Anim.SetBool("S_Walk", true);
            Instance.SetState(new BSlimeMove());
        }
    }

    public virtual void OnExit()
    {

    }
}

public class BSlimeMove : IState<Monster>
{
    public Monster Instance { get; set; }
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
    }
    public virtual void OnUpdate()
    {
        Vector3 dir = (Instance.transform.position - Instance.target.transform.position).normalized;
        Instance.transform.position += dir * Instance.M_MoveSpeed * Time.deltaTime;
        Instance.Anim.SetBool("S_Walk", true);

        if (dir.x != 0)
        {
            Instance.Sprite.flipX = dir.x < 0;
        }

        float dist = Vector2.Distance(Instance.transform.position, Instance.target.transform.position);

        if (!Instance.target || dist < 8)
        {
            Instance.SetState(new BSlimeIdle());
        }

        if (dist < 2f)
        {
            Instance.Anim.SetBool("S_Attack", true);
            Instance.SetState(new BSlimeAttack());
        }
    }
    public virtual void OnExit()
    {
        Instance.Anim.SetBool("S_Walk", false);
    }
}

public class BSlimeAttack : IState<Monster>
{
    public Monster Instance { get; set; }
    Coroutine BS_attackCor;
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
        Instance.Anim.SetBool("S_Attack", true);
        BS_attackCor = Instance.StartCoroutine(BS_Attack());
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {
        Instance.Anim.SetBool("S_Attack", false);
        Instance.StopCoroutine(BS_attackCor);
    }
    IEnumerator BS_Attack()
    {
        if (Instance.target != null)
        {
            Instance.GetComponent<Player>().P_Hit(Instance.M_Damage);
            //Player.player.Hp -= Instance.M_Damage;
            Instance.target.SetState(new PlayerHit());
        }
        Instance.Anim.SetBool("S_Attack", false);
        yield return new WaitForSeconds(1f);
        Instance.SetState(new BSlimeIdle());
    }
}

public class BSlimeDead : IState<Monster>
{
    private  int BS_Score = 500;
    public Monster Instance { get; set; }
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
        Instance.StartCoroutine(BS_Dead());
        GameManager.gamemanager.Score += BS_Score;
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
    IEnumerator BS_Dead()
    {
        for (int i = 0; i < 2; i++)
        {
            Instance.Sprite.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.25f);
            Instance.Sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.25f);
            Instance.IsDeath = true;
        }
        Instance.gameObject.SetActive(false);
    }
}
#endregion
