using System.CodeDom;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    public enum Player {
        X,
        O
    }
    public partial class MainWindow : Window
    {
        Player player;
        Button button;
        private List<Button> buttons = new List<Button>();
        private string[,] gameboard = new string[,] { { " ", " ", " " },{ " ", " ", " " }, { " ", " ", " " } };
        const string X = "X";
        const string O = "O";
        private int col;
        private int row;
        private bool isGameOver = false;

        public MainWindow()
        {
            InitializeComponent();
        }
        /*
         * Start the game
         * Player must chose "playX" or "playO" before this button is enable
         * Make GameGrid + reset-button visible
         * Add all buttons from gamegrid in a list(gamelist)
         */
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            startBtn.IsEnabled = false;
            GameGrid.Visibility = Visibility.Visible;
            resetBtn.Visibility = Visibility.Visible;
            startBtn.Visibility = Visibility.Hidden;
            playO.Visibility = Visibility.Hidden;
            playX.Visibility = Visibility.Hidden;
            Choose.Visibility = Visibility.Hidden;
            foreach (Button b in GameGrid.Children)
            {
                buttons.Add(b);
            }
        }
        /*
         * Player chose X as symbol
         */
        private void playX_Click(object sender, RoutedEventArgs e)
        {
            startBtn.IsEnabled = true;
            playO.IsEnabled = false;
            player = Player.X;
        }
        /*
         * Player chose O as symbol
         */
        private void playO_Click(object sender, RoutedEventArgs e)
        {
            startBtn.IsEnabled = true;
            playX.IsEnabled = false;
            player = Player.O;
        }

        /*
         * When a button in GameGrid is clicked on
         * The button content & gameboard-place becomes the players symbol, X or O
         * and its removed from the game-list.
         * 
         * The computer makes its move
         * 
         */
        private void ButtonClick(object sender, RoutedEventArgs e) {
            button = (Button)sender;
            buttons.Remove(button); // gamelist updates
            if (!isGameOver)
            {
                // button&gameboard updates
                string playerSymbol = (player == Player.X) ? X : O;
                button.Content = playerSymbol;
                button.IsEnabled = false;
                col = Grid.GetColumn(button);
                row = Grid.GetRow(button);
                gameboard[col, row] = playerSymbol;
                // checks the board for a Win and/or isBoardFull
                isGameOver = CheckBoard();
                // if false -> Computers move. else if true -> messagebox of victory
                if (!isGameOver)
                    ComputerMove(player);
                else
                    MessageBox.Show($"{playerSymbol} Won!!");
            }

        }

        /*
         * Returns true if game is over
         * Returns false if game can continue
         */
        private bool CheckBoard() {
            // Checks Horizontal & Vertical
            for (int i = 0; i < 3; i++)
            {
                if (gameboard[i, 0] == gameboard[i, 1] && gameboard[i, 1] == gameboard[i, 2] && gameboard[i, 0] != " ")
                    return true;
                if (gameboard[0, i] == gameboard[1, i] && gameboard[1, i] == gameboard[2, i] && gameboard[0, i] != " ")
                    return true;
            }

            // Checks Diagonal
            if (gameboard[0, 0] == gameboard[1, 1] && gameboard[1, 1] == gameboard[2, 2] && gameboard[0, 0] != " ")
                return true;
            if (gameboard[0, 2] == gameboard[1, 1] && gameboard[1, 1] == gameboard[2, 0] && gameboard[0, 2] != " ")
                return true;

            // Checks if board is full
            if (isBoardFull())
            {
                MessageBox.Show("It's a draw! Nobody won!");
                isGameOver = true;
                return false;
            }

            return false; // Continue play
        }

        /*
         * Return true if board is full
         */
        private bool isBoardFull() {
            if (buttons.Count == 0) {
                return true;
            }
            return false;
        }

        /*
         * Computer makes a move.
         * Using FindWinningMove() to "block" Player
         */
        private void ComputerMove(Player currentPlayer) {
            if (isGameOver) return;
            // Gets player and computer symbols, "X" or "O"
            string computerSymbol = currentPlayer == Player.X ? O : X;
            string playerSymbol = currentPlayer == Player.X ? X : O;
            Button bestButton = null;
            //Simulates a move to check a win for Computer
            bestButton = FindWinningMove(computerSymbol);
            //Simulates a move to check a win for Computer
            if (bestButton == null)
                bestButton = FindWinningMove(playerSymbol);

            // No wins found, put move on first avaiable button
            if (bestButton == null)
                bestButton = buttons.FirstOrDefault();

            // Found a win in simulator
            if (bestButton != null)
            {
                bestButton.Content = computerSymbol;
                bestButton.IsEnabled = false;
                col = Grid.GetColumn(bestButton);
                row = Grid.GetRow(bestButton);
                gameboard[col, row] = computerSymbol;
                buttons.Remove(bestButton);
                isGameOver = CheckBoard();
                if (isGameOver)
                    MessageBox.Show($"{computerSymbol} Won!!");
            }
        }

        /*
         * Simulates a move to see if it can block a player from winning
         * 
         * Returns the Button that blocks player from winning
         * Returns a null Button if computer can make his move anywhere
         */
        private Button FindWinningMove(string symbol)
        {
            foreach (var button in buttons)
            {
                col = Grid.GetColumn(button);
                row = Grid.GetRow(button);
                if (gameboard[col, row] == " ")
                {
                    gameboard[col, row] = symbol;
                    // Checks if move is a win
                    bool isWinningMove = CheckBoard();
                    gameboard[col, row] = " ";
                    // If the move is a win -> returns button
                    if (isWinningMove)
                        return button;
                }
            }
            return null; // Computer can make his move anywhere
        }


        /*
         * Resets game
         * Clears the gameboard, list<button> and activates all buttons
         */
        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            gameboard = new string[3, 3] { { " ", " ", " " }, { " ", " ", " " }, { " ", " ", " " } };
            buttons.Clear();
            foreach (Button btn in GameGrid.Children)
            {
                btn.Content = null;
                btn.IsEnabled = true;
                buttons.Add(btn);
            }
            isGameOver = false;
        }
    }
}