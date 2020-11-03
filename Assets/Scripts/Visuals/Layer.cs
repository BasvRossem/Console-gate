namespace Visuals
{
    /// <summary>
    /// Layer that can be edited using a cursor, and a view.
    /// Has to be created and rendered by the monitor object.
    /// </summary>
    public class Layer
    {
        public readonly TextGrid textGrid;
        public readonly Cursor cursor;
        public readonly View view;
        public int zIndex;

        private bool _isChanged;

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
        /// <param name="letter">Letter to write</param>
        public void WriteCharacter(char letter)
        {
            while (cursor.position.row >= textGrid.GetSize().rows) textGrid.AddRow();
            
            bool columnOutOfBounds = cursor.position.column >= textGrid.GetSize().columns;
            if (Tools.CheckWarning(columnOutOfBounds , "Cursor is out of bounds. Ignoring character.")) return;

            textGrid[cursor.position.row, cursor.position.column] = letter;
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
        /// <param name="automaticReturn">Whether to go to the next line after writing.</param>
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
                cursor.Move(new GridPosition(0, -1 * _size.columns));
            }
            Change();
        }

        /// <summary>
        /// Clears the screen and sets the Layer text.
        /// </summary>
        /// <param name="newText">Text to write to the Layer.</param>
        /// <param name="automaticReturn">Whether to go to the next line after writing. Default of true.</param>
        public void WriteText(string newText, bool automaticReturn = true)
        {
            cursor.ResetPosition();
            view.SetInternalPosition(new GridPosition(0,0));
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
        /// Remove characters in a certain area.
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
        /// <summary>
        /// Set the text to the view and return the view.
        /// </summary>
        /// <returns>View with rendered text.</returns>
        public View RenderView()
        {
            view.SetText(textGrid);
            return view;
        }

        // Change
        /// <summary>
        /// Change the isChanged variable.
        /// </summary>
        /// <param name="changed">A boolean value to set the isChanged variable to.</param>
        public void Change(bool changed = true)
        {
            _isChanged = changed;
        }

        /// <summary>
        /// Returns if the view of anything on this layer has changed.
        /// </summary>
        /// <returns>If the view of anything on this layer has changed.</returns>
        public bool HasChanged()
        {
            return (_isChanged || view.HasChanged());
        }
    }
}