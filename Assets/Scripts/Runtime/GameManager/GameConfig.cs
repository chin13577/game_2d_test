using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SpawnWeightData
{
    public int SpawnAmount;
    public int Weight;
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
public class GameConfig : ScriptableObject
{
    public int InitSpawnHeroCount = 5;
    public int InitSpawnMonsterCount = 5;

    public float ExpMultiplier { get => Mathf.Clamp(_expMultiplier, 0.1f, float.MaxValue); }
    [SerializeField] private float _expMultiplier = 0.5f;

    public int SpawnHeroTurn { get => Mathf.Clamp(_spawnHeroTurn, 1, int.MaxValue); }
    [SerializeField] private int _spawnHeroTurn = 1;
    public int SpawnEnemyTurn { get => Mathf.Clamp(_spawnEnemyTurn, 1, int.MaxValue); }
    [SerializeField] private int _spawnEnemyTurn = 1;

    public List<SpawnWeightData> SpawnOnMonsterDeadWeightList = new List<SpawnWeightData>();
    public List<SpawnWeightData> SpawnOnHeroJoinWeightList = new List<SpawnWeightData>();

    [Header("Config Spawn Character by Turn")]
    public List<SpawnWeightData> SpawnMonsterWeightList = new List<SpawnWeightData>();
    public List<SpawnWeightData> SpawnHeroWeightList = new List<SpawnWeightData>();

    public float SpawnMonsterLevelMultiplier = 0.8f;
    public float SpawnHeroLevelMultiplier = 0.8f;

    public int MaxLevel = 99;

    [Tooltip("Status Formula. 'x' represent to level")]
    public StatusFormula heroStatusFormula;
    [Tooltip("Status Formula. 'x' represent to level")]
    public StatusFormula enemyStatusFormula;
}
