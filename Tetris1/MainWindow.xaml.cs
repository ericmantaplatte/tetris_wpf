using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[] //Saves tile images in array for fast nonreadable access
        {
            new BitmapImage(new System.Uri("dings/TileEmpty.png", System.UriKind.Relative)),    //Empty Tile Image
            new BitmapImage(new System.Uri("dings/TileCyan.png", System.UriKind.Relative)),     //IBlock Tile Image
            new BitmapImage(new System.Uri("dings/TileBlue.png", System.UriKind.Relative)),     //JBlock Tile Image
            new BitmapImage(new System.Uri("dings/TileOrange.png", System.UriKind.Relative)),   //LBlock Tile Image
            new BitmapImage(new System.Uri("dings/TileYellow.png", System.UriKind.Relative)),   //OBlock Tile Image
            new BitmapImage(new System.Uri("dings/TileGreen.png", System.UriKind.Relative)),    //SBlock Tile Image
            new BitmapImage(new System.Uri("dings/TilePurple.png", System.UriKind.Relative)),   //TBlock TileImage
            new BitmapImage(new System.Uri("dings/TileRed.png", System.UriKind.Relative)),      //ZBlock Tile Image
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new System.Uri("dings/Block-Empty.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-I.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-J.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-L.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-O.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-S.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-T.png", System.UriKind.Relative)),
            new BitmapImage(new System.Uri("dings/Block-Z.png", System.UriKind.Relative)),
        }; //Saves block images in array for fast nonreadable access

        private readonly Image[,] imageControls;
        private GameState gameState = new();
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            var myGrid = (Canvas)this.FindName("GameCanvas");
            int cellSize = Convert.ToInt32(myGrid.Width / 10);

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 15);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1; //Needed to Reset from Preview
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block b)
        {
            foreach (BlockPosition bp in b.TilePositions())
            {
                imageControls[bp.Row, bp.Column].Source = tileImages[b.Id];

            }
        }

        private async Task Update()
        {
            Draw(gameState);
            while (!gameState.GameOver)
            {
                await Task.Delay(gameState.gameSpeed);
                gameState.MoveBlockDown();
                Draw(gameState);
                //this.ScoreTx.Text = gameState.gameSpeed.ToString();
            }
            GameOverScreen.Visibility = Visibility.Visible;
        }

        private void DrawHeldBlock(Block heldblock)
        {
            if (heldblock == null)
                HoldImage.Source = blockImages[0];
            else
                HoldImage.Source = blockImages[heldblock.Id];
        }

        private void DrawDropPreview(Block b)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach (BlockPosition bp in b.TilePositions())
            {
                imageControls[bp.Row + dropDistance, bp.Column].Opacity = 0.33;
                imageControls[bp.Row + dropDistance, bp.Column].Source = tileImages[b.Id];
            }
        }

        //Draws Grid, Block, Queue, Held Block and Preview
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.Queue);
            DrawHeldBlock(gameState.HeldBlock);
            DrawDropPreview(gameState.CurrentBlock);
            ScoreTx.Text = gameState.Score.ToString();
        }

        private void DrawNextBlock(BlockQueue bq)
        {
            Block next = bq.NextBlock;
            NextBlockImage.Source = blockImages[next.Id];
        }

        //EventHandler for Key Inputs
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
                return;
            switch (e.Key)
            {
                case Key.Up:
                    if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) //if shift key is held during up press
                        gameState.RotateBlockCounterClockWise();
                    else
                        gameState.RotateBlockClockWise();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown(); break;
                case Key.Right:
                    gameState.MoveRight(); break;
                case Key.Left:
                    gameState.MoveLeft(); break;
                case Key.Enter:
                    gameState.HoldBlock(); break;
                case Key.Space:
                    gameState.DropBlock(); break;
                default: return;
            }
            Draw(gameState);
        }


        //waits for the game to load and starts the game loop
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await Update();
        }



        //Creates a new Grid and waits for the game to start again
        private async void Retry_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverScreen.Visibility = Visibility.Hidden;
            await Update();


        }
    }
}
