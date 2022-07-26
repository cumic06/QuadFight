using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    public static MonsterPoolManager Instance { get; private set; }

    Dictionary<string, Queue<Monster>> monsterPool = new Dictionary<string, Queue<Monster>>();

    public Monster GetMonster(string key, GameObject monsterPrefab)
    {
        Monster m_bullet = null;

        if (monsterPool.ContainsKey(key))
        {
            if (monsterPool[key].Count > 0)
            {
                m_bullet = monsterPool[key].Dequeue();
                m_bullet.gameObject.SetActive(true);
            }
            else
            {
                m_bullet = Instantiate(monsterPrefab).GetComponent<Monster>();
                //m_bullet.key = key;
                m_bullet.transform.SetParent(transform);
            }
        }
        else
        {
            monsterPool.Add(key, new Queue<Monster>());

            m_bullet = Instantiate(monsterPrefab).GetComponent<Monster>();
            //m_bullet.key = key;
            m_bullet.transform.SetParent(transform);
        }
        return m_bullet;
    }
    public void ReturnMonster(string key, Monster m_bullet)
    {
        monsterPool[key].Enqueue(m_bullet);
        m_bullet.gameObject.SetActive(false);
    }
    private void Awake()
    {
        Instance = this;
    }
}
