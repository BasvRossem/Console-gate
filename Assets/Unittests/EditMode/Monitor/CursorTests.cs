using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CursorTests
    {
        private string CursorName = "Newname";

        private Cursor CreateTestCursor()
        {
            return new Cursor(5, 5, CursorName);
        }

        [Test]
        public void CursorDirections()
        {
            Cursor cursor = CreateTestCursor();
            Assert.AreEqual(new Vector2Int(-1, 0), cursor.Left);
            Assert.AreEqual(new Vector2Int(1, 0), cursor.Right);
            Assert.AreEqual(new Vector2Int(0, -1), cursor.Up);
            Assert.AreEqual(new Vector2Int(0, 1), cursor.Down);
        }

        [Test]
        public void Name()
        {
            Cursor cursor = CreateTestCursor();
            Assert.AreEqual(CursorName, cursor.GetName());
        }

        [Test]
        public void MoveWithinBounds()
        {
            Cursor cursor = CreateTestCursor();

            cursor.SetBounds(5, 10, 5, 10);
            cursor.ResetPosition();
            Assert.AreEqual(5, cursor.x);
            Assert.AreEqual(5, cursor.y);

            cursor.Move(cursor.Left);
            Assert.AreEqual(5, cursor.x);
            Assert.AreEqual(5, cursor.y);

            for(int moves = 0; moves < 50; moves++)
            {
                cursor.Move(cursor.Right);
                cursor.Move(cursor.Down);
            }

            Assert.AreEqual(10, cursor.x);
            Assert.AreEqual(10, cursor.y);
        }
    }
}
