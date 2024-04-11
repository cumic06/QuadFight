using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    public override void Monster_Hit(int damage)
    {
        Debug.Log("슬라임 체력" + M_Hp);
        M_Hp -= damage;
        StartCoroutine(S_changeColor());
        //FXManager.instance.FxSpawn();
    }
    protected override void Awake()
    {
        base.Awake();
        DeathAction += () => SetState(new SlimeDead());
    }
    IEnumerator S_changeColor()
    {
        for (int i = 0; i < 2; i++)
        {
            Sprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            Sprite.color = Color.white;
        }
    }
    protected override void Start()
    {
        SetState(new SlimeIdle());
    }
}

public class SlimeIdle : IState<Monster>
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
            Instance.SetState(new SlimeMove());
        }
    }

    public virtual void OnExit()
    {

    }
}

public class SlimeMove : IState<Monster>
{
    public Monster Instance { get; set; }
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
    }

    public virtual void OnUpdate()
    {
        Vector3 dir = (Instance.target.transform.position - Instance.transform.position).normalized;
        Instance.transform.position += dir * Instance.M_MoveSpeed * Time.deltaTime;
        Instance.Anim.SetBool("S_Walk", true);

        if (dir.x != 0)
        {
            Instance.Sprite.flipX = dir.x < 0;
        }

        float dist = Vector2.Distance(Instance.transform.position, Instance.target.transform.position);
        if (!Instance.target || dist > 8)
        {
            Instance.SetState(new SlimeIdle());
        }
        if (dist < 1.5f)
        {
            Instance.SetState(new SlimeAttack());
        }
    }
    public virtual void OnExit()
    {
        Instance.Anim.SetBool("S_Walk", false);
    }

}
public class SlimeAttack : IState<Monster>
{
    public Monster Instance { get; set; }
    Coroutine S_attackCor;
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
        S_attackCor = Instance.StartCoroutine(Slime_Attack());
    }
    public virtual void OnUpdate()
    {
    }
    public virtual void OnExit()
    {
        Instance.Anim.SetBool("S_Attack", false);
        Instance.StopCoroutine(S_attackCor);
    }
    IEnumerator Slime_Attack()
    {
        if (Instance.target != null)
        {
            Instance.Anim.SetBool("S_Attack", true);
            Player.Instance.P_Hit(Instance.M_Damage);
        }
        yield return new WaitForSeconds(1f);
        Instance.Anim.SetBool("S_Attack", false);
        Instance.SetState(new SlimeIdle());
    }
}

public class SlimeDead : IState<Monster>
{
    public Monster Instance { get; set; }
    private int S_Score = 100;

    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
        Instance.StartCoroutine(S_Dead());
        GameManager.Instance.Score += S_Score;
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }

    IEnumerator S_Dead()
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
