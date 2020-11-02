using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Visuals
{
    /// <summary>
    /// This class is a cursor which the monitor class uses to write to the text grid.
    /// </summary>
    public class MonitorCursor
    {
        public Vector2Int Left = new Vector2Int(-1, 0);
        public Vector2Int Right = new Vector2Int(1, 0);
        public Vector2Int Up = new Vector2Int(0, -1);
        public Vector2Int Down = new Vector2Int(0, 1);

        private Vector2Int min_bounds;
        private Vector2Int max_bounds;
        private Vector2Int position;
        public Vector2Int offset;
        public int x;
        public int y;

        private string name;
        public const string DefaultName = "Default";

        /// <summary>
        /// Initialize a cursor at he specified location.
        /// The cursor has a name so it can be specified when using multiple cursors in the monitor.
        /// </summary>
        /// <param name="x">The x coordinate in comparison to the text grid. Default of 0.</param>
        /// <param name="y">The y coordinate in comparison to the text grid. Default of 0.</param>
        /// <param name="cursorName">The name of the cursor. Default of "Default".</param>
        public MonitorCursor(int x = 0, int y = 0, string cursorName = MonitorCursor.DefaultName)
        {
            SetPosition(x, y);
            SetBounds();
            UpdateXY();

            name = cursorName;
        }

        /// <summary>
        /// Returns the name of the cursor.
        /// </summary>
        /// <returns>The name.</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Set the bounds in which the cursor can write. And moves cursor to within those bounds.
        /// </summary>
        /// <param name="min_x">Minimal x value.</param>
        /// <param name="max_x">Maximal x value.</param>
        /// <param name="min_y">Minimal y value.</param>
        /// <param name="max_y">Maximal y value.</param>
        public void SetBounds(int min_x = 0, int min_y = 0, int max_x = int.MaxValue, int max_y = int.MaxValue)
        {
            min_bounds = new Vector2Int(min_x, min_y);
            max_bounds = new Vector2Int(max_x, max_y);
            CheckBounds();
            UpdateXY();
        }

        /// <summary>
        /// Return a list of min and max bounds.
        /// </summary>
        /// <returns>A list of min and max bounds.</returns>
        public List<Vector2Int> GetBounds()
        {
            return new List<Vector2Int>() { min_bounds, max_bounds };
        }

        // Positioning
        /// <summary>
        /// Set a new position of the cursor within bounds.
        /// </summary>
        /// <param name="x">New x position. Default of 0.</param>
        /// <param name="y">New y position. Default of 0.</param>
        public void SetPosition(int x = 0, int y = 0)
        {
            position = new Vector2Int(x, y);
            CheckBounds();

            UpdateXY();
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns>The position.</returns>
        public Vector2Int GetPosition()
        {
            return position;
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
            position += direction;

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
            if (position.x < min_bounds.x) position.x = min_bounds.x;
            if (position.x >= max_bounds.x) position.x = max_bounds.x;

            if (position.y < min_bounds.y) position.y = min_bounds.y;
            if (position.y >= max_bounds.y) position.y = max_bounds.y;
        }

        /// <summary>
        /// Update the public x and y positioning.
        /// </summary>
        private void UpdateXY()
        {
            x = position.x;
            y = position.y;
        }
    }

    /// <summary>
    /// This class is to function as a buffer between the interface and actually rendering the text.
    /// It is used to make text editing easier within the monitor class.
    /// </summary>
    public class TextGrid
    {
        private int columnAmount;
        private int rowAmount;

        private List<char[]> grid;

        /// <summary>
        /// Initialize a text grid.
        /// </summary>
        /// <param name="RowAmount">Amount of rows the text grid should have.</param>
        /// <param name="ColumnAmount">Amount of columns the text grid should have.</param>
        public TextGrid(int RowAmount, int ColumnAmount)
        {
            rowAmount = RowAmount;
            columnAmount = ColumnAmount;

            grid = new List<char[]>(rowAmount);
            for (int row = 0; row < rowAmount; row++) AddExtraRow();
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
            grid[index] = MakeRow(columnAmount, ' ');
        }

        /// <summary>
        /// Fills the current grid with a specific character.
        /// </summary>
        /// <param name="character">The character to fill the grid with.</param>
        public void Fill(char character)
        {
            for (int y = 0; y < grid.Count(); y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    grid[y][x] = character;
                }
            }
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

        public void AddExtraRow()
        {
            grid.Add(MakeRow(columnAmount, ' '));
        }
    }

    /// <summary>
    /// This class is used to show text on the screen.
    /// </summary>
    public class Monitor : MonoBehaviour
    {
        public TextMeshProUGUI textMesh;
        private List<List<Vector2>> textMeshCharacterPositions;

        private const int RowAmount = 24;
        private const int ColumnAmount = 80;
        public TextGrid textGrid = new TextGrid(RowAmount, ColumnAmount);

        protected MonitorCursor defaultCursor = new MonitorCursor();
        public MonitorCursor selectedCursor;
        public List<MonitorCursor> cursors = new List<MonitorCursor>();
        public UICursor uiCursor;

        private string text;

        public int verticalViewOffset = 0;

        private void Awake()
        {
            selectedCursor = defaultCursor;

            cursors.Add(defaultCursor);

            CalibrateTextMesh();
        }

        private void Update()
        {
            RenderMonitorText();
            if ((uiCursor != null) && (uiCursor.linkedCursor != null)) UpdateUICursorPosition();
        }

        /// <summary>
        /// Calibrate the character positions to use with the UI cursor.
        /// </summary>
        /// <remarks>
        /// Text mesh does not really render spaces in its mesh.
        /// This and other factors have created the need of this function.
        /// </remarks>
        private void CalibrateTextMesh()
        {
            // Fill text grid with data
            textGrid.Fill('*');

            // Render data to text mesh
            RenderMonitorText();
            textMesh.ForceMeshUpdate();

            // Calculate character center positions
            textMeshCharacterPositions = new List<List<Vector2>>();
            for (int row = 0; row < textMesh.textInfo.lineCount; row++)
            {
                List<Vector2> rowPositions = new List<Vector2>();

                int firstCharacterIndex = textMesh.textInfo.lineInfo[row].firstCharacterIndex;
                for (int column = 0; column < textMesh.textInfo.lineInfo[row].characterCount; column++)
                {
                    TMP_CharacterInfo characterInfo = textMesh.textInfo.characterInfo[firstCharacterIndex + column];
                    Vector2 characterCenter = ((characterInfo.topLeft + characterInfo.bottomRight) / 2) + textMesh.transform.position;

                    rowPositions.Add(characterCenter);
                }

                textMeshCharacterPositions.Add(rowPositions);
            }

            // Empty monitor
            ResetMonitor();
        }

        // Cursor
        /// <summary>
        /// Select the deafult cursor if no cursor is selected.
        /// </summary>
        private void NullCoalesceSelectedCursor()
        {
            selectedCursor = selectedCursor ?? defaultCursor;
        }

        /// <summary>
        /// Find a cursor in the cursor list with the given name.
        /// </summary>
        /// <param name="name">Name of the cursor.</param>
        /// <returns>If the cursor exists, it returns the cursor, else null.</returns>
        private MonitorCursor FindCursor(string name)
        {
            IEnumerable<MonitorCursor> foundCursors = cursors.Where(cursor => cursor.GetName() == name);
            if (foundCursors.Count() == 0) return null;
            return foundCursors.First();
        }

        /// <summary>
        /// Selects a cursor using the name.
        /// </summary>
        /// <param name="name">Name of the cursor to select.</param>
        /// <returns>True is succesful, false if no cursor with the given name is found.</returns>
        public bool SelectCursor(string name)
        {
            MonitorCursor newSelected = FindCursor(name);
            if (Tools.CheckWarning(newSelected == null, string.Format("No cursor found with name \"{0}\". Swiched to default.", name)))
            {
                selectedCursor = defaultCursor;
                return false;
            }

            selectedCursor = newSelected;
            return true;
        }

        /// <summary>
        /// Add a new cursor to the monitor.
        /// </summary>
        /// <param name="name">Name of the new cursor.</param>
        /// <returns>The new cursor name, or null if a cursor with that name already exists.</returns>
        public string AddCursor(string name)
        {
            if (Tools.CheckError(FindCursor(name) != null, "A cursor with this name already exists.")) return null;

            MonitorCursor newCursor = new MonitorCursor(cursorName: name);
            cursors.Add(newCursor);

            return name;
        }

        /// <summary>
        /// Remove a cursor using the name.
        /// </summary>
        /// <param name="name">Name of the cursor to select.</param>
        /// <returns>True is succesful, false if no cursor with the given name exists.</returns>
        public bool RemoveCursor(string name)
        {
            MonitorCursor cursorToRemove = FindCursor(name);
            if (Tools.CheckError(cursorToRemove == null, string.Format("No cursor found with name \"{0}\"", name))) return false;
            if (Tools.CheckError(cursorToRemove.GetName() == selectedCursor.GetName(), string.Format("Cannot remove selected cursor with name \"{0}\"", name))) return false;
            cursors.Remove(cursorToRemove);
            return true;
        }

        // Writing to the monitor
        /// <summary>
        /// Write a single character to the monitor and move the cursor accordingly.
        /// </summary>
        /// <param name="letter"></param>
        public void WriteCharacter(char letter)
        {
            NullCoalesceSelectedCursor();
            if (selectedCursor.x >= textGrid.GetSize().y) return;

            while (selectedCursor.GetPosition().y >= textGrid.GetSize().x)
            {
                textGrid.AddExtraRow();
            }

            textGrid[selectedCursor.y, selectedCursor.x] = letter;
            selectedCursor.Move(selectedCursor.Right);
        }

        /// <summary>
        /// Turn the text grid into strings to write to the monitor.
        /// </summary>
        private void AssembleText()
        {
            text = "";
            for (int y = verticalViewOffset; y < verticalViewOffset + RowAmount; y++)
            {
                text += new string(textGrid[y]);
                text += "\n";
            }
        }

        /// <summary>
        /// Show the text to the screen.
        /// </summary>
        private void RenderMonitorText()
        {
            AssembleText();
            textMesh.SetText(text);
        }

        /// <summary>
        /// Remove all text from the monitor.
        /// </summary>
        public void ResetMonitor()
        {
            textGrid = new TextGrid(RowAmount, ColumnAmount);
        }

        /// <summary>
        /// Clears the screen and sets the monitor text.
        /// </summary>
        /// <param name="newText">Text to write to the monitor.</param>
        public void SetMonitorText(string newText)
        {
            selectedCursor.ResetPosition();
            ResetMonitor();
            WriteLine(newText);
        }

        /// <summary>
        /// Add text at the current place of the cursor.
        /// </summary>
        /// <note>
        /// This moves the cursor one line down automatically when the line is written.
        /// </note>
        /// <param name="newTextLine">Text to place.</param>
        public void WriteLine(string newTextLine, bool automaticReturn = true)
        {
            NullCoalesceSelectedCursor();
            var lines = newTextLine.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (char character in lines[i])
                {
                    WriteCharacter(character);
                }
                if (i != lines.Length - 1 || (i == lines.Length - 1 && automaticReturn))
                {
                    selectedCursor.Move(selectedCursor.Down);
                    selectedCursor.Move(new Vector2Int(-1 * ColumnAmount, 0));
                }
            }
        }

        /// <summary>
        /// Remove a line from the text.
        /// </summary>
        /// <param name="index">The index of the line to remove.</param>
        public void RemoveMonitorTextLineAtPosition(int index)
        {
            if (Tools.CheckError(index < 0, string.Format("Index {0} cannot be negative.", index))) return;
            if (Tools.CheckError(index > RowAmount - 1, string.Format("Index {0} is higher than lines on the monitor.", index))) return;

            textGrid.ClearRow(index);
        }

        /// <summary>
        /// Remove characters in a ceratin area.
        /// </summary>
        /// <param name="startRow">Start row.</param>
        /// <param name="startColumn">Start column.</param>
        /// <param name="endRow">End row.</param>
        /// <param name="endColumn">End column.</param>
        public void ClearArea(int startRow, int startColumn, int endRow, int endColumn)
        {
            for (int x = startColumn; x <= endColumn; x++)
            {
                for (int y = startRow; y <= endRow; y++)
                {
                    textGrid[y, x] = ' ';
                }
            }
        }

        // Drawing shapes to the monitor
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
        }

        /// <summary>
        /// Draw a rectangle using the rows and column indexes.
        /// </summary>
        /// <param name="startRowIndex">Index of the row where the box should start.</param>
        /// <param name="startColumnIndex">Index of the column where the box should start.</param>
        /// <param name="endRowIndex">Index of the row where the box should end.</param>
        /// <param name="endColumnIndex">Index of the column where the box should end.</param>
        /// <remarks>
        /// Calling the function <c>Monitor.DrawRectangle(0, 0, 5, 5)</c> would create:
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
        }

        // Monitor information
        /// <summary>
        /// Get the amount of characters the monitor is high.
        /// </summary>
        /// <returns>The amount of rows.</returns>
        public int GetRowAmount()
        {
            return RowAmount;
        }

        /// <summary>
        /// Get the amount of characters the monitor is wide.
        /// </summary>
        /// <returns>The amount of columns.</returns>
        public int GetColumnAmount()
        {
            return ColumnAmount;
        }

        // UI Cursor
        /// <summary>
        /// Select a row on the monitor iusing the UI cursor.
        /// </summary>
        /// <param name="row">The index of the row to be selected</param>
        public void SelectRow(int row)
        {
            // Mesh info is probably only loaded at the text end of the frame.
            // So now we force an update so we can use the mesh data.
            RenderMonitorText();
            textMesh.ForceMeshUpdate();

            if (Tools.CheckError((row >= textMesh.textInfo.lineCount), string.Format("Cannot acces row with index {0}, there are {1} lines.", row, textMesh.textInfo.lineCount))) return;

            // Retrieve character data
            TMP_LineInfo lineInfo = textMesh.textInfo.lineInfo[row];
            TMP_CharacterInfo characterInfoBegin = textMesh.textInfo.characterInfo[lineInfo.firstCharacterIndex];
            TMP_CharacterInfo characterInfoFinal = textMesh.textInfo.characterInfo[lineInfo.lastCharacterIndex];

            Vector2 newPositionCenter = ((characterInfoBegin.topLeft + characterInfoFinal.bottomRight) / 2) + textMesh.transform.position;
            Vector2 newPositionOffset = new Vector2(-1, -1);
            Vector2 newSize = new Vector2(uiCursor.characterSize.x * ColumnAmount, uiCursor.characterSize.y);

            uiCursor.SetSize(newSize);
            uiCursor.SetPositionCenter(newPositionCenter + newPositionOffset);
        }

        /// <summary>
        /// Update the UI cursor position to its linked cursor.
        /// </summary>
        private void UpdateUICursorPosition()
        {
            if (selectedCursor.y < 0 || selectedCursor.x < 0) return;
            // The - 1 on the x vale is to make sure it stops at the right edge.
            if (selectedCursor.y >= textMeshCharacterPositions.Count || selectedCursor.x >= textMeshCharacterPositions[0].Count - 1) return;

            Vector2 newPosition = textMeshCharacterPositions[selectedCursor.y][selectedCursor.x];
            uiCursor.SetPositionCenter(newPosition);
        }

        public void MoveView(int rows)
        {
            //int min_x = selectedCursor.GetBounds()[0].x;
            //int min_y = selectedCursor.GetBounds()[0].y;
            //int max_x = selectedCursor.GetBounds()[1].x;
            //int max_y = selectedCursor.GetBounds()[1].y;

            //if (min_y + rows < 0) min_y = 0;
            //if (max_y >= textGrid.GetSize().x) max_y = textGrid.GetSize().x - 1;

            //selectedCursor.SetBounds(min_y: min_y + rows, max_y: max_y + rows);
            verticalViewOffset += rows;
            if (verticalViewOffset < 0) verticalViewOffset = 0;
            if (verticalViewOffset >= textGrid.GetSize().x - RowAmount) verticalViewOffset = textGrid.GetSize().x - RowAmount;
        }
    }

    /// <summary>
    /// Testable monitor class with empty awake.
    /// </summary>
    public class MonitorTestable : Monitor
    {
        private void Awake()
        {
            selectedCursor = defaultCursor;

            cursors.Add(defaultCursor);
        }
    }
}