namespace Tetris1
{
    /*
     Constructor class for the Position of a Block
     */
    public class BlockPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public BlockPosition(int row, int column)        {
            Row = row;
            Column = column;
        }

    }
}
