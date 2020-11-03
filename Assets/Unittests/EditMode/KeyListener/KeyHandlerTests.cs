using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UserInput;

namespace Tests
{
    public class KeyHandlerTests
    {
        private GameObject go;
        private UserInput.KeyListener k;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject();
            k = go.AddComponent(typeof(UserInput.KeyListener)) as UserInput.KeyListener;
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
            Assert.IsTrue(k.AddKey(new List<KeyCode> { KeyCode.A }, (args) => { Debug.Log("Printed"); }));
        }


        [Test]
        public void AddKeyMethod()
        {
            Assert.IsTrue(k.AddKey(new List<KeyCode> { KeyCode.A }, keyEvent));
        }



        [Test]
        public void AddKeyEmptyList()
        {
            Assert.IsFalse(k.AddKey(new List<KeyCode> { }, keyEvent));
        }

        [Test]
        public void AddKeyEmptyCallback()
        {
            Assert.IsFalse(k.AddKey(new List<KeyCode> { }, null));
        }



        [Test]
        public void AddOptionLambda()
        {
            Assert.IsTrue(k.AddOption(KeyBoardOptions.Alphabetical, (args) => { Debug.Log("Printed"); }));
        }

        [Test]
        public void AddOptionMethod()
        {
            Assert.IsTrue(k.AddOption(KeyBoardOptions.Alphabetical, keyEvent));
        }

        public void AddOptionEmptyCallback()
        {
            Assert.IsTrue(k.AddOption(KeyBoardOptions.Alphabetical, null));

        }

    }
}
