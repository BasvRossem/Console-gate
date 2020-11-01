using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public class View
    {
        public TextGrid textGrid;

        public Vector2Int size;
        public Vector2Int monitorPosition;

        public int startRow;
        public int startColumn;

        public View(int width, int height, int startX = 0, int startY = 0, int positionX = 0, int positionY = 0)
        {
            size = new Vector2Int(width, height);
            startRow = startX;
            startColumn = startY;
            monitorPosition = new Vector2Int(positionX, positionY);
        }

        // Size
        public void SetSize(int rows, int columns)
        {
            size = new Vector2Int(columns, rows);
        }

        public void Grow(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            monitorPosition.y -= up;
            monitorPosition.x -= left;

            size.y += down + up;
            size.x += left + right;
        }

        // Positioning
        public void SetPosition(int row, int column)
        {
            monitorPosition = new Vector2Int(column, row);
        }

        public void Move(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            monitorPosition.y += up + down;
            monitorPosition.x += left + right;
        }

        // Rendering
        public void SetText(TextGrid externalTextGrid)
        {
            textGrid = new TextGrid(size.y, size.x);

            int endRow = startRow + size.y - 1;
            int endColumn = startColumn + size.x - 1;

            for (int row = startRow; row <= endRow; row++)
            {
                for (int column = startColumn; column <= endColumn; column++)
                {
                    var character = externalTextGrid[row, column];

                    var textRow = row - startRow;
                    var textColumn = column - startColumn;

                    textGrid[textRow, textColumn] = character;
                }
            }
        }
    }
}