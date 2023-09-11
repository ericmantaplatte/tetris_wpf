namespace Tetris1
{
    public class LBlock : Block
    {
        private readonly BlockPosition[][] tiles = new BlockPosition[][]
{
            new BlockPosition[] {new(0,2), new(1,0), new(1,1), new(1,2)},
            new BlockPosition[] {new(0,1), new (1,1), new(2,1), new(2,2)},
            new BlockPosition[] {new(1,0), new (1,1), new(1,2), new(2,0)},
            new BlockPosition[] {new(0,0), new (0,1), new(1,1), new(2,1)}
};

        public override int Id => 3;
        protected override BlockPosition StartOffset => new BlockPosition(0, 3);
        protected override BlockPosition[][] Tiles => tiles;

    }
}
