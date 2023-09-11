using System.Collections.Generic;

namespace Tetris1
{
    /*
     * Abstract class which contains the Data and Form of Blocks aswell as their rotation state and offset
     * Moves and rotates the blocks
     */
    public abstract class Block
    {
        protected abstract BlockPosition[][] Tiles { get; } //Sets the Form of the block in all 4 Rotations
        protected abstract BlockPosition StartOffset { get; }
        public abstract int Id { get; }
        private int rotationState;
        private BlockPosition offset;

        public Block()
        {
            offset = new BlockPosition(StartOffset.Row, StartOffset.Column);

        }


        //Returns the grid position occupied by the block while taking the rotation into account
        public IEnumerable<BlockPosition> TilePositions()
        {
            foreach (BlockPosition bp in Tiles[rotationState])
            {
                yield return new BlockPosition(bp.Row + offset.Row, bp.Column + offset.Column);
            }
        }

        public void RotateClockWise()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public void RotatteCounterClockWise()
        {
            if (rotationState == 0)
                rotationState = Tiles.Length - 1;
            else
                rotationState--;
        }

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }

    }
}
