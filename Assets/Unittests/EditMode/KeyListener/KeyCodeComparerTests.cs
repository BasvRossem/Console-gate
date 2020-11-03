using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UserInput;

namespace Tests
{
    public class KeyCodeComparerTests
    {
        private KeyCodeComparer _k;

        [SetUp]
        public void SetUp()
        {
            _k =  new KeyCodeComparer();
        }

        [TearDown]
        public void TearDown()
        {
        }


        [Test]
        public void SameObjects()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A };
            Assert.IsTrue(_k.Equals(l1, l1));
        }


        [Test]
        public void SameSingleContents()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A };
            List<KeyCode> l2 = new List<KeyCode> { KeyCode.A };
            Assert.IsTrue(_k.Equals(l1, l2));
        }

        [Test]
        public void SameMultiContents()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A, KeyCode.B, KeyCode.C };
            List<KeyCode> l2 = new List<KeyCode> { KeyCode.A, KeyCode.B, KeyCode.C };
            Assert.IsTrue(_k.Equals(l1, l2));
        }


        [Test]
        public void EmptyLists()
        {
            List<KeyCode> l1 = new List<KeyCode> { };
            List<KeyCode> l2 = new List<KeyCode> { };
            Assert.IsTrue(_k.Equals(l1, l2));
        }


        [Test]
        public void DifferentLists()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A };
            List<KeyCode> l2 = new List<KeyCode> { KeyCode.B };
            Assert.IsFalse(_k.Equals(l1, l2));
        }


        [Test]
        public void SubsetLists()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A, KeyCode.B };
            List<KeyCode> l2 = new List<KeyCode> { KeyCode.C};
            l2.AddRange(l1);
            Assert.IsFalse(_k.Equals(l1, l2));
        }


        [Test]
        public void CrossJoinLists()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A, KeyCode.B };
            List<KeyCode> l2 = new List<KeyCode> { KeyCode.C };
            List<KeyCode> l3 = new List<KeyCode> { KeyCode.D };
            l2.AddRange(l1);
            l3.AddRange(l1);
            Assert.IsFalse(_k.Equals(l2, l3));
        }

    }
}