using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
using Visuals;

namespace VisualsTests
{
    public class LayerTests
    {
        private Layer layer;

        [SetUp]
        public void Setup()
        {
            layer = new Layer(new GridSize(10, 20), 1);
        }

        [Test]
        public void WriteText()
        {
            layer.WriteText("There is, a house\nIn New Orleans.");
            Assert.AreEqual("There is, a house   ", new string (layer.textGrid[0]));
            Assert.AreEqual("In New Orleans.     ", new string (layer.textGrid[1]));
        }
        
        [Test]
        public void CursorOutOfTextBounds()
        {
            LogAssert.Expect(LogType.Warning, "Cursor is out of bounds. Ignoring character.");
            layer.WriteText("There is a house in New Orleans.");
            Assert.AreEqual("There is a house in ", new string (layer.textGrid[0]));
        }

        [Test]
        public void HasChangedAfterWrite()
        {
            layer.WriteText("Test message", false);
            Assert.IsTrue(layer.HasChanged());
        }

        [Test]
        public void CheckView()
        {
            layer.WriteText("Test message");
            View view = layer.RenderView();
            Assert.AreEqual("Test message        ", new string(view.textGrid[0]));
        }

        [Test]
        public void ClearLine()
        {
            layer.WriteLine("Test");
            layer.ClearLine(0);
            Assert.AreEqual("                    ", new string(layer.textGrid[0]));
        }

        [Test]
        public void ClearNegativeLine()
        {
            LogAssert.Expect(LogType.Error, "Index [-1] cannot be negative.");
            layer.ClearLine(-1);
        }

        [Test]
        public void ClearLineOutsideScope()
        {
            LogAssert.Expect(LogType.Error, "Index [20] is higher than lines on the layer.");
            layer.ClearLine(20);
        }

        [Test]
        public void DrawRectangle()
        {
            layer.DrawRectangle(0,0, 9,19);
            
            Assert.AreEqual("*------------------*", new string(layer.textGrid[0]));
            Assert.AreEqual("*------------------*", new string(layer.textGrid[9]));
            for (var i = 1; i < 9; i++)
            {
                Assert.AreEqual("|                  |", new string(layer.textGrid[i]));
            }
        }

        [Test]
        public void ClearArea()
        {
            layer.textGrid.Fill('*');
            layer.ClearArea(0,0,9,19);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual("                    ", new string(layer.textGrid[i]));
            }
        }
    }
}