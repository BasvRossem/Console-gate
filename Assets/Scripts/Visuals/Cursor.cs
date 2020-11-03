using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    /// <summary>
    /// This class is a cursor which the monitor class uses to write to the text grid.
    /// </summary>
    public class Cursor
    {
        public static readonly GridPosition Up = new GridPosition(-1, 0);
        public static readonly GridPosition Down = new GridPosition(1, 0);
        public static readonly GridPosition Left = new GridPosition(0, -1);
        public static readonly GridPosition Right = new GridPosition(0, 1);

        private GridPosition _minBounds;
        private GridPosition _maxBounds;
        
        public GridPosition position;
        public GridPosition offset;

        /// <summary>
        /// Initialize a cursor at he specified location.
        /// The cursor has a name so it can be specified when using multiple cursors in the monitor.
        /// </summary>
        /// <param name="x">The x coordinate in comparison to the text grid. Default of 0.</param>
        /// <param name="y">The y coordinate in comparison to the text grid. Default of 0.</param>
        public Cursor(int row = 0, int column = 0)
        {
            SetPosition(new GridPosition(row, column));
            SetBounds(new GridPosition(0, 0), new GridPosition(int.MaxValue, int.MaxValue));
        }

        /// <summary>
        /// Set the bounds in which the cursor can write. And moves cursor to within those bounds.
        /// </summary>
        /// <param name="minX">Minimal x value.</param>
        /// <param name="maxX">Maximal x value.</param>
        /// <param name="minY">Minimal y value.</param>
        /// <param name="maxY">Maximal y value.</param>
        public void SetBounds(GridPosition minPosition, GridPosition maxPosition)
        {
            _minBounds = minPosition;
            _maxBounds = maxPosition;
            CheckBounds();
        }

        /// <summary>
        /// Return a list of min and max bounds.
        /// </summary>
        /// <returns>A list of min and max bounds.</returns>
        public List<GridPosition> GetBounds()
        {
            return new List<GridPosition>() { _minBounds, _maxBounds };
        }

        // Positioning
        /// <summary>
        /// Set a new monitorPosition of the cursor within bounds.
        /// </summary>
        /// <param name="newPosition">New position on the layer.</param>
        public void SetPosition(GridPosition newPosition)
        {
            position = newPosition;
            CheckBounds();
        }

        /// <summary>
        /// Move the cursor in a specified direction.
        /// </summary>
        /// <param name="direction">The direction wanted.</param>
        /// <remarks>
        /// These directions are specified with the cursor.
        /// These are up, down, left, and right.
        /// </remarks>
        public void Move(GridPosition direction)
        {
            position += direction;

            CheckBounds();
        }

        /// <summary>
        /// Move the Cursor to the top left within its bounds.
        /// </summary>
        public void ResetPosition()
        {
            Move(new GridPosition(int.MinValue, int.MinValue));
        }

        /// <summary>
        /// Moves the cursor back in bounds if it is not.
        /// </summary>
        private void CheckBounds()
        {
            if (position.column < _minBounds.column) position.column = _minBounds.column;
            if (position.column > _maxBounds.column) position.column = _maxBounds.column;

            if (position.row < _minBounds.row) position.row = _minBounds.row;
            if (position.row > _maxBounds.row) position.row = _maxBounds.row;
        }
    }
}