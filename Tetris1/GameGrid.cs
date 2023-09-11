namespace Tetris1
{
    /*
     * Creates and handels the game grid
     *      First 2 Rows in the Grid are Hidden
     *      Checks if rows and columns exist in the game grid
     *      Checks if the rows are full or contain and empty column
     *      Deletes and moves the full columns down.
     */
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool isInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns; //Returns true if row and column is inside the Grid
        }

        public bool IsEmpty(int r, int c)
        {
            return isInside(r, c) && grid[r, c] == 0; //Checks if a row or column is inside the Grid and Empty
        }


        //Checks if row contains an empty column
        public bool isRowFull(int r)
        {

            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                    return false;
            }
            return true;
        }


        //Checks if row contains atleast 1 filled column
        public bool isRowEmpty(int r)
        {

            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                    return false;
            }
            return true;
        }


        //Sets the entire row to empty
        public void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        //Moves Row down by the amount of cleared rows
        public void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }


        //Clears all full rows on the  Grid and returns amount of cleared Rows
        public int ClearFullRows()
        {
            int cleared = 0;
            for (int r = Rows - 1; r >= 0; r--)
            {
                if (isRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }
    }
}
