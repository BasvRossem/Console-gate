using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Visuals;

namespace VisualsTests
{
    public class ViewTests
    {
        private View _view;
        private GridSize _size = new GridSize(5, 5);
        private GridPosition _start = new GridPosition(3, 3);
        private GridPosition _monitor = new GridPosition(8, 8);

        [SetUp]
        public void Setup()
        {
            _view = new View(_size, _start, _monitor);
        }

        [Test]
        public void Constructor()
        {
            Assert.AreEqual(_size, _view.size);
            Assert.AreEqual(_start, _view.internalPosition);
            Assert.AreEqual(_monitor, _view.externalPosition);
        }

        [Test]
        public void SetSizeNegative()
        {
            LogAssert.Expect(LogType.Error, "Size cannot be negative or zero.");
            var size = new GridSize(-5, -5);

            _view.SetSize(size);
        }

        [Test]
        public void SetExternalPositionNegative()
        {
            LogAssert.Expect(LogType.Error, "External position cannot be negative.");
            var position = new GridPosition(-5, -5);

            _view.SetExternalPosition(position);
        }

        [Test]
        public void SetInternalPositionNegative()
        {
            LogAssert.Expect(LogType.Error, "Internal position cannot be negative.");
            var position = new GridPosition(-5, -5);

            _view.SetInternalPosition(position);
        }

        [Test]
        public void HasChangedAfterInit()
        {
            Assert.IsTrue(_view.HasChanged());
        }

        [Test]
        public void SetTextGrid()
        {
            var grid = new TextGrid(new GridSize(24, 80));
            _view.SetSize(new GridSize(24, 80));
            _view.SetInternalPosition(new GridPosition(0));
            _view.SetText(grid);
            Assert.AreEqual(grid.GetSize(), _view.textGrid.GetSize());
        }

        [Test]
        public void MoveInternalPositionPositive()
        {
            _view.SetInternalPosition(new GridPosition());
            _view.StayInBounds(false);
            
            GridPosition positionBefore = _view.internalPosition;
            _view.MoveInternalPosition(1, 1, 1, 1);
            GridPosition positionAfter = _view.internalPosition;
            Assert.AreEqual(positionBefore, positionAfter);

            _view.MoveInternalPosition(down: 1, right: 1);
            var expected = new GridPosition(1, 1);
            GridPosition actual = _view.internalPosition;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MoveInternalPositionNegative()
        {
            LogAssert.Expect(LogType.Error, "Cannot move in a negative direction.");
            
            _view.internalPosition = new GridPosition();
            _view.MoveInternalPosition(-1, 0, 1, 5);
        }
        
        [Test]
        public void MoveInternalPositionTooMuchLeftAndUp()
        {
            LogAssert.Expect(LogType.Warning, "New view internal position row cannot be negative. Set to 0.");
            LogAssert.Expect(LogType.Warning, "New view internal position column cannot be negative. Set to 0.");
            _view.internalPosition = new GridPosition();
            _view.MoveInternalPosition(1, 0, 2, 1);
            Assert.AreEqual(new GridPosition(), _view.internalPosition);
        }

        [Test]
        public void GrowInAllDirections()
        {
            _view.SetInternalPosition(new GridPosition(5,5));
            _view.SetSize(new GridSize(10, 10));
            _view.Grow(1,1,1,1);
            Assert.AreEqual(new GridPosition(4,4), _view.internalPosition);
            Assert.AreEqual(new GridSize(12,12), _view.size);
        }
    }
}