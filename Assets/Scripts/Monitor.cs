using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

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

    public Cursor(int x = 0, int y = 0)
    {
        SetPosition(x, y);
        SetBounds();
        UpdateXY();
    }

    private void UpdateXY()
    {
        x = position.x;
        y = position.y;
    }

    public void SetPosition(int x = 0, int y = 0)
    {
        position = new Vector2Int(x, y);
    }

    public void SetBounds(int min_x = 0, int max_x = int.MaxValue, int min_y = 0, int max_y = int.MaxValue)
    {
        min_bounds = new Vector2Int(min_x, min_y);
        max_bounds = new Vector2Int(max_x, max_y);
    }

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
}

public class TextGrid
{
    private int columnAmount;
    private int rowAmount;

    private List<char[]> grid;

    private char[] MakeRow(int length, char character)
    {
        char[] arr = new char[length];
        for (int i = 0; i < length; i++)
        {
            arr[i] = character;
        }
        return arr;
    }

    public TextGrid(int RowAmount, int ColumnAmount)
    {
        rowAmount = RowAmount;
        columnAmount = ColumnAmount;

        grid = new List<char[]>(rowAmount);
        for (int row = 0; row < rowAmount; row++) grid.Add(MakeRow(columnAmount, ' '));
    }

    public char this[int index_row, int index_column]
    {
        get => grid[index_row][index_column];
        set => grid[index_row][index_column] = value;
    }

    public char[] this[int index_row]
    {
        get => grid[index_row];
        set => grid[index_row] = value;
    }

    public Vector2Int GetSize()
    {
        return new Vector2Int(grid.Count, grid[0].Length);
    }

    public void Clear(int index)
    {
        grid[index] = MakeRow(columnAmount, ' ');
    }
}

public class Monitor : MonoBehaviour
{
    private TextMeshProUGUI monitor;

    private const int RowAmount = 24;
    private const int ColumnAmount = 80;
    private TextGrid textGrid;

    private Cursor cursor;

    private string text;

    private void Start()
    {
        monitor = GetComponent<TextMeshProUGUI>();
        ResetMonitor();
    }

    private void Update()
    {
        ResetMonitor();
        // System.DateTime.Now.ToString()

        DrawLineVertical(0, 0, RowAmount);
        DrawLineVertical(ColumnAmount - 1, 0, RowAmount);
        DrawLineHorizontal(0, 1, ColumnAmount - 1);
        DrawLineHorizontal(RowAmount - 1, 1, ColumnAmount - 1);

        cursor = new Cursor();
        cursor.SetBounds(1, ColumnAmount - 1, 1, RowAmount - 1);

        // Move cursor to top left
        cursor.Move(new Vector2Int(-1 * ColumnAmount, -1 * RowAmount));

        AddMonitorTextLine("|--------------------|");
        AddMonitorTextLine("| Hello World        |");
        AddMonitorTextLine("| How are you doing? |");
        AddMonitorTextLine("| Im fine!           |");
        AddMonitorTextLine("|----------:D----------|");

        RenderMonitorText();
    }

    public void WriteCharacter(char letter)
    {
        textGrid[cursor.y, cursor.x] = letter;
        cursor.Move(cursor.Right);
    }

    private void RenderMonitorText()
    {
        text = "";

        for (int y = 0; y < RowAmount; y++)
        {
            text += new string(textGrid[y]);
            text += "\n";
        }
        monitor.SetText(text);
    }

    public void ResetMonitor()
    {
        textGrid = new TextGrid(RowAmount, ColumnAmount);
    }

    public void SetMonitorText(string newText)
    {
        ResetMonitor();
        AddMonitorTextLine(newText);
    }

    public void AddMonitorTextLine(string newTextLine)
    {
        foreach (char character in newTextLine)
        {
            WriteCharacter(character);
        }

        cursor.Move(cursor.Down);
        cursor.Move(new Vector2Int(-1 * ColumnAmount, 0));
    }

    public void RemoveMonitorTextLineAtPosition(int index)
    {
        if (CheckError(index < 0, string.Format("Index {0} cannot be negative.", index))) return;
        if (CheckError(index > RowAmount - 1, string.Format("Index {0} is higher than lines on the monitor.", index))) return;

        textGrid.Clear(index);
    }

    public void DrawLineHorizontal(int row, int startColumn, int endColumn)
    {
        for (int column = startColumn; column < endColumn; column++)
        {
            textGrid[row][column] = '-';
        }
    }

    public void DrawLineVertical(int column, int startRow, int endRow)
    {
        for (int row = startRow; row < endRow; row++)
        {
            textGrid[row][column] = '|';
        }
    }

    private bool CheckError(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogError(errorMessage);
        }
        return condition;
    }
}