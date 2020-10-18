using NUnit.Framework;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Visuals;

namespace Tests
{
    public class MonitorTests
    {
        private GameObject monitorObject;
        private GameObject uiCursorObject;

        private Monitor monitor;
        private TextMeshProUGUI mesh;

        private UICursor uiCursor;
        private Image image;

        private static string newCursorName = "New Cursor";

        [SetUp]
        public void SetUp()
        {
            monitorObject = new GameObject();
            mesh = monitorObject.AddComponent<TextMeshProUGUI>();
            monitor = monitorObject.AddComponent<MonitorTestable>();

            uiCursorObject = new GameObject();
            uiCursor = uiCursorObject.AddComponent<UICursor>();
            image = uiCursorObject.AddComponent<Image>();

            monitor.textMesh = mesh;
            monitor.SelectCursor(MonitorCursor.DefaultName);
            monitor.uiCursor = uiCursor;
            monitor.textGrid = new TextGrid(monitor.GetRowAmount(), monitor.GetColumnAmount());

            monitor.ResetMonitor();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(monitorObject);
            Object.Destroy(uiCursorObject);
            Object.Destroy(monitor);
            Object.Destroy(mesh);
            Object.Destroy(uiCursor);
            Object.Destroy(image);
        }

        // Cursor tests
        [UnityTest]
        public IEnumerator AddCursor()
        {
            Assert.IsFalse(monitor.cursors.Where(cursor => cursor.GetName() == newCursorName).Any());

            monitor.AddCursor(newCursorName);
            Assert.IsTrue(monitor.cursors.Where(cursor => cursor.GetName() == newCursorName).Any());
            yield return null;
        }

        [UnityTest]
        public IEnumerator SelectCursor()
        {
            Assert.IsFalse(monitor.selectedCursor.GetName() == newCursorName);

            monitor.AddCursor(newCursorName);
            monitor.SelectCursor(newCursorName);
            Assert.IsTrue(monitor.selectedCursor.GetName() == newCursorName);

            yield return null;
        }

        [UnityTest]
        public IEnumerator RemoveCursor()
        {
            monitor.AddCursor(newCursorName);
            monitor.SelectCursor(newCursorName);

            UnityEngine.TestTools.LogAssert.Expect(LogType.Error, "Cannot remove selected cursor with name \"New Cursor\"");
            Assert.IsFalse(monitor.RemoveCursor(newCursorName));

            monitor.SelectCursor(MonitorCursor.DefaultName);
            Assert.IsTrue(monitor.RemoveCursor(newCursorName));

            yield return null;
        }

        // Write tests
        [UnityTest]
        public IEnumerator WriteText()
        {
            monitor.AddMonitorTextLine("Hey how are you?");

            char[] expected = "Hey how are you?                                                                ".ToCharArray();
            char[] actual = monitor.textGrid[0];
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        // Drawing
        [UnityTest]
        public IEnumerator DrawRectangleUsingDrawLines()
        {
            monitor.DrawRectangle(0, 0, 5, 5);
            char[,] expected = new char[,] {
                {'*', '-', '-', '-', '-', '*'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'*', '-', '-', '-', '-', '*'},
            };

            char[,] actual = new char[6, 6];
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    actual[x, y] = monitor.textGrid[x][y];
                }
            }

            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ClearArea()
        {
            monitor.DrawRectangle(0, 0, 5, 5);
            monitor.DrawRectangle(1, 1, 4, 4);
            monitor.DrawRectangle(2, 2, 3, 3);
            monitor.ClearArea(1, 1, 4, 4);
            char[,] expected = new char[,] {
                {'*', '-', '-', '-', '-', '*'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', '|'},
                {'*', '-', '-', '-', '-', '*'},
            };

            char[,] actual = new char[6, 6];
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    actual[x, y] = monitor.textGrid[x][y];
                }
            }

            Assert.AreEqual(expected, actual);

            yield return null;
        }
    }
}