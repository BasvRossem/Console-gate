using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public class View
    {
        public TextGrid textGrid;

        public GridSize size;
        public GridPosition monitorPosition;
        public GridPosition startPosition;

        private bool _isChanged;

        public View(GridSize size, GridPosition startPosition = new GridPosition(), GridPosition monitorPosition = new GridPosition())
        {
            this.size = size;
            this.startPosition = startPosition;
            this.monitorPosition = monitorPosition;

            Change();
        }

        // Size
        public void SetSize(int rows, int columns)
        {
            size = new GridSize(rows, columns);
            Change();
        }

        public void Grow(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            monitorPosition.row -= up;
            monitorPosition.column -= left;

            size.rows += down + up;
            size.columns += left + right;
            Change();
        }

        // Positioning
        public void SetStartPosition(int row, int column)
        {
            startPosition = new GridPosition(row, column);
            Change();
        }

        public void SetMonitorPosition(int row, int column)
        {
            monitorPosition = new GridPosition(row, column);
            Change();
        }

        public void Move(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            monitorPosition.row += up + down;
            monitorPosition.column += left + right;

            Change();
        }

        // Rendering
        public void SetText(TextGrid externalTextGrid)
        {
            textGrid = new TextGrid(size);

            int endRow = startPosition.row + size.rows - 1;
            int endColumn = startPosition.column + size.columns - 1;

            for (int row = startPosition.row; row <= endRow; row++)
            {
                for (int column = startPosition.column; column <= endColumn; column++)
                {
                    char character = externalTextGrid[row, column];

                    int textRow = row - startPosition.row;
                    int textColumn = column - startPosition.column;

                    textGrid[textRow, textColumn] = character;
                }
            }
            Change();
        }

        // Change
        public void Change(bool changed = true)
        {
            _isChanged = changed;
        }

        public bool HasChanged()
        {
            return _isChanged;
        }
    }
}