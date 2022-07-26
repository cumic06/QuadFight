using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    #region Variable
    private MonsterState currentState;

    public MonsterState CurrentState => currentState;

    private Dictionary<string, MonsterState> monsterStateDict = new Dictionary<string, MonsterState>();
    #endregion

    private SpriteRenderer sprite;

    private CircleCollider2D col;

    private Animator anim;
    public Animator Anim => anim;

    public Transform player;

    public float M_speed;

    public string key;

    public void SetState<T>(string key) where T : MonsterState, new()
    {
        if (!monsterStateDict.ContainsKey(key))
        {
            monsterStateDict.Add(key, new T());
        }

        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = monsterStateDict[key];
        currentState.OnEnter(this);
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        transform.position += Vector3.forward * Time.deltaTime * 20;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        StartCoroutine(M_Move());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Monster_Hit();
        }
    }

    public virtual void Monster_Hit()
    {
        StartCoroutine(nameof(changeColor));
    }

    public virtual void Monster_Dead()
    {
        MonsterPoolManager.Instance.ReturnMonster(key, this);
    }

    IEnumerator changeColor()
    {
        for (float t = 0; t < 3; t += Time.deltaTime)
        {
            sprite.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.3f);
            sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator M_Move()
    {
        for (float t = 0; t < 3; t += Time.deltaTime)
        {
            transform.position += Vector3.zero * Time.deltaTime;
            yield return null;
        }
    }

}
