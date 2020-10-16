using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using Visuals;

namespace Tests
{
    public class MonitorTests
    {
        public MonitorTestable CreateMonitor()
        {
            GameObject gameObject = new GameObject();
            TextMeshProUGUI mesh = gameObject.AddComponent<TextMeshProUGUI>();
            MonitorTestable monitor = gameObject.AddComponent<MonitorTestable>();

            monitor.textMesh = mesh;
            monitor.AddCursor("TestCursor");
            monitor.SelectCursor("TestCursor");

            return monitor;
        }

        [UnityTest]
        public IEnumerator WriteText()
        {
            Monitor monitor = CreateMonitor();
            monitor.AddMonitorTextLine("Hey how are you?");

            yield return null;
            Assert.AreEqual("Hey how are you?                                                                ".ToCharArray(), monitor.textGrid[0]);
        }
    }
}