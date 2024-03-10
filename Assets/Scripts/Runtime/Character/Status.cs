using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class StatusFormula
{
    public string AttackFormula = "2*x+5";
    public string MaxHPFormula = "2.5*x+15";
    public string MaxEXPFormula = "5*x+10";
}
public class Status
{
    private int _baseAtk;
    private int _baseMaxHP;
    private int hp;

    public int BonusAtk { get => _bonusAtk; }
    private int _bonusAtk;
    public int BonusMaxHP { get => _bonusMaxHp; }
    private int _bonusMaxHp;

    public int TotalAtk { get => _baseAtk + _bonusAtk; }

    public int TotalMaxHP { get => _baseMaxHP + _bonusMaxHp; }
    public int HP { get => hp; }

    private int _exp;
    public int EXP
    {
        get
        {
            return _exp;
        }
        set
        {
            if (value >= MaxEXP)
            {
                int expInput = value;
                while (expInput - MaxEXP >= 0)
                {
                    expInput -= MaxEXP;
                    Level += 1;
                }
                _exp = expInput;
            }
            else
            {
                _exp = value;
            }
        }
    }
    public int MaxEXP;

    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            CalculateStatusByLevel(value);
            if (value > _level)
            {
                // level up
                hp = _bonusMaxHp;
            }
            _level = value;
        }
    }


    private int _level = 1;
    private StatusFormula _formula;

    public Status() : this(new StatusFormula())
    {
    }

    public Status(StatusFormula formula)
    {
        this._formula = formula;
        _baseAtk = UnityEngine.Random.Range(1, 10);
        _baseMaxHP = UnityEngine.Random.Range(1, 10);

        Level = 1;
        ResetHP();
    }

    public void ResetHP()
    {
        hp = TotalMaxHP;
    }

    private void CalculateStatusByLevel(int level)
    {
        this._bonusAtk = Evaluate(_formula.AttackFormula, level);
        this._bonusMaxHp = Evaluate(_formula.MaxHPFormula, level);
        this.MaxEXP = Evaluate(_formula.MaxEXPFormula, level);
    }
    private int Evaluate(string formula, int level)
    {
        DataTable table = new DataTable();

        try
        {
            formula = formula.Replace("x", level.ToString());
            var result = table.Compute(formula, "");

            return Convert.ToInt32(result);
        }
        catch (Exception error)
        {
            Debug.Log(error.Message);
            return 1;
        }
    }
}
