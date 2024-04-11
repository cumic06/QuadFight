using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MobType
{
    slime,
    bigSlime,
    skeleton,
    bronzeKnight,
    sliverKnight,
    goldKnight,
    baal,
    mamon
}

[CreateAssetMenu(fileName = "StageInfo", menuName = "StageInfo")]
public class StageInfo : ScriptableObject
{
    public List<SpawnData> spawnDataList;
    public float LimitTime;
}

[Serializable]
public class SpawnData
{
    public float spawnDelay;
    public MobType mobType;
}

