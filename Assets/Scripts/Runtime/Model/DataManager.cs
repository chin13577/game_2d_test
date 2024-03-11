using FS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
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

    public void SetCurrentBattleEnemy(Monster enemy)
    {
        this._currentEnemy = enemy;
    }
}
