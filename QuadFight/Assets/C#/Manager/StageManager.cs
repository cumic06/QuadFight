using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum WStage
{
    main,
    stage1,
    stage2,
    stage3,
    stage4
}
public class StageManager : MonoBehaviour
{
    #region 변수
    public WStage wstage;

    public WStage Wstage
    {
        get => wstage;
        set
        {
            switch (value)
            {
                case WStage.main:
                    break;
                case WStage.stage1:
                    break;
                case WStage.stage2:
                    break;
                case WStage.stage3:
                    break;
                case WStage.stage4:
                    break;
            }
            wstage = value;
        }
    }

    private int S_enemycont;
    private int Bs_enemycont;

    [SerializeField] Transform[] map;
    [SerializeField] GameObject[] monsterPrefab;

    [SerializeField] float spawnTime = 10f;
    [SerializeField] float curTime = 0;

    [SerializeField] string SlimeKey = "Slime";
    [SerializeField] string B_SlimeKey = "B_Slime";
    #endregion

    #region 몹스폰 관리
    private void SlimeSpawn()
    {
        if (curTime >= spawnTime && S_enemycont < 10)
        {
            Monster m_bullet = MonsterPoolManager.Instance.GetMonster(SlimeKey, monsterPrefab[0]);
            int randomspawn = Random.Range(0, map.Length);
            m_bullet.transform.position = map[randomspawn].transform.position;
            S_enemycont++;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }
    private void BSlimeSpawn()
    {
        if (curTime >= spawnTime && Bs_enemycont < 3)
        {
            Monster m_bullet = MonsterPoolManager.Instance.GetMonster(B_SlimeKey, monsterPrefab[1]);
            int randomspawn = Random.Range(0, map.Length);
            m_bullet.transform.position = map[randomspawn].transform.position;
            Bs_enemycont++;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }
    #endregion

    #region Component
    public static StageManager stageManager;
    #endregion

    private void Awake()
    {
        stageManager = GetComponent<StageManager>();
    }

    private void Update()
    {
        StageManagemant();
    }

    #region 스테이지 관리

    public void StageManagemant()
    {
        if (wstage == WStage.stage1)
        {
            if (!GameManager.gamemanager.isClear)
            {
                SlimeSpawn();
                BSlimeSpawn();
            }
        }

        if (wstage == WStage.stage2)
        {
            if (!GameManager.gamemanager.isClear)
            {
                SlimeSpawn();
                BSlimeSpawn();
            }
        }

        if (wstage == WStage.stage3)
        {
            if (!GameManager.gamemanager.isClear)
            {
                SlimeSpawn();
                BSlimeSpawn();
            }
        }
    }
    #endregion
}
