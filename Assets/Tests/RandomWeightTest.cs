using System.Collections;
using System.Collections.Generic;
using FS.Util;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RandomWeightTest
{
    private class Obstacle
    {
        public int width;
        public int height;

        public override string ToString()
        {
            return "width: " + width + " height: " + height;
        }
    }
    // A Test behaves as an ordinary method
    [Test]
    public void TestCalculateTotalWeight()
    {
        RandomWeight<Obstacle> randomWeight = new RandomWeight<Obstacle>();
        randomWeight.AddItem(new Obstacle { width = 1, height = 1 }, 10);
        randomWeight.AddItem(new Obstacle { width = 1, height = 2 }, 20);
        randomWeight.AddItem(new Obstacle { width = 2, height = 1 }, 20);
        randomWeight.AddItem(new Obstacle { width = 2, height = 2 }, 20);

        Assert.AreEqual(70, randomWeight.TotalWeight);
        // Use the Assert class to test conditions
    }

    [Test]
    public void TestRandomWeight()
    {
        RandomWeight<Obstacle> randomWeight = new RandomWeight<Obstacle>();
        Obstacle result = randomWeight.Random();
        Assert.AreEqual(null, result);

        randomWeight.AddItem(new Obstacle { width = 1, height = 1 }, 10);
        randomWeight.AddItem(new Obstacle { width = 1, height = 2 }, 20);
        randomWeight.AddItem(new Obstacle { width = 2, height = 1 }, 20);
        randomWeight.AddItem(new Obstacle { width = 2, height = 2 }, 20);

        result = randomWeight.Random();
        Debug.Log(result);
        Assert.AreNotEqual(null, result);
        // Use the Assert class to test conditions
    }
}
