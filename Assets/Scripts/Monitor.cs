using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is a cursor which the monitor class uses to write to the text grid.
/// </summary>
public class Cursor
{
    public Vector2Int Left = new Vector2Int(-1, 0);
    public Vector2Int Right = new Vector2Int(1, 0);
    public Vector2Int Up = new Vector2Int(0, -1);
    public Vector2Int Down = new Vector2Int(0, 1);

    private Vector2Int min_bounds;
    private Vector2Int max_bounds;
    public Vector2Int position;
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
    public Cursor(int x = 0, int y = 0, string cursorName = Cursor.DefaultName)
    {
        SetPosition(x, y);
        SetBounds();
        UpdateXY();

        name = cursorName;
    }

    /// <summary>
    /// Get the name of the cursor.
    /// </summary>
    /// <returns>Cursor name.</returns>
    public string GetName()
    {
        return name;
    }

    /// <summary>
    /// Set the bounds in which the cursor can write.
    /// </summary>
    /// <param name="min_x">Minimal x value.</param>
    /// <param name="max_x">Maximal x value.</param>
    /// <param name="min_y">Minimal y value.</param>
    /// <param name="max_y">Maximal y value.</param>
    public void SetBounds(int min_x = 0, int max_x = int.MaxValue, int min_y = 0, int max_y = int.MaxValue)
    {
        min_bounds = new Vector2Int(min_x, min_y);
        max_bounds = new Vector2Int(max_x, max_y);
    }

    // Positioning
    /// <summary>
    /// Update the public x and y positioning.
    /// </summary>
    private void UpdateXY()
    {
        x = position.x;
        y = position.y;
    }

    /// <summary>
    /// Set a new position of the cursor.
    /// </summary>
    /// <param name="x">New x position. Default of 0.</param>
    /// <param name="y">New y position. Default of 0.</param>
    public void SetPosition(int x = 0, int y = 0)
    {
        position = new Vector2Int(x, y);
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

        // If the curser is out of bounds, put it in bounds
        if (position.x < min_bounds.x) position.x = min_bounds.x;
        if (position.x > max_bounds.x) position.x = max_bounds.x;

        if (position.y < min_bounds.y) position.y = min_bounds.y;
        if (position.y > max_bounds.y) position.y = max_bounds.y;

        UpdateXY();
    }

    /// <summary>
    /// Move the Cursor to the top left within its bounds.
    /// </summary>
    public void ResetPosition()
    {
        Move(new Vector2Int(int.MinValue, int.MinValue));
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
        for (int row = 0; row < rowAmount; row++) grid.Add(MakeRow(columnAmount, ' '));
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

/// <summary>
/// This class is used to show text on the screen.
/// </summary>
public class Monitor : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private const int RowAmount = 24;
    private const int ColumnAmount = 80;
    private TextGrid textGrid;

    private Cursor defaultCursor;
    public Cursor selectedCursor;
    public List<Cursor> cursors;
    public UICursor uiCursor;

    private string text;

    private void Start()
    {
        defaultCursor = new Cursor();
        selectedCursor = defaultCursor;

        cursors = new List<Cursor>();
        cursors.Add(defaultCursor);

        ResetMonitor();
    }

    private void Update()
    {
        RenderMonitorText();
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
    private Cursor FindCursor(string name)
    {
        IEnumerable<Cursor> foundCursors = cursors.Where(cursor => cursor.GetName() == name);
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
        Cursor newSelected = FindCursor(name);
        if (Tools.CheckError(newSelected == null, string.Format("No cursor found with name \"{0}\"", name))) return false;

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

        Cursor newCursor = new Cursor(cursorName: name);
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
        Cursor cursorToRemove = FindCursor(name);
        if (Tools.CheckError(cursorToRemove == null, string.Format("No cursor found with name \"{0}\"", name))) return false;
        cursors.Remove(cursorToRemove);
        return true;
    }

    // Writing to the monitor
    /// <summary>
    /// Write a single character to the monitor and move the cursor accordingly.
    /// </summary>
    /// <param name="letter"></param>
    private void WriteCharacter(char letter)
    {
        NullCoalesceSelectedCursor();
        textGrid[selectedCursor.y, selectedCursor.x] = letter;
        selectedCursor.Move(selectedCursor.Right);
    }

    /// <summary>
    /// Turn the text grid into strings to write to the monitor.
    /// </summary>
    private void AssembleText()
    {
        text = "";

        for (int y = 0; y < RowAmount; y++)
        {
            text += new string(textGrid[y]);
            text += "\n";
        }
    }

    /// <summary>
    /// Show the text to the screen.
    /// </summary>
    public void RenderMonitorText()
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
        ResetMonitor();
        AddMonitorTextLine(newText);
    }

    /// <summary>
    /// Add text at the current place of the cursor.
    /// </summary>
    /// <note>
    /// This moves the cursor one line down automatically when the line is written.
    /// </note>
    /// <param name="newTextLine">Text to place.</param>
    public void AddMonitorTextLine(string newTextLine)
    {
        foreach (char character in newTextLine)
        {
            WriteCharacter(character);
        }

        NullCoalesceSelectedCursor();
        selectedCursor.Move(selectedCursor.Down);
        selectedCursor.Move(new Vector2Int(-1 * ColumnAmount, 0));
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
    /// <param name="startRow">Index of the row where the box should start.</param>
    /// <param name="startColumn">Index of the column where the box should start.</param>
    /// <param name="endRow">Index of the row where the box should end.</param>
    /// <param name="endColumn">Index of the column where the box should end.</param>
    public void DrawRectangle(int startRow, int startColumn, int endRow, int endColumn)
    {
        // Draw lines
        DrawLineHorizontal(startRow, startColumn + 1, endColumn);
        DrawLineHorizontal(endRow, startColumn + 1, endColumn);
        DrawLineVertical(startColumn, startRow + 1, endRow);
        DrawLineVertical(endColumn, startRow + 1, endRow);

        // Draw Corners
        textGrid[startRow, startColumn] = '*';
        textGrid[startRow, endColumn] = '*';
        textGrid[endRow, startColumn] = '*';
        textGrid[endRow, endColumn] = '*';
    }

    // Monitor information
    /// <summary>
    /// Get the amount of characters the monitor is high.
    /// </summary>
    /// <returns></returns>
    public int GetRowAmount()
    {
        return RowAmount;
    }

    /// <summary>
    /// Get the amount of characters the monitor is wide.
    /// </summary>
    /// <returns></returns>
    public int GetColumnAmount()
    {
        return ColumnAmount;
    }

    // UI Cursor
    /// <summary>
    /// Move the selection of the UI cursor one to the right.
    /// </summary>
    public void moveUICursorRight()
    {
        TMP_CharacterInfo characterInfo = textMesh.textInfo.characterInfo[0];
        Vector2 newPosition = (characterInfo.topLeft + characterInfo.bottomRight) / 2;
        Vector2 meshPosition = textMesh.transform.position;
        uiCursor.SetPositionCenter(newPosition + meshPosition);
    }

    /// <summary>
    /// Select a row on the monitor iusing the UI cursor.
    /// </summary>
    /// <param name="row">The index of the row to be selected</param>
    public void SelectRow(int row)
    {
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
}