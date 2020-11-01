using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class Collection
    {
        [Test]
        public void GridSizeRows()
        {
            var size = new GridSize(5, 10);
            Assert.AreEqual(5, size.rows);
        }
        
        [Test]
        public void GridSizeColumns()
        {
            var size = new GridSize(5, 10);
            Assert.AreEqual(10, size.columns);
        }

        [Test]
        public void GridSizeToString()
        {
            var size = new GridSize(5, 10);
            
            var expected = "5, 10";
            var actual = size.ToString();
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GridPositionRows()
        {
            var size = new GridPosition(5, 10);
            Assert.AreEqual(5, size.row);
        }
        
        [Test]
        public void GridPositionColumns()
        {
            var size = new GridPosition(5, 10);
            Assert.AreEqual(10, size.column);
        }

        [Test]
        public void GridPositionToString()
        {
            var size = new GridPosition(5, 10);
            
            var expected = "5, 10";
            var actual = size.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}