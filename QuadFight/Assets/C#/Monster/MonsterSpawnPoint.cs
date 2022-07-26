using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    public int enemycont;
    [SerializeField] Transform[] map;
    [SerializeField] GameObject monsterPrefab;

    [SerializeField] float spawnTime = 10f;
    [SerializeField] float curTime = 0;


    [SerializeField] string poolKey = "Monster";

    private void Update()
    {
        M_SpawnPoint();
    }

    private void M_SpawnPoint()
    {
        if (curTime >= spawnTime && enemycont < 12)
        {
            Monster m_bullet = MonsterPoolManager.Instance.GetMonster(poolKey, monsterPrefab);
            int randomspawn = Random.Range(0, map.Length);
            m_bullet.transform.position = map[randomspawn].transform.position;
            enemycont++;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }
}
