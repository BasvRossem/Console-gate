using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    /// <summary>
    /// This class is to function as a buffer between the interface and actually rendering the text.
    /// It is used to make text editing easier within the monitor class.
    /// </summary>
    public class TextGrid
    {
        private GridSize _size;

        private List<char[]> grid;

        /// <summary>
        /// Initialize a text grid.
        /// </summary>
        /// <param name="rowAmount">Amount of rows the text grid should have.</param>
        /// <param name="columnAmount">Amount of columns the text grid should have.</param>
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
        /// <returns>An array of specified length wfilled with the specified character.</returns>
        private char[] MakeRow(int length, char character)
        {
            char[] arr = new char[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = character;
            }
            return arr;
        }

        /// <summary>
        /// Get the size of the text grid.
        /// </summary>
        /// <returns>A Vector2int withthe size of the tecxt grid.</returns>
        public Vector2Int GetSize()
        {
            return new Vector2Int(grid.Count, grid[0].Length);
        }

        /// <summary>
        /// Clears a row at the specified index.
        /// </summary>
        /// <param name="index">The index of the row to be cleared.</param>
        public void ClearRow(int index)
        {
            grid[index] = MakeRow(_size.columns, ' ');
        }

        /// <summary>
        /// Fills the current grid with a specific character.
        /// </summary>
        /// <param name="character">The character to fill the grid with.</param>
        public void Fill(char character)
        {
            for (int y = 0; y < grid.Count; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    grid[y][x] = character;
                }
            }
        }

        public void Reset()
        {
            grid = new List<char[]>(_size.rows);
            for (int row = 0; row < _size.rows; row++) grid.Add(MakeRow(_size.columns, ' '));
        }

        // Operators
        /// <summary>
        /// Returns the character in the grid at the given location.
        /// </summary>
        /// <param name="index_row">Index of the wanted row.</param>
        /// <param name="index_column">Index of th wanted column.</param>
        /// <returns>A character at the place that is specified.</returns>
        public char this[int index_row, int index_column]
        {
            get => grid[index_row][index_column];
            set => grid[index_row][index_column] = value;
        }

        /// <summary>
        /// Returns the characters of the selected row.
        /// </summary>
        /// <param name="index_row">The index of the wanted row.</param>
        /// <returns>Returns an aray of all characters in the selected row.</returns>
        public char[] this[int index_row]
        {
            get => grid[index_row];
            set => grid[index_row] = value;
        }
    }
}