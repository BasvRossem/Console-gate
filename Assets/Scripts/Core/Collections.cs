using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Used to improve the readability of the code concerning the amount of rows and columns.
/// </summary>
public struct GridSize
{
    public int rows { get; set; }
    public int columns { get; set; }

    public GridSize(int rows = 0, int columns = 0)
    {
        this.rows = rows;
        this.columns = columns;
    }

    public override string ToString()
    {
        return $"{rows}, {columns}";
    }
}

/// <summary>
/// Used to improve the readability of the code concerning the position in regards to a row and column.
/// </summary>
public struct GridPosition
{
    public int row { get; set; }
    public int column { get; set; }

    public GridPosition(int row = 0, int column = 0)
    {
        this.row = row;
        this.column = column;
    }

    public override string ToString()
    {
        return $"{row}, {column}";
    }

    public static GridPosition operator +(GridPosition left, GridPosition right)
    {
        return new GridPosition(left.row + right.row, left.column + right.column);
    }
}