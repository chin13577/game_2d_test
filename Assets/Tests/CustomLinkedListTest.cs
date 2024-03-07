using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CustomLinkedListTest : MonoBehaviour
{
    private class TestHero
    {
        public string id;
        public TestHero(string id)
        {
            this.id = id;
        }
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestFind()
    {
        CustomLinkedList<TestHero> heroList = new CustomLinkedList<TestHero>();
        heroList.AddLast(new TestHero("A"));
        heroList.AddLast(new TestHero("B"));
        TestHero cHero = new TestHero("C");
        heroList.AddLast(cHero);
        heroList.AddLast(new TestHero("D"));

        TestHero hero = heroList.Find(obj => obj.id == "C");

        Assert.AreEqual(cHero, hero);
    }

    [Test]
    public void TestRemove()
    {
        CustomLinkedList<TestHero> heroList = new CustomLinkedList<TestHero>();
        heroList.AddLast(new TestHero("A"));
        heroList.AddLast(new TestHero("B"));
        TestHero cHero = new TestHero("C");
        heroList.AddLast(cHero);
        heroList.AddLast(new TestHero("D"));
        heroList.AddLast(new TestHero("E"));

        heroList.Remove(cHero);

        TestHero hero = heroList.Find(obj => obj.id == "C");
        Assert.AreEqual(null, hero);

        Assert.AreEqual(4, heroList.Count);
    }

    [Test]
    public void TestRemoveWithPredicate()
    {
        CustomLinkedList<TestHero> heroList = new CustomLinkedList<TestHero>();
        heroList.AddLast(new TestHero("A"));
        heroList.AddLast(new TestHero("B"));
        TestHero cHero = new TestHero("C");
        heroList.AddLast(cHero);
        heroList.AddLast(new TestHero("D"));
        heroList.AddLast(new TestHero("E"));

        TestHero removeResult = heroList.Remove(hero => hero.id == "C");

        foreach (var item in heroList)
        {
            Debug.Log(item.id);
        }

        TestHero hero = heroList.Find(obj => obj.id == "C");

        Assert.AreEqual(null, hero);
        Assert.AreEqual(cHero, removeResult);
        Assert.AreEqual(4, heroList.Count);
    }

    [Test]
    public void TestFirstToLast()
    {
        CustomLinkedList<TestHero> heroList = new CustomLinkedList<TestHero>();
        heroList.AddLast(new TestHero("A"));
        heroList.AddLast(new TestHero("B"));
        TestHero cHero = new TestHero("C");
        heroList.AddLast(cHero);
        heroList.AddLast(new TestHero("D"));
        heroList.AddLast(new TestHero("E"));

        heroList.FirstToLast();

        foreach (var item in heroList)
        {
            Debug.Log(item.id);
        }
        Assert.AreEqual(heroList.First.Value.id, "B");
        Assert.AreEqual(heroList.Last.Value.id, "A");
    }
    //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //// `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator NewTestScriptWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
