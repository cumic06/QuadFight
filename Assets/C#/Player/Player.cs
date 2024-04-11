using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoFSM<Player>
{
    #region ����
    [Header("ü��")]
    [SerializeField] int maxHp;
    public int MaxHp => maxHp;
    [SerializeField] float hp;
    public float Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            if (Hp <= 0)
            {
                SetState(new PlayerDead());
            }
        }
    }

    [Header("�̵��ӵ�")]
    [SerializeField] float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [Header("����ӵ�")]
    [SerializeField] float guardSpeed;
    public float GuardSpeed => guardSpeed;

    [Header("����(�������� ������ ���Թ���)")]
    [SerializeField] float guardPower = 1;
    public float GuardPower
    {
        get => guardPower;
        set => guardPower = value;
    }

    [Header("���ݷ�")]
    [SerializeField] int attackDamage;
    public int AttackDamage => attackDamage;

    [Header("���ݼӵ�(�������� ����)")]
    [SerializeField] float attackSpeed = 1;
    public float AttackSpeed
    {
        get => attackSpeed;
        set
        {
            attackSpeed = Mathf.Clamp(value, 1, 5);
            Anim.SetFloat("AttackSpeed", value);
        }
    }

    private Vector2 moveDir;
    public Vector2 MoveDir => moveDir;

    private int attackDir;
    public int AttackDir => attackDir;

    [HideInInspector] public bool PlayerDeath;
    #endregion

    #region Component
    private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    private Animator anim;
    public Animator Anim => anim;

    public static Player Instance;
    #endregion

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Instance = GetComponent<Player>();
        AttackSpeed = attackSpeed;
    }
    private void Start()
    {
        SetState(new PlayerIdle());
    }

    protected override void Update()
    {
        InputDir();
        base.Update();
    }

    public void InputDir()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveDir.x != 0)
        {
            attackDir = moveDir.x > 0 ? 1 : -1;
        }
    }
    public void P_Hit(int damage)
    {
        Hp -= damage * GuardPower;

        GameManager.Instance.SetHpText(Hp.ToString());

        SetState(new PlayerHit());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(attackDir * 0.3f, 1), new Vector3(3f, 3));
    }
}

#region FSM ���ѻ��� �ӽ�
public class PlayerIdle : IState<Player>
{
    public Player Instance { get; set; }

    public void OnEnter(Player player)
    {
        Instance = player;
    }
    public void OnExit()
    {

    }
    public void OnUpdate()
    {
        if (Instance.MoveDir != Vector2.zero)
        {
            Instance.SetState(new PlayerMove());
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Instance.SetState(new PlayerAttack());
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Instance.SetState(new PlayerGuard());
        }
    }
}
public class PlayerMove : IState<Player>
{
    public Player Instance { get; set; }
    public void OnEnter(Player player)
    {
        Instance = player;
        Instance.Anim.SetBool("isRun", true);
    }
    public void OnExit()
    {
        Instance.Anim.SetBool("isRun", false);
    }
    public void OnUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Instance.SetState(new PlayerAttack());
        }
        if (Instance.MoveDir == Vector2.zero)
        {
            Instance.SetState(new PlayerIdle());
        }

        //�̵�
        Instance.transform.position += (Vector3)Instance.MoveDir * Instance.MoveSpeed * Time.deltaTime;
        if (Instance.MoveDir.x != 0)
        {
            Instance.Sprite.flipX = Instance.MoveDir.x < 0;
        }
    }
}
public class PlayerAttack : IState<Player>
{
    Coroutine attackCor;
    public Player Instance { get; set; }
    public void OnEnter(Player player)
    {
        Instance = player;
        attackCor = Instance.StartCoroutine(P_Attack());
    }
    public void OnExit()
    {
        Instance.Anim.SetBool("isAttack", false);
        Instance.StopCoroutine(attackCor);
    }
    public void OnUpdate()
    {

    }
    IEnumerator P_Attack()
    {
        int attackDir = Instance.AttackDir;

        Instance.Anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.6f / Instance.AttackSpeed);
        Collider2D[] hits = Physics2D.OverlapBoxAll(Instance.transform.position + new Vector3(attackDir, 0), new Vector2(1.5f, 2), 0, LayerMask.GetMask("Monster"));
        foreach (var hit in hits)
        {
            hit.GetComponent<Monster>().Monster_Hit(Instance.AttackDamage);
        }
        Instance.Anim.SetBool("isAttack", false);
        Instance.SetState(new PlayerIdle());
    }
}
public class PlayerGuard : IState<Player>
{
    Coroutine guardCor;
    public Player Instance { get; set; }
    public void OnEnter(Player player)
    {
        Instance = player;
        guardCor = Instance.StartCoroutine(P_Guard());
    }
    public void OnExit()
    {
        Instance.GuardPower = 1;
        Instance.Anim.SetBool("isGuard", false);
        Instance.StopCoroutine(guardCor);
    }
    public void OnUpdate()
    {

    }

    IEnumerator P_Guard()
    {
        Instance.Anim.SetBool("isGuard", true);
        Instance.GuardPower = 0.8f;
        yield return new WaitForSeconds(1f);
        Instance.Anim.SetBool("isGuard", false);
        Instance.GuardPower = 1;
        Instance.SetState(new PlayerIdle());
    }

}
public class PlayerHit : IState<Player>
{
    public Player Instance { get; set; }
    public void OnEnter(Player player)
    {
        Instance = player;
        Instance.StartCoroutine(P_Hit());
        Instance.StartCoroutine(P_changeColor());
    }
    public void OnExit()
    {
        Debug.Log("�÷��̾� ü��" + Instance.Hp);
        Instance.Anim.SetBool("isHit", false);
    }
    public void OnUpdate()
    {

    }

    IEnumerator P_Hit()
    {
        Instance.Anim.SetBool("isHit", true);
        yield return new WaitForSeconds(0.3f);
        Instance.Anim.SetBool("isHit", false);
        Instance.SetState(new PlayerIdle());
    }
    IEnumerator P_changeColor()
    {
        Instance.Sprite.color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(0.1f);
        Instance.Sprite.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
    }
}
public class PlayerDead : IState<Player>
{
    public Player Instance { get; set; }
    public void OnEnter(Player player)
    {
        Instance = player;
        GameManager.Instance.isGameOver = true;
    }
    public void OnExit()
    {

    }
    public void OnUpdate()
    {

    }

}
#endregion