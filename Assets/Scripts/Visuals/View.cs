namespace Visuals
{
    /// <summary>
    /// A view is used by a layer to regulate what will eventually be rendered on the monitor.
    /// </summary>
    public class View
    {
        public TextGrid textGrid;

        public GridSize size;
        public GridPosition externalPosition;
        public GridPosition internalPosition;

        private bool _isChanged;

        public View(GridSize size, GridPosition internalPosition = new GridPosition(), GridPosition externalPosition = new GridPosition())
        {
            SetSize(size);
            SetInternalPosition(internalPosition);
            SetExternalPosition(externalPosition);

            Change();
        }

        // Size
        /// <summary>
        /// Set a new size for the view.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        public void SetSize(GridSize newSize)
        {
            if (Tools.CheckError((newSize.rows <= 0) || (newSize.columns <= 0), "Size cannot be negative or zero.")) return;
            size = newSize;
            Change();
        }

        /// <summary>
        /// Grow the view in a (combination of) direction(s).
        /// </summary>
        /// <param name="up">How much to grow the view in the up direction.</param>
        /// <param name="down">How much to grow the view in the down direction.</param>
        /// <param name="left">How much to grow the view in the left direction.</param>
        /// <param name="right">How much to grow the view in the right direction.</param>
        public void Grow(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            internalPosition.row -= up;
            internalPosition.column -= left;

            size.rows += up + down;
            size.columns += left + right;
            Change();
        }

        // Positioning
        /// <summary>
        /// Set the new top left position of the view relative to its internal text grid.
        /// </summary>
        /// <param name="newInternalPosition">The new top left position.</param>
        public void SetInternalPosition(GridPosition newInternalPosition)
        {
            if (Tools.CheckError(newInternalPosition.row < 0 || newInternalPosition.column < 0, "Internal position cannot be negative.")) return;
            
            internalPosition = newInternalPosition;
            Change();
        }

        /// <summary>
        /// Set the new top left position of the view on the monitor. 
        /// </summary>
        /// <param name="newMonitorPosition">New top left position on the monitor.</param>
        public void SetExternalPosition(GridPosition newMonitorPosition)
        {
            if (Tools.CheckError(newMonitorPosition.row < 0 || newMonitorPosition.column < 0, "External position cannot be negative.")) return;
            
            externalPosition = newMonitorPosition;
            Change();
        }

        /// <summary>
        /// Move the internal view in (a) certain direction(s).
        /// </summary>
        /// <param name="up">How much to move the internal view up.</param>
        /// <param name="down">How much to move the internal view down.</param>
        /// <param name="left">How much to move the internal view left.</param>
        /// <param name="right">How much to move the internal view right.</param>
        public void MoveInternalPosition(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            if (Tools.CheckError((up < 0 || down < 0 || left < 0 || right < 0), "Cannot move in a negative direction.")) return;
            
            internalPosition.row += down - up;
            internalPosition.column += right - left;

            if (Tools.CheckWarning(internalPosition.row < 0, "New view internal position row cannot be negative. Set to 0.")) internalPosition.row = 0;
            if (Tools.CheckWarning(internalPosition.column < 0, "New view internal position column cannot be negative. Set to 0.")) internalPosition.column = 0;
            
            Change();
        }

        // Rendering
        /// <summary>
        /// Make a 2d slice of the text grid using the internal position of the view to this grid, and the size of the view.
        /// </summary>
        /// <param name="externalTextGrid">The external grid to take a slice out of.</param>
        public void SetText(TextGrid externalTextGrid)
        {
            textGrid = new TextGrid(size);

            int endRow = internalPosition.row + size.rows - 1;
            int endColumn = internalPosition.column + size.columns - 1;

            for (int row = internalPosition.row; row <= endRow; row++)
            {
                for (int column = internalPosition.column; column <= endColumn; column++)
                {
                    char character = externalTextGrid[row, column];

                    int textRow = row - internalPosition.row;
                    int textColumn = column - internalPosition.column;

                    textGrid[textRow, textColumn] = character;
                }
            }

            Change();
        }

        // Change
        /// <summary>
        /// Set the isChanged variable so that the monitor renderer knows if anything has changed.
        /// </summary>
        /// <param name="changed">The boolean value to set the isChanged variable to.</param>
        public void Change(bool changed = true)
        {
            _isChanged = changed;
        }

        /// <summary>
        /// Returns whether the view has been changed. 
        /// </summary>
        /// <returns>A boolean value whether the view or its underlying data has changed.</returns>
        public bool HasChanged()
        {
            return _isChanged;
        }
    }
}