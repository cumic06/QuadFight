using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoFSM<Player>
{
    #region 변수
    [Header("체력")]
    [SerializeField] int maxHp;
    public int MaxHp => maxHp;
    [SerializeField] int hp;
    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
        }
    }


    [Header("이동속도")]
    [SerializeField] float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [Header("가드속도")]
    [SerializeField] float guardSpeed = 1000f;
    public float GuardSpeed => guardSpeed;
    [Header("공격력")]
    [SerializeField] int attackDamage;
    public int AttackDamage => attackDamage;

    private Vector2 moveDir;
    public Vector2 MoveDir => moveDir;

    private int attackDir;
    public int AttackDir => attackDir;
    #endregion

    #region Component
    private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    private Animator anim;
    public Animator Anim => anim;
    #endregion

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
    void InputDir()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveDir.x != 0)
        {
            attackDir = moveDir.x > 0 ? 1 : -1;
        }
    }
    public void Hit(int damage)
    {
        SetState(new PlayerHit());
    }
}

#region FSM 유한상태 머신
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instance.SetState(new PlayerAttack());
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
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
        Instance.Anim.SetBool("isWalk", true);
    }
    public void OnExit()
    {
        Instance.Anim.SetBool("isWalk", false);
    }
    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instance.SetState(new PlayerAttack());
        }
        if (Instance.MoveDir == Vector2.zero)
        {
            Instance.SetState(new PlayerIdle());
        }

        //이동
        /*Instance.transform.position += (Vector3)Instance.MoveDir * Instance.MoveSpeed * Time.deltaTime*/
        Instance.transform.position = new Vector2(Instance.MoveDir.x * Instance.MoveSpeed ,Instance.MoveDir.y * Instance.MoveSpeed * Time.deltaTime).normalized;
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
        Instance.StartCoroutine(P_Attack());
    }
    public void OnExit()
    {
        Instance.Anim.SetBool("isAttack", false);
        Instance.StopCoroutine(P_Attack());
    }
    public void OnUpdate()
    {

    }
    IEnumerator P_Attack()
    {
        int attackDir = Instance.AttackDir;

        Instance.Anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.7f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(Instance.transform.position + new Vector3(attackDir, 0), new Vector2(1.5f, 2), 0, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            hit.GetComponent<Monster>().Monster_Hit(Instance.AttackDamage);
        }
    }
}
public class PlayerGuard : IState<Player> 
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

    }

}
public class PlayerHit : IState<Player>
{
    public Player Instance {get;set;}
    public void OnEnter(Player player)
    {
        Instance = player;
    }
    public void OnExit()
    {

    }
    public void OnUpdate()
    {

    }
}
#endregion