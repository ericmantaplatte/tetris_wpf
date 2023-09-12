using System;
/*
 * Class that generates a new block that is placed after the first block finishes
 */
namespace Tetris1
{
    public class BlockQueue
    {

        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random rdm = new();
        public Block NextBlock { get; set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }
        private Block RandomBlock()
        {
            return blocks[rdm.Next(blocks.Length)];
        }

        //adds a different block into the queue
        public Block GetAndUpdate()
        {
            Block block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);
            return block;
        }

    }
}
