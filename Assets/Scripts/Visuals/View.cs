namespace Visuals
{
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
        public void SetSize(GridSize newSize)
        {
            if (Tools.CheckError((newSize.rows <= 0) || (newSize.columns <= 0), "Size cannot be negative or zero.")) return;
            size = newSize;
            Change();
        }

        public void Grow(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            internalPosition.row -= up;
            internalPosition.column -= left;

            size.rows += up + down;
            size.columns += left + right;
            Change();
        }

        // Positioning
        public void SetInternalPosition(GridPosition newInternalPosition)
        {
            if (Tools.CheckError(newInternalPosition.row < 0 || newInternalPosition.column < 0, "Internal position cannot be negative.")) return;
            
            internalPosition = newInternalPosition;
            Change();
        }

        public void SetExternalPosition(GridPosition newMonitorPosition)
        {
            if (Tools.CheckError(newMonitorPosition.row < 0 || newMonitorPosition.column < 0, "External position cannot be negative.")) return;
            
            externalPosition = newMonitorPosition;
            Change();
        }

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