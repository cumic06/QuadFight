using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Monster : MonoFSM<Monster>
{
    #region 변수
    protected Action DeathAction { get; set; }

    [Header("몹체력")]
    [SerializeField] protected int M_maxHp;
    [SerializeField] protected int M_hp;

    public int M_Hp
    {
        get => M_hp;
        set
        {
            M_hp = Mathf.Clamp(value, 0, M_maxHp);
            if (M_hp <= 0)
            {
                DeathAction?.Invoke();
            }
        }
    }

    [Header("몹공격력")]

    [SerializeField] protected int M_damage;

    public int M_Damage => M_damage;

    [Header("몹이동 속도")]
    [SerializeField] protected float M_moveSpeed = 1;
    public float M_MoveSpeed => M_moveSpeed;

    [Header("플레이어 감지 범위")]
    [SerializeField] protected float findDis = 3;
    public float FindDis => findDis;

    [Header("무적시간")]
    [SerializeField] float hitDelay = 0.1f;
    public float HitDelay => hitDelay;

    [HideInInspector] public Player target;

    [HideInInspector] public bool IsDeath;
    #endregion

    #region Component
    private Animator anim;
    public Animator Anim => anim;

    private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    #endregion
    public abstract void Monster_Hit(int damage);

    public override void SetState(IState<Monster> state)
    {
        if (IsDeath)
        {
            gameObject.SetActive(false);
            return;
        }
        base.SetState(state);
    }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    protected virtual void Start()
    {
        SetState(new MonsterIdle());
    }
}
#region Monster FSM
public class MonsterIdle : IState<Monster>
{
    public Monster Instance { get; set; }
    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
    }
    public virtual void OnUpdate()
    {
        RaycastHit2D find = Physics2D.CircleCast(Instance.transform.position, 5, Vector2.zero, 5, LayerMask.GetMask("Player"));
        if (find)
        {
            Instance.target = find.collider.GetComponent<Player>();
            Instance.SetState(new MonsterMove());
        }
    }

    public virtual void OnExit()
    {

    }
}
public class MonsterMove : IState<Monster>
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

        if (dir.x != 0)
        {
            Instance.Sprite.flipX = dir.x < 0;
        }

        float dist = Vector2.Distance(Instance.transform.position, Instance.target.transform.position);
        if (!Instance.target || dist > 8)
        {
            Instance.SetState(new MonsterIdle());
        }

        if (dist < 1f)
        {
            Instance.SetState(new MonsterAttack());
        }
    }

    public virtual void OnExit()
    {

    }
}
public class MonsterAttack : IState<Monster>
{
    public Monster Instance { get; set; }
    Coroutine m_attackCor;

    public virtual void OnEnter(Monster instance)
    {
        Instance = instance;
        m_attackCor = Instance.StartCoroutine(M_Attack());
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {
        Instance.StopCoroutine(m_attackCor);
    }
    protected IEnumerator M_Attack()
    {
        if (Instance.target != null)
        {
            Player.Instance.P_Hit(Instance.M_Damage);
        }
        yield return new WaitForSeconds(1f);
        Instance.SetState(new MonsterIdle());
    }
}
#endregion
