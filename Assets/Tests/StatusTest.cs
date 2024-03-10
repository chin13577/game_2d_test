using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StatusTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestStatusFormula()
    {
        // Use the Assert class to test conditions

        StatusFormula formula = new StatusFormula();
        formula.AttackFormula = "x + 5";
        Status status = new Status(formula);
        Assert.AreEqual(6, status.BonusAtk);

        status.Level = 5;
        Assert.AreEqual(10, status.BonusAtk);
    }

    [Test]
    public void TestEXPFormula()
    {
        StatusFormula formula = new StatusFormula();
        formula.MaxEXPFormula = "x";
        Status status = new Status(formula);
        status.EXP += 1;
        Assert.AreEqual(0, status.EXP);
        Assert.AreEqual(2, status.Level);

        status = new Status(formula);
        status.EXP += 5;

        Assert.AreEqual(2, status.EXP);
        Assert.AreEqual(3, status.Level);
    }

}
