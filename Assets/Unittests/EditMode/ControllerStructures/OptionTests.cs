using System;
using System.Collections;
using NUnit.Framework;
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
        public void PrintOptionLevelName()
        {
            
        }
    }
}