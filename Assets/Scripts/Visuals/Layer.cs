using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Visuals
{
    public class Layer
    {
        public TextGrid textGrid;
        public Cursor cursor;
        public View view;
        public int zIndex;

        public bool isChanged;

        private GridSize _size;

        public Layer(GridSize size, int z = 0)
        {
            _size = size;

            textGrid = new TextGrid(_size);
            cursor = new Cursor();
            view = new View(_size);
            zIndex = z;

            Change();
        }

        // Writing to the layer
        /// <summary>
        /// Write a single character to the Layer and move the cursor accordingly.
        /// </summary>
        /// <param name="letter"></param>
        public void WriteCharacter(char letter)
        {
            if (Tools.CheckWarning(cursor.x >= textGrid.GetSize().y || cursor.y >= textGrid.GetSize().x, "Cursor is out of bounds. Ignoring character.")) return;

            textGrid[cursor.y, cursor.x] = letter;
            cursor.Move(Cursor.Right);
            Change();
        }

        /// <summary>
        /// Add text at the current place of the cursor.
        /// </summary>
        /// <note>
        /// This moves the cursor one line down automatically when the line is written.
        /// </note>
        /// <param name="newTextLine">Text to place.</param>
        /// <param name="automaticReturn"></param>
        public void WriteLine(string newTextLine, bool automaticReturn = true)
        {
            string[] lines = newTextLine.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (char character in lines[i])
                {
                    WriteCharacter(character);
                }

                if (i == lines.Length - 1 && (i != lines.Length - 1 || !automaticReturn)) continue;

                cursor.Move(Cursor.Down);
                cursor.Move(new Vector2Int(-1 * _size.columns, 0));
            }
            Change();
        }

        /// <summary>
        /// Clears the screen and sets the Layer text.
        /// </summary>
        /// <param name="newText">Text to write to the Layer.</param>
        public void WriteText(string newText, bool automaticReturn = true)
        {
            cursor.ResetPosition();
            textGrid.Reset();
            WriteLine(newText, automaticReturn);
            Change();
        }

        // Clearing
        /// <summary>
        /// Remove a line from the text.
        /// </summary>
        /// <param name="index">The index of the line to remove.</param>
        public void ClearLine(int index)
        {
            if (Tools.CheckError(index < 0, $"Index [{index}] cannot be negative.")) return;
            if (Tools.CheckError(index > _size.rows - 1, $"Index [{index}] is higher than lines on the layer.")) return;

            textGrid.ClearRow(index);
            Change();
        }

        /// <summary>
        /// Remove characters in a ceratin area.
        /// </summary>
        /// <param name="startRowIndex">Start row.</param>
        /// <param name="startColumnIndex">Start column.</param>
        /// <param name="endRowIndex">End row.</param>
        /// <param name="endColumnIndex">End column.</param>
        public void ClearArea(int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
        {
            for (int x = startColumnIndex; x <= endColumnIndex; x++)
            {
                for (int y = startRowIndex; y <= endRowIndex; y++)
                {
                    textGrid[y, x] = ' ';
                }
            }
            Change();
        }

        // Drawing shapes to the Layer
        /// <summary>
        /// Draw a horizontal line.
        /// </summary>
        /// <param name="row">The index of the row where the line should be.</param>
        /// <param name="startColumn">The index of the column where the line should start.</param>
        /// <param name="endColumn">The index of the column there the line should end.</param>
        public void DrawLineHorizontal(int row, int startColumn, int endColumn)
        {
            for (int column = startColumn; column < endColumn; column++)
            {
                textGrid[row][column] = '-';
            }
            Change();
        }

        /// <summary>
        /// Draw a vertical line.
        /// </summary>
        /// <param name="column">The index of the column where the line should be.</param>
        /// <param name="startRow">The index of the row where the line should start.</param>
        /// <param name="endRow">The index of the row where the line should end.</param>
        public void DrawLineVertical(int column, int startRow, int endRow)
        {
            for (int row = startRow; row < endRow; row++)
            {
                textGrid[row][column] = '|';
            }
            Change();
        }

        /// <summary>
        /// Draw a rectangle using the rows and column indexes.
        /// </summary>
        /// <param name="startRowIndex">Index of the row where the box should start.</param>
        /// <param name="startColumnIndex">Index of the column where the box should start.</param>
        /// <param name="endRowIndex">Index of the row where the box should end.</param>
        /// <param name="endColumnIndex">Index of the column where the box should end.</param>
        /// <remarks>
        /// Calling the function <c>Layer.DrawRectangle(0, 0, 5, 5)</c> would create:
        /// *----*
        /// |    |
        /// |    |
        /// |    |
        /// |    |
        /// *----*
        /// </remarks>
        public void DrawRectangle(int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
        {
            // Draw lines
            DrawLineHorizontal(startRowIndex, startColumnIndex + 1, endColumnIndex);
            DrawLineHorizontal(endRowIndex, startColumnIndex + 1, endColumnIndex);
            DrawLineVertical(startColumnIndex, startRowIndex + 1, endRowIndex);
            DrawLineVertical(endColumnIndex, startRowIndex + 1, endRowIndex);

            // Draw Corners
            textGrid[startRowIndex, startColumnIndex] = '*';
            textGrid[startRowIndex, endColumnIndex] = '*';
            textGrid[endRowIndex, startColumnIndex] = '*';
            textGrid[endRowIndex, endColumnIndex] = '*';

            Change();
        }

        // Rendering
        public View RenderView()
        {
            view.SetText(textGrid);
            return view;
        }

        // Change
        public void Change(bool changed = true)
        {
            isChanged = changed;
        }

        public bool HasChanged()
        {
            return (isChanged || view.HasChanged());
        }
    }
}