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

    //private int S_enemycont;
    //private int Bs_enemycont;
    //private int Bone_enemycont;

    [SerializeField] Transform[] map;
    [SerializeField] GameObject[] monsterPrefab;

    //[SerializeField] float spawnTime = 10f;
    //[SerializeField] float curTime = 0;

    private float stageLimitTime;

    public List<StageInfo> stageInfoList;
    #region Component
    public static StageManager Instance;
    #endregion
    #endregion
    private void Awake()
    {
        Instance = GetComponent<StageManager>();
    }

    #region 몹스폰 관리
    private void Start()
    {
        if (wstage != WStage.main)
        {
            StartCoroutine(Mob_Spawn(stageInfoList[(int)wstage - 1]));
        }
    }
    IEnumerator Mob_Spawn(StageInfo info)
    {
        stageLimitTime = info.LimitTime;
        for (int i = 0; i < info.spawnDataList.Count; i++)
        {
            yield return new WaitForSeconds(info.spawnDataList[i].spawnDelay);
            var obj = Instantiate(monsterPrefab[(int)info.spawnDataList[i].mobType]);
            obj.transform.position = map[Random.Range(0, map.Length)].position;
        }
    }

    //private void SlimeSpawn()
    //{
    //    if (curTime >= spawnTime && S_enemycont < 10)
    //    {
    //        GameObject slime = monsterPrefab[0];
    //        int randomspawn = Random.Range(0, map.Length);
    //        slime.transform.position = map[randomspawn].transform.position;
    //        Instantiate(slime);
    //        S_enemycont++;
    //        curTime = 0;
    //    }
    //    curTime += Time.deltaTime;
    //}
    //private void BSlimeSpawn()
    //{
    //    if (curTime >= spawnTime && Bs_enemycont < 3)
    //    {
    //        GameObject bslime = monsterPrefab[1];
    //        int randomspawn = Random.Range(0, map.Length);
    //        bslime.transform.position = map[randomspawn].transform.position;
    //        Instantiate(bslime);
    //        Bs_enemycont++;
    //        curTime = 0;
    //    }
    //    curTime += Time.deltaTime;
    //}

    //private void BoneKnightSpawn()
    //{
    //    if (curTime >= spawnTime && Bone_enemycont < 3)
    //    {
    //        GameObject boneknight = monsterPrefab[2];
    //        int randomspawn = Random.Range(0, map.Length);
    //        boneknight.transform.position = map[randomspawn].transform.position;
    //        Instantiate(boneknight);
    //        Bone_enemycont++;
    //        curTime = 0;
    //    }
    //    curTime += Time.deltaTime;
    //}
    //#endregion



    //private void Update()
    //{
    //    StageManagemant();
    //}

    //#region 스테이지 관리

    //public void StageManagemant()
    //{
    //    if (wstage == WStage.stage1)
    //    {
    //        if (!GameManager.Instance.isClear || !GameManager.Instance.isGameOver)
    //        {
    //            SlimeSpawn();
    //            BSlimeSpawn();
    //        }
    //    }

    //    if (wstage == WStage.stage2)
    //    {
    //        if (!GameManager.Instance.isClear || !GameManager.Instance.isGameOver)
    //        {
    //            SlimeSpawn();
    //            BSlimeSpawn();
    //            BoneKnightSpawn();
    //        }
    //    }

    //    if (wstage == WStage.stage3)
    //    {
    //        if (!GameManager.Instance.isClear || !GameManager.Instance.isGameOver)
    //        {
    //            SlimeSpawn();
    //            BSlimeSpawn();
    //        }
    //    }
    //}
    #endregion
}
