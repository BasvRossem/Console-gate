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
        public static Vector2Int Left = new Vector2Int(-1, 0);
        public static Vector2Int Right = new Vector2Int(1, 0);
        public static Vector2Int Up = new Vector2Int(0, -1);
        public static Vector2Int Down = new Vector2Int(0, 1);

        private Vector2Int _minBounds;
        private Vector2Int _maxBounds;
        private Vector2Int _position;
        public Vector2Int offset;
        public int x;
        public int y;

        private readonly string _name;
        public const string DefaultName = "Default";

        /// <summary>
        /// Initialize a cursor at he specified location.
        /// The cursor has a name so it can be specified when using multiple cursors in the monitor.
        /// </summary>
        /// <param name="x">The x coordinate in comparison to the text grid. Default of 0.</param>
        /// <param name="y">The y coordinate in comparison to the text grid. Default of 0.</param>
        /// <param name="cursorName">The name of the cursor. Default of "Default".</param>
        public Cursor(int x = 0, int y = 0, string cursorName = Cursor.DefaultName)
        {
            SetPosition(x, y);
            SetBounds();
            UpdateXY();

            _name = cursorName;
        }

        /// <summary>
        /// Returns the name of the cursor.
        /// </summary>
        /// <returns>The name.</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Set the bounds in which the cursor can write. And moves cursor to within those bounds.
        /// </summary>
        /// <param name="minX">Minimal x value.</param>
        /// <param name="maxX">Maximal x value.</param>
        /// <param name="minY">Minimal y value.</param>
        /// <param name="maxY">Maximal y value.</param>
        public void SetBounds(int minX = 0, int minY = 0, int maxX = int.MaxValue, int maxY = int.MaxValue)
        {
            _minBounds = new Vector2Int(minX, minY);
            _maxBounds = new Vector2Int(maxX, maxY);
            CheckBounds();
            UpdateXY();
        }

        /// <summary>
        /// Return a list of min and max bounds.
        /// </summary>
        /// <returns>A list of min and max bounds.</returns>
        public List<Vector2Int> GetBounds()
        {
            return new List<Vector2Int>() { _minBounds, _maxBounds };
        }

        // Positioning
        /// <summary>
        /// Set a new monitorPosition of the cursor within bounds.
        /// </summary>
        /// <param name="newX">New x monitorPosition. Default of 0.</param>
        /// <param name="newY">New y monitorPosition. Default of 0.</param>
        public void SetPosition(int newX = 0, int newY = 0)
        {
            _position = new Vector2Int(newX, newY);
            CheckBounds();

            UpdateXY();
        }

        /// <summary>
        /// Gets the monitorPosition.
        /// </summary>
        /// <returns>The monitorPosition.</returns>
        public Vector2Int GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// Move the cursor in a specified direction.
        /// </summary>
        /// <param name="direction">The direction wanted.</param>
        /// <remarks>
        /// These directions are specified with the cursor.
        /// These are up, down, left, and right.
        /// </remarks>
        public void Move(Vector2Int direction)
        {
            _position += direction;

            CheckBounds();
            UpdateXY();
        }

        /// <summary>
        /// Move the Cursor to the top left within its bounds.
        /// </summary>
        public void ResetPosition()
        {
            Move(new Vector2Int(int.MinValue, int.MinValue));
        }

        /// <summary>
        /// Moves the cursor back in bounds if it isn't.
        /// </summary>
        private void CheckBounds()
        {
            if (_position.x < _minBounds.x) _position.x = _minBounds.x;
            if (_position.x >= _maxBounds.x) _position.x = _maxBounds.x;

            if (_position.y < _minBounds.y) _position.y = _minBounds.y;
            if (_position.y >= _maxBounds.y) _position.y = _maxBounds.y;
        }

        /// <summary>
        /// Update the public x and y positioning.
        /// </summary>
        private void UpdateXY()
        {
            x = _position.x;
            y = _position.y;
        }
    }
}