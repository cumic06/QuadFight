using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IState<T> where T : MonoBehaviour
{
    void OnEnter(T instance);
    void Update();
    void OnExit();
}
public class PlayerState : IState<Player>
{
    protected Player player;

    public bool isIdle = false;
    public bool isMove = false;
    public bool isAttack = false;
    public bool isGuard = false;
    public bool isGuardMove = false;

    public virtual void OnEnter(Player player)
    {
        if (this.player == null)
        {
            this.player = player;
        }

    }
    public virtual void Update()
    {

    }
    public virtual void OnExit()
    {

    }
}
public class PlayerIdle : PlayerState
{
    public override void OnEnter(Player player)
    {
        isIdle = true;
        base.OnEnter(player);
    }
    public override void Update()
    {
        MoveState();
    }
    private void MoveState()
    {
        if (InputSystem.Instance.MoveDir != Vector2.zero)
        {
            player.SetState<PlayerMove>(nameof(PlayerMove));
        }
        isIdle = false;
    }
}

public class PlayerMove : PlayerState
{
    public override void OnEnter(Player player)
    {
        isMove = true;
        base.OnEnter(player);
    }
    public override void Update()
    {
        if (!isGuard && !isGuardMove)
        {
            Move();
        }
    }
    public void Move()
    {
        player.Anim.SetBool("isRun", true);

        player.transform.Translate(new Vector2(InputSystem.Instance.MoveDir.x, InputSystem.Instance.MoveDir.y) * player.MoveSpeed * Time.deltaTime);

        if (InputSystem.Instance.MoveDir == Vector2.zero)
        {
            isMove = false;
            player.SetState<PlayerIdle>(nameof(PlayerIdle));
        }

        if (InputSystem.Instance.MoveDir.x != 0)
        {
            player.Sprtie.flipX = InputSystem.Instance.MoveDir.x < 0;
        }

    }
    public override void OnExit()
    {
        player.Anim.SetBool("isRun", false);
        player.transform.Translate(Vector2.zero);
    }
}

public class PlayerAttack : PlayerState
{
    Coroutine attackcor;
    public override void OnEnter(Player player)
    {
        isAttack = true;
        base.OnEnter(player);

        if (!isGuard && !isGuardMove)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (attackcor != null)
        {
            player.StopCoroutine(attackcor);
        }
        attackcor = player.StartCoroutine(P_Attack());
    }

    IEnumerator P_Attack()
    {
        player.Anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        player.Anim.SetBool("isAttack", false);
        isAttack = false;
        player.SetState<PlayerIdle>(nameof(PlayerIdle));
    }
}
public class PlayerGuard : PlayerState
{
    Coroutine guardcor;
    public override void OnEnter(Player player)
    {
        isGuard = true;
        base.OnEnter(player);

        if (!isAttack)
        {
            Guard();
        }
    }
    public override void Update()
    {
        GuardMoveState();
    }
    private void Guard()
    {
        if (guardcor != null)
        {
            player.StopCoroutine(guardcor);
        }
        guardcor = player.StartCoroutine(P_Guard());
    }
    private void GuardMoveState()
    {
        if (!isAttack && !isMove)
        {
            if (InputSystem.Instance.MoveDir != Vector2.zero)
            {
                player.SetState<PlayerGuardMove>(nameof(PlayerGuardMove));
            }
        }
    }

    IEnumerator P_Guard()
    {
        player.Anim.SetBool("isGuard", true);
        yield return new WaitForSeconds(2f);
        player.Anim.SetBool("isGuard", false);
        player.SetState<PlayerIdle>(nameof(PlayerIdle));
        isGuard = false;
    }
}
public class PlayerGuardMove : PlayerGuard
{
    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
    }
    public override void Update()
    {
        GuardMove();
    }
    private void GuardMove()
    {
        if (!isMove)
        {
            player.transform.Translate(new Vector2(InputSystem.Instance.MoveDir.x, InputSystem.Instance.MoveDir.y) * player.GuardSpeed * Time.deltaTime);
        }

        if (InputSystem.Instance.MoveDir.x != 0)
        {
            player.Sprtie.flipX = InputSystem.Instance.MoveDir.x < 0;
        }

    }
}