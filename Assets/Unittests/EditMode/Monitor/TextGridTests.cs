using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngineInternal;

namespace Tests
{
    public class TextGridTests
    {
        [Test]
        public void BracketsOperator()
        {
            TextGrid grid = new TextGrid(10, 10);

            grid[1][1] = 'H';
            Assert.AreEqual('H', grid[1][1]);

            char[] array = { 'H', 'I' };
            grid[8] = array;
            Assert.AreEqual(array, grid[8]);
        }

        [Test]
        public void Fill()
        {
            TextGrid grid = new TextGrid(10, 10);

            grid.Fill('H');

            Vector2Int size = grid.GetSize();

            for (int row = 0; row < size.y; row++)
            {
                for (int column = 0; column < size.x; column++)
                {
                    Assert.AreEqual('H', grid[row][column]);
                }
            }
        }

        [Test]
        public void ClearRow()
        {
            TextGrid grid = new TextGrid(10, 10);

            grid.Fill('H');

            grid.ClearRow(5);

            for (int i = 0; i < grid[5].Count(); i++)
            {
                Assert.AreEqual(' ', grid[5][i]);
            }
        }
    }
}
