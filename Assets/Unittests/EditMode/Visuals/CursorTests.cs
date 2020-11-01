using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Cursor = Visuals.Cursor;

namespace VisualsTests
{
    public class CursorTests
    {
        private Cursor _cursor;

        [SetUp]
        public void Setup()
        {
            _cursor = new Cursor();
        }

        [Test]
        public void CheckDirections()
        {
            Assert.AreEqual(new Vector2Int(-1, 0), Cursor.Left);
            Assert.AreEqual(new Vector2Int(1, 0), Cursor.Right);
            Assert.AreEqual(new Vector2Int(0, -1), Cursor.Up);
            Assert.AreEqual(new Vector2Int(0, 1), Cursor.Down);
        }

        [Test]
        public void SetBoundsGetBounds()
        {
            _cursor.SetBounds(5, 6, 10, 11);

            Assert.AreEqual(new List<Vector2Int>() { new Vector2Int(5, 6), new Vector2Int(10, 11) }, _cursor.GetBounds());

            _cursor.SetPosition(5, 5);
            _cursor.SetBounds(10, 10, 15, 15);
            Assert.AreEqual(new Vector2Int(10, 10), _cursor.GetPosition());

            _cursor.SetPosition(12, 12);
            Assert.AreEqual(new Vector2Int(12, 12), _cursor.GetPosition());
        }

        [Test]
        public void SetPositionGetPosition()
        {
            _cursor.SetPosition(5, 5);
            Assert.AreEqual(new Vector2Int(5, 5), _cursor.GetPosition());

            _cursor.SetPosition(10, 10);
            Assert.AreEqual(new Vector2Int(10, 10), _cursor.GetPosition());
        }

        [Test]
        public void Move()
        {
            _cursor.Move(Cursor.Right);
            Assert.AreEqual(new Vector2Int(1, 0), _cursor.GetPosition());
            _cursor.Move(Cursor.Left);
            Assert.AreEqual(new Vector2Int(0, 0), _cursor.GetPosition());
            _cursor.Move(Cursor.Down);
            Assert.AreEqual(new Vector2Int(0, 1), _cursor.GetPosition());
            _cursor.Move(Cursor.Up);
            Assert.AreEqual(new Vector2Int(0, 0), _cursor.GetPosition());
        }

        [Test]
        public void MoveWithinBounds()
        {
            _cursor.SetBounds(5, 5, 10, 10);
            _cursor.ResetPosition();
            Assert.AreEqual(5, _cursor.x);
            Assert.AreEqual(5, _cursor.y);

            _cursor.Move(Cursor.Left);
            Assert.AreEqual(5, _cursor.x);
            Assert.AreEqual(5, _cursor.y);

            for (int moves = 0; moves < 50; moves++)
            {
                _cursor.Move(Cursor.Right);
                _cursor.Move(Cursor.Down);
            }

            Assert.AreEqual(10, _cursor.x);
            Assert.AreEqual(10, _cursor.y);
        }

        [Test]
        public void ResetPosition()
        {
            _cursor.SetPosition(1, 10);
            _cursor.ResetPosition();
            Assert.AreEqual(new Vector2Int(0, 0), _cursor.GetPosition());
        }
    }
}