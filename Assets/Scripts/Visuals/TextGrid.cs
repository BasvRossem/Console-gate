using System.Collections.Generic;

namespace Visuals
{
    /// <summary>
    /// This class is to function as a buffer between the interface and actually rendering the text.
    /// It is used to make text editing and writing easier.
    /// </summary>
    public class TextGrid
    {
        private GridSize _size;

        private List<char[]> _grid;

        public TextGrid(GridSize size)
        {
            _size = size;
            Reset();
        }

        /// <summary>
        /// Generates a row of characters.
        /// </summary>
        /// <param name="length">Length of the array.</param>
        /// <param name="character">Character to fill the array with.</param>
        /// <returns>An array of specified length, filled with the specified character.</returns>
        private static char[] MakeRow(int length, char character)
        {
            var arr = new char[length];
            for (var i = 0; i < length; i++)
            {
                arr[i] = character;
            }
            return arr;
        }

        /// <summary>
        /// Get the size of the text grid.
        /// </summary>
        /// <returns>A GrisSize with the size of the text grid.</returns>
        public GridSize GetSize()
        {
            _size = new GridSize(_grid.Count, _grid[0].Length);
            return _size;
        }

        /// <summary>
        /// Clears a row at the specified index.
        /// </summary>
        /// <param name="index">The index of the row to be cleared.</param>
        public void ClearRow(int index)
        {
            _grid[index] = MakeRow(_size.columns, ' ');
        }

        /// <summary>
        /// Fills the current grid with a specific character.
        /// </summary>
        /// <param name="character">The character to fill the grid with.</param>
        public void Fill(char character)
        {
            for (var row = 0; row < _size.rows; row++)
            {
                for (var column = 0; column < _size.columns; column++)
                {
                    _grid[row][column] = character;
                }
            }
        }

        /// <summary>
        /// Resize the grid to its starting size and clear the grid.
        /// </summary>
        public void Reset()
        {
            _grid = new List<char[]>(_size.rows);
            for (var row = 0; row < _size.rows; row++) _grid.Add(MakeRow(_size.columns, ' '));
        }

        /// <summary>
        /// Adds an empty row ro the grid.
        /// </summary>
        public void AddRow()
        {
            _grid.Add(MakeRow(_size.columns, ' '));
        }
        // Operators
        /// <summary>
        /// Returns the character in the grid at the given location.
        /// </summary>
        /// <param name="indexRow">Index of the wanted row.</param>
        /// <param name="indexColumn">Index of th wanted column.</param>
        /// <returns>A character at the place that is specified.</returns>
        public char this[int indexRow, int indexColumn]
        {
            get => _grid[indexRow][indexColumn];
            set => _grid[indexRow][indexColumn] = value;
        }

        /// <summary>
        /// Returns the characters of the selected row.
        /// </summary>
        /// <param name="indexRow">The index of the wanted row.</param>
        /// <returns>Returns an array of all characters in the selected row.</returns>
        public char[] this[int indexRow]
        {
            get => _grid[indexRow];
            set => _grid[indexRow] = value;
        }
    }
}