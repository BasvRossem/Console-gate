using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Visuals;

namespace VisualsTests
{
    public class TextGridTests
    {
        private TextGrid _grid;

        [SetUp]
        public void Setup()
        {
            _grid = new TextGrid(new GridSize(10, 10));
        }

        [Test]
        public void BracketsOperator()
        {
            _grid[1][1] = 'H';
            Assert.AreEqual('H', _grid[1][1]);

            char[] array = {'H', 'I'};
            _grid[8] = array;
            Assert.AreEqual(array, _grid[8]);
        }

        [Test]
        public void GetSize()
        {
            Assert.AreEqual(new Vector2Int(10, 10), _grid.GetSize());
        }

        [Test]
        public void Fill()
        {
            _grid.Fill('H');

            Vector2Int size = _grid.GetSize();

            for (var row = 0; row < size.y; row++)
            for (var column = 0; column < size.x; column++)
                Assert.AreEqual('H', _grid[row][column]);
        }

        [Test]
        public void ClearRow()
        {
            _grid.Fill('H');

            _grid.ClearRow(5);

            for (var i = 0; i < _grid[5].Count(); i++) Assert.AreEqual(' ', _grid[5][i]);
        }
    }
}