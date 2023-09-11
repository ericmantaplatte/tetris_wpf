namespace Tetris1
{
    public class IBlock : Block
    {
        private readonly BlockPosition[][] tiles = new BlockPosition[][]
        {
            new BlockPosition[] {new(1,0), new(1,1), new(1,2), new(1,3)},
            new BlockPosition[] {new(0,2), new (1,2), new(2,2), new(3,2)},
            new BlockPosition[] {new(2,0), new (2,1), new(2,2), new(2,3)},
            new BlockPosition[] {new(0,1), new (1,1), new(2,1), new(3,1)}
        };

        public override int Id => 1;
        protected override BlockPosition StartOffset => new(-1, 3);
        protected override BlockPosition[][] Tiles => tiles;
    }
}
