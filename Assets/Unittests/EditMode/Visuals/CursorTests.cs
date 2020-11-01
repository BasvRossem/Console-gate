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
            Assert.AreEqual(new GridPosition(0, -1), Cursor.Left);
            Assert.AreEqual(new GridPosition(0, 1), Cursor.Right);
            Assert.AreEqual(new GridPosition(-1, 0), Cursor.Up);
            Assert.AreEqual(new GridPosition(1, 0), Cursor.Down);
        }

        [Test]
        public void SetBoundsGetBounds()
        {
            _cursor.SetBounds(new GridPosition(5, 6), new GridPosition(10, 11));

            Assert.AreEqual(new List<GridPosition>() { new GridPosition(5, 6), new GridPosition(10, 11) }, _cursor.GetBounds());

            _cursor.SetPosition(new GridPosition(5, 5));
            _cursor.SetBounds(new GridPosition(10, 10), new GridPosition(15, 15));
            Assert.AreEqual(new GridPosition(10, 10), _cursor.GetPosition());

            _cursor.SetPosition(new GridPosition(12, 12));
            Assert.AreEqual(new GridPosition(12, 12), _cursor.GetPosition());
        }

        [Test]
        public void SetPositionGetPosition()
        {
            _cursor.SetPosition(new GridPosition(5, 5));
            Assert.AreEqual(new GridPosition(5, 5), _cursor.GetPosition());

            _cursor.SetPosition(new GridPosition(10, 10));
            Assert.AreEqual(new GridPosition(10, 10), _cursor.GetPosition());
        }

        [Test]
        public void Move()
        {
            _cursor.Move(Cursor.Right);
            Assert.AreEqual(new GridPosition(0, 1), _cursor.GetPosition());
            _cursor.Move(Cursor.Left);
            Assert.AreEqual(new GridPosition(0, 0), _cursor.GetPosition());
            _cursor.Move(Cursor.Down);
            Assert.AreEqual(new GridPosition(1, 0), _cursor.GetPosition());
            _cursor.Move(Cursor.Up);
            Assert.AreEqual(new GridPosition(0, 0), _cursor.GetPosition());
        }

        [Test]
        public void MoveWithinBounds()
        {
            _cursor.SetBounds(new GridPosition(5, 5), new GridPosition(10, 10));

            _cursor.ResetPosition();
            Assert.AreEqual(5, _cursor.position.column);
            Assert.AreEqual(5, _cursor.position.row);

            _cursor.Move(Cursor.Left);
            Assert.AreEqual(5, _cursor.position.column);
            Assert.AreEqual(5, _cursor.position.row);

            for (int moves = 0; moves < 50; moves++)
            {
                _cursor.Move(Cursor.Right);
                _cursor.Move(Cursor.Down);
            }

            Assert.AreEqual(10, _cursor.position.column);
            Assert.AreEqual(10, _cursor.position.row);
        }

        [Test]
        public void ResetPosition()
        {
            _cursor.SetPosition(new GridPosition(1, 10));
            _cursor.ResetPosition();
            Assert.AreEqual(new GridPosition(0, 0), _cursor.GetPosition());
        }
    }
}