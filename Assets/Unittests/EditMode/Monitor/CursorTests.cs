using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Visuals;
using MonitorCursor = Visuals.MonitorCursor;

namespace Tests
{
    public class CursorTests
    {
        private string CursorName = "Newname";

        private Visuals.MonitorCursor CreateTestCursor()
        {
            return new MonitorCursor(0, 0, CursorName);
        }

        // Varable checking
        [Test]
        public void CheckDirections()
        {
            MonitorCursor cursor = CreateTestCursor();
            Assert.AreEqual(new Vector2Int(-1, 0), cursor.Left);
            Assert.AreEqual(new Vector2Int(1, 0), cursor.Right);
            Assert.AreEqual(new Vector2Int(0, -1), cursor.Up);
            Assert.AreEqual(new Vector2Int(0, 1), cursor.Down);
        }

        // Function checking
        [Test]
        public void GetName()
        {
            MonitorCursor cursor = CreateTestCursor();
            Assert.AreEqual(CursorName, cursor.GetName());
        }

        [Test]
        public void SetBoundsGetBounds()
        {
            MonitorCursor cursor = CreateTestCursor();

            cursor.SetBounds(5, 6, 10, 11);

            Assert.AreEqual(new List<Vector2Int>() { new Vector2Int(5, 6), new Vector2Int(10, 11) }, cursor.GetBounds());

            cursor.SetPosition(5, 5);
            cursor.SetBounds(10, 10, 15, 15);
            Assert.AreEqual(new Vector2Int(10, 10), cursor.GetPosition());

            cursor.SetPosition(12, 12);
            Assert.AreEqual(new Vector2Int(12, 12), cursor.GetPosition());
        }

        [Test]
        public void SetPositionGetPosition()
        {
            MonitorCursor cursor = CreateTestCursor();
            cursor.SetPosition(5, 5);
            Assert.AreEqual(new Vector2Int(5, 5), cursor.GetPosition());

            cursor.SetPosition(10, 10);
            Assert.AreEqual(new Vector2Int(10, 10), cursor.GetPosition());
        }

        [Test]
        public void Move()
        {
            MonitorCursor cursor = CreateTestCursor();

            cursor.Move(cursor.Right);
            Assert.AreEqual(new Vector2Int(1, 0), cursor.GetPosition());
            cursor.Move(cursor.Left);
            Assert.AreEqual(new Vector2Int(0, 0), cursor.GetPosition());
            cursor.Move(cursor.Down);
            Assert.AreEqual(new Vector2Int(0, 1), cursor.GetPosition());
            cursor.Move(cursor.Up);
            Assert.AreEqual(new Vector2Int(0, 0), cursor.GetPosition());
        }

        [Test]
        public void MoveWithinBounds()
        {
            MonitorCursor cursor = CreateTestCursor();

            cursor.SetBounds(5, 5, 10, 10);
            cursor.ResetPosition();
            Assert.AreEqual(5, cursor.x);
            Assert.AreEqual(5, cursor.y);

            cursor.Move(cursor.Left);
            Assert.AreEqual(5, cursor.x);
            Assert.AreEqual(5, cursor.y);

            for (int moves = 0; moves < 50; moves++)
            {
                cursor.Move(cursor.Right);
                cursor.Move(cursor.Down);
            }

            Assert.AreEqual(10, cursor.x);
            Assert.AreEqual(10, cursor.y);
        }

        [Test]
        public void ResetPosition()
        {
            MonitorCursor cursor = CreateTestCursor();
            cursor.SetPosition(1, 10);
            cursor.ResetPosition();
            Assert.AreEqual(new Vector2Int(0, 0), cursor.GetPosition());
        }
    }
}