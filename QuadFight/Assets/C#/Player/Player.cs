using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Variable
    private PlayerState currentState;
    public PlayerState CurrentState => currentState;
    
    [SerializeField] float moveSpeed = 1500f;

    [SerializeField] float guardSpeed = 1000f;

    public float GuardSpeed => guardSpeed;
    public float MoveSpeed => moveSpeed;

    private Dictionary<string, PlayerState> playerStateDict = new Dictionary<string, PlayerState>();
    #endregion

    private SpriteRenderer sprite;
    public SpriteRenderer Sprtie => sprite;

    private Animator anim;
    public Animator Anim => anim;


    public void SetState<T>(string key) where T : PlayerState, new()
    {
        if (!playerStateDict.ContainsKey(key))
        {
            Debug.Log($"Added {key}");
            playerStateDict.Add(key, new T());
        }
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = playerStateDict[key];
        currentState.OnEnter(this);
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        SetState<PlayerIdle>(nameof(PlayerIdle));
    }
    private void Start()
    {
        InputSystem.Instance.attackEvent += () => SetState<PlayerAttack>(nameof(PlayerAttack));
        InputSystem.Instance.guardEvent += () => SetState<PlayerGuard>(nameof(PlayerGuard));
        InputSystem.Instance.guardmoveEvent += () => SetState<PlayerGuardMove>(nameof(PlayerGuardMove));
    }
    private void Update()
    {
        currentState.Update();
    }
}
