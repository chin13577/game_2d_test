using FS;
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

    private int _turn = 0;

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
