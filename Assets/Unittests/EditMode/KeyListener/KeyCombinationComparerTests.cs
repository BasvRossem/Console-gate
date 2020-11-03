using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UserInput;

namespace Tests
{
    public class KeyCombinationComparerTests
    {
        private KeyCodeCombinationComparer _kc;

        [SetUp]
        public void SetUp()
        {
            _kc = new KeyCodeCombinationComparer();
        }

        [TearDown]
        public void TearDown()
        {
        }
        
        [Test]
        public void SameObjects()
        {
            Tuple<List<KeyCode>, KeyCode> l1 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A }, KeyCode.B);
            Assert.IsTrue(_kc.Equals(l1, l1));
        }
        

        [Test]
        public void SameSingleContents()
        {    
            Tuple<List<KeyCode>, KeyCode> l1 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A }, KeyCode.B);
            Tuple<List<KeyCode>, KeyCode> l2 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A }, KeyCode.B);
            Assert.IsTrue(_kc.Equals(l1, l2));
        }

        [Test]
        public void SameMultiContents()
        {
            Tuple<List<KeyCode>, KeyCode> l1 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A, KeyCode.B, KeyCode.C }, KeyCode.D);
            Tuple<List<KeyCode>, KeyCode> l2 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A, KeyCode.B, KeyCode.C }, KeyCode.D);
            Assert.IsTrue(_kc.Equals(l1, l2));
        }


        [Test]
        public void EmptyLists()
        {
            Tuple<List<KeyCode>, KeyCode> l1 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { }, KeyCode.A);
            Tuple<List<KeyCode>, KeyCode> l2 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { }, KeyCode.A);
            Assert.IsTrue(_kc.Equals(l1, l2));
        }
        
        
        [Test]
        public void NullLists()
        {
            Tuple<List<KeyCode>, KeyCode> l1 = null;
            Tuple<List<KeyCode>, KeyCode> l2 = null;
            Assert.IsFalse(_kc.Equals(l1, l2));
        }


        [Test]
        public void DifferentLists()
        {
            Tuple<List<KeyCode>, KeyCode> l1 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A, KeyCode.B }, KeyCode.C);
            Tuple<List<KeyCode>, KeyCode> l2 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.D, KeyCode.E }, KeyCode.F);
            Assert.IsFalse(_kc.Equals(l1, l2));
        }


        [Test]
        public void SubsetLists()
        {
            Tuple<List<KeyCode>, KeyCode> l1 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.A, KeyCode.B }, KeyCode.C);
            Tuple<List<KeyCode>, KeyCode> l2 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.D }, KeyCode.F);
            l2.Item1.AddRange(l1.Item1);
            Assert.IsFalse(_kc.Equals(l1, l2));
        }


        [Test]
        public void CrossJoinLists()
        {
            List<KeyCode> l1 = new List<KeyCode> { KeyCode.A, KeyCode.B };
            Tuple<List<KeyCode>, KeyCode> l2 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.D }, KeyCode.E);
            Tuple<List<KeyCode>, KeyCode> l3 = new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.F }, KeyCode.G);
            l2.Item1.AddRange(l1);
            l3.Item1.AddRange(l1);
            Assert.IsFalse(_kc.Equals(l2, l3));
        }
    }
}