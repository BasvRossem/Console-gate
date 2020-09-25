﻿using System;
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

    private Vector2Int bounds;
    public Vector2Int position;
    public int x;
    public int y;

    public Cursor(int x = 0, int y = 0, int bounds_x = int.MaxValue, int bounds_y = int.MaxValue)
    {
        SetPosition(x, y);
        SetBounds(bounds_x, bounds_y);
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

    public void SetBounds(int bounds_x = int.MaxValue, int bounds_y = int.MaxValue)
    {
        bounds = new Vector2Int(bounds_x, bounds_y);
    }

    public void Move(Vector2Int direction)
    {
        position += direction;

        // If the curser is out of bounds, put it in bounds
        if (position.x < 0) position.x = 0;
        if (position.x > bounds.x) position.x = bounds.x;
        if (position.y < 0) position.y = 0;
        if (position.y > bounds.y) position.y = bounds.y;

        UpdateXY();
    }
}

public class TextGrid
{
    private int columnAmount;
    private int rowAmount;

    private List<char[]> grid;

    private char[] MakeArray(int length, char character)
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
        for (int row = 0; row < rowAmount; row++) grid.Add(MakeArray(columnAmount, ' '));
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
        grid[index] = new char[columnAmount];
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
        cursor = new Cursor();
        cursor.SetBounds(ColumnAmount, RowAmount);
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
        cursor.SetPosition(0, cursor.y);
        cursor.Move(cursor.Down);
    }

    public void RemoveMonitorTextLineAtPosition(int index)
    {
        if (checkError(index < 0, string.Format("Index {0} cannot be negative.", index))) return;
        if (checkError(index > RowAmount - 1, string.Format("Index {0} is higher than lines on the monitor.", index))) return;

        textGrid.Clear(index);
    }

    private bool checkError(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogError(errorMessage);
        }
        return condition;
    }
}