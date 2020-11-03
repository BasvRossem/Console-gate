using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ControllerStructures
{
    public class OptionTests
    {
        [Test]
        public void DefaultOptionText()
        {
            Option option = new Option("Text");
            Assert.AreEqual("Text", option.text);
        }

        [Test]
        public void DefaultOptionRunThrowsException()
        {
            Option option = new Option("Text");
            Assert.Throws<NotImplementedException>(option.Run);
        }

        [Test]
        public void PrintOptionPrintText()
        {
            LogAssert.Expect(LogType.Log, "This is printed.");
            OptionPrint option = new OptionPrint("Text", "This is printed.");
            option.Run();
        }

        [Test]
        public void LoadLevelOptionLoadsLevel()
        {
            OptionLoadLevel option = new OptionLoadLevel("", "This does not exist.");
            Assert.Throws<InvalidOperationException>(option.Run);
        }
    }
}