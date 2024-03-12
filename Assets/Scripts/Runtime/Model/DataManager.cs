using FS;
using FS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static event Action<int> OnUpdateTurn;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DataManager();
            return _instance;
        }
    }

    private static DataManager _instance;

    public Monster CurrentEnemy { get => _currentEnemy; }
    private Monster _currentEnemy;

    public GameConfig Config { get; private set; }
    public RandomWeight<int> HeroSpawnByTurnWeight { get; private set; }
    public RandomWeight<int> EnemySpawnByTurnWeight { get; private set; }


    public RandomWeight<int> SpawnOnHeroJoinWeight { get; private set; }
    public RandomWeight<int> SpawnOnEnemyDeadWeight { get; private set; }

    private int _turn = 0;

    public void SetConfig(GameConfig config)
    {
        this.Config = config;
        HeroSpawnByTurnWeight = GenerateSpawnRandomWeight(config.SpawnHeroWeightList);
        EnemySpawnByTurnWeight = GenerateSpawnRandomWeight(config.SpawnMonsterWeightList);

        SpawnOnEnemyDeadWeight = GenerateSpawnRandomWeight(config.SpawnOnMonsterDeadWeightList);
        SpawnOnHeroJoinWeight = GenerateSpawnRandomWeight(config.SpawnOnHeroJoinWeightList);
    }

    private RandomWeight<int> GenerateSpawnRandomWeight(List<SpawnWeightData> weightList)
    {
        RandomWeight<int> spawnWeight = new RandomWeight<int>();
        for (int i = 0; i < weightList.Count; i++)
        {
            SpawnWeightData spawnWeightData = weightList[i];
            spawnWeight.AddItem(spawnWeightData.SpawnAmount, spawnWeightData.Weight);
        }
        return spawnWeight;
    }

    public void SetCurrentBattleEnemy(Monster enemy)
    {
        this._currentEnemy = enemy;
    }

    public void UpdateTurn()
    {
        this._turn += 1;
        OnUpdateTurn?.Invoke(this._turn);
    }
    public void SetTurn(int turn)
    {
        this._turn = turn;
    }

    public int GetTurn()
    {
        return _turn;
    }
}
