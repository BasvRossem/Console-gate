using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandlerTests
{
    private GameObject go;
    private Keylistener k;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject();
        k = go.AddComponent(typeof(Keylistener)) as Keylistener;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    private void keyEvent(List<KeyCode> args)
    {
        Debug.Log(args);
    }

    [Test]
    public void AddKeyLambda()
    {
        Assert.IsTrue(k.addKey(new List<KeyCode> { KeyCode.A }, (args) => { Debug.Log("Printed"); }));
    }


    [Test]
    public void AddKeyMethod()
    {
        Assert.IsTrue(k.addKey(new List<KeyCode> { KeyCode.A }, keyEvent));
    }



    [Test]
    public void AddKeyEmptyList()
    {
        Assert.IsFalse(k.addKey(new List<KeyCode> { }, keyEvent));
    }

    [Test]
    public void AddKeyEmptyCallback()
    {
        Assert.IsFalse(k.addKey(new List<KeyCode> { }, null));
    }



    [Test]
    public void AddOptionLambda()
    {
        Assert.IsTrue(k.addOption(KeyBoardOptions.Alphabetical, (args) => { Debug.Log("Printed"); }));
    }

    [Test]
    public void AddOptionMethod()
    {
        Assert.IsTrue(k.addOption(KeyBoardOptions.Alphabetical, keyEvent));
    }

    public void AddOptionEmptyCallback()
    {
        Assert.IsTrue(k.addOption(KeyBoardOptions.Alphabetical, null));

    }

}
