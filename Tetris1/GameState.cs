namespace Tetris1
{
    public class GameState
    {
        public int gameSpeed = 500;
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                //For loop so block doesnt spawn in the 2 hidden Rows
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                        currentBlock.Move(-1, 0);
                }
            }
        }

        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameGrid GameGrid { get; set; }
        public BlockQueue Queue { get; set; }
        public bool GameOver { get; private set; }

        public int Score { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            Queue = new BlockQueue();
            CurrentBlock = Queue.GetAndUpdate();
            CanHold = true;
        }


        //Checks if Position is possible
        private bool BlockFits()
        {
            foreach (BlockPosition bp in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(bp.Row, bp.Column))
                    return false;
            }
            return true;
        }


        public void RotateBlockClockWise()
        {
            currentBlock.RotateClockWise();
            if (!BlockFits())
            {
                currentBlock.RotatteCounterClockWise();
            }
        }
        public void RotateBlockCounterClockWise()
        {
            currentBlock.RotatteCounterClockWise();
            if (!BlockFits())
            {
                currentBlock.RotateClockWise();
            }
        }

        public void MoveLeft()
        {
            currentBlock.Move(0, -1);
            if (!BlockFits())
            {
                currentBlock.Move(0, 1);
            }
        }

        public void MoveRight()
        {
            currentBlock.Move(0, 1);
            if (!BlockFits())
            {
                currentBlock.Move(0, -1);
            }
        }

        private bool CheckGameOver()
        {
            return !(GameGrid.isRowEmpty(0) && GameGrid.isRowEmpty(1)); //Returns true if first hidden 2 Rows are not empty
        }


        //Increases Speed for every cleared row
        private void IncreaseSpeed(int clearedRows)
        {
            if (gameSpeed > 100)
                gameSpeed -= (clearedRows * 25);
            if (gameSpeed <= 100)
                gameSpeed = 100;
        }


        //Standard Tetris Score increase
        private void IncreaseScore(int clearedRows)
        {
            switch (clearedRows)
            {
                case 0: break;
                case 1:
                    Score += 100; break;
                case 2:
                    Score += 300; break;
                case 3:
                    Score += 500; break;
                case 4:
                    Score += 800; break;
                default:
                    Score += 800 + clearedRows * 200;
                    break;
            }

        }
        private void PlaceBlock()
        {
            foreach (BlockPosition bp in CurrentBlock.TilePositions())
            {
                GameGrid[bp.Row, bp.Column] = CurrentBlock.Id;

            }
            IncreaseSpeedAndScore(GameGrid.ClearFullRows());

            if (CheckGameOver())
                GameOver = true;
            else
            {
                CurrentBlock = Queue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        private void IncreaseSpeedAndScore(int clearedRows)
        {
            IncreaseSpeed(clearedRows);
            IncreaseScore(clearedRows);
        }


        /*
         * Checks if you can hold a block stops method if no
         * if you are not holding a block, hold current block
         * if you are holding a block, swaps current block with current block
         */
        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }


            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = Queue.GetAndUpdate();
            }
            else
            {
                Block tempBlock = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tempBlock;
            }
            CanHold = false;
        }


        //Calculates the distance the tiles can drop
        private int TileDropDistance(BlockPosition bp)
        {
            int drop = 0;
            while (GameGrid.IsEmpty(bp.Row + drop + 1, bp.Column))
                drop++;

            return drop;
        }


        //Calculates the lower value of grid rows and tiledropdistance
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;
            foreach (BlockPosition bp in CurrentBlock.TilePositions())
                drop = System.Math.Min(drop, TileDropDistance(bp));

            return drop;
        }


        //Method to instantly drop and place the block
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
