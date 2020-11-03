using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CoreTests
{
    public class ToolsTest
    {
        [Test]
        public void ErrorTrue()
        {
            LogAssert.Expect(LogType.Error, "Check error.");

            bool actual = Tools.CheckError(true, "Check error.");
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void ErrorFalse()
        {
            bool actual = Tools.CheckError(false, "Check error.");
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void WarningTrue()
        {
            LogAssert.Expect(LogType.Warning, "Check warning.");

            bool actual = Tools.CheckWarning(true, "Check warning.");
            Assert.IsTrue(actual);
        }

        [Test]
        public void WarningFalse()
        {
            bool actual = Tools.CheckWarning(false, "Check warning.");
            Assert.IsFalse(actual);
        }
    }
}