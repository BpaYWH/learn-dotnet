
namespace ConsoleApp {
    public class Gomoku
    {
        private char [,] board;
        private Player player1, player2;
        private int turn;
        private bool isGameOver;
        private string gameoverState;

        public Gomoku() {
            init();
        }

        public void init() {
            this.board = new char [15,15];
            fill2DArray(this.board, ' ');
            this.turn = 1;
            this.player1 = new Player("Player 1", 'O');
            this.player2 = new Player("Player 2", 'X');
            this.isGameOver = false;
        }

        private void clearLine() {
                int currentlinecursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.WriteLine(new string(' ', Console.WindowWidth)); 
    Console.SetCursorPosition(0, currentlinecursor);
        }
        public void play() {
            greeting();
            playerSetup(this.player1, this.player2);

            while (!isGameOver) {
                playTurn(this.player1);
                if (isGameOver) {
                    gameover(player1);
                    break;
                }

                clearLine();

                playTurn(this.player2);
                if (isGameOver)
                    gameover(player2);
            }
        }

        private void fill2DArray(char [,] array, char fill) {
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    array[i,j] = fill;
                }
            }
        } 
        
        private void greeting() {
            Console.WriteLine("Hello World from Gomoku!");
        }

        private void playerSetup(Player player1, Player player2) {
            Console.WriteLine("Player 1 will move first");

            string instruction = $"{player1.getName()}, please enter your name:";
            Console.WriteLine(instruction);
            player1.setName(nonNullReadLine(instruction));

            instruction = $"{player2.getName()}, please enter your name:";
            Console.WriteLine(instruction);
            player2.setName(nonNullReadLine(instruction));

            Console.WriteLine($"{player1.getName()}: {player1.getSign()} | {player2.getName()}: {player2.getSign()}");
        }

        private string nonNullReadLine(string? alert) {
            string? line = Console.ReadLine();
            while (line == "") {
                if (alert != null)
                    Console.WriteLine(alert);
                line = Console.ReadLine();
            }
            return line!;
        }

        private void playTurn(Player player) {
            Console.WriteLine($"Turn {this.turn}, {player.getName()}({player.getSign()})'s turn");
            printBoard();

            // Move
            while (true) {
                if (playerMove(player)) {
                    break;
                }
            }

            // Check win
            if(checkWin(player.getSign())) {
                this.gameoverState = "win";
                this.isGameOver = true;
                printBoard();
                return;
            }

            // Update game state 
            turn++;
            if (turn == 15*15) {
                this.gameoverState = "draw";
                this.isGameOver = true;
            }
        }

        private void printBoard() {
            Console.WriteLine("Gomoku board");

            // print x-coord
            Console.Write("|    ");
            for (int i = 0; i < 15; i++) {
                Console.Write($"| {i} ");
            }
            Console.WriteLine("|");

            printSeparator();

            // print y-coord & board
            for (int i = 0; i < 15; i++) {
                if (i < 10) {
                    Console.Write($"|  {i} ");
                } else {
                    Console.Write($"| {i} ");
                }
                for (int j = 0; j < 15; j++) {
                    if (j > 9) {
                        Console.Write($"|  {this.board[j, i]} ");
                        continue;
                    }
                    Console.Write($"| {this.board[j, i]} ");
                }
                Console.WriteLine("|");
                printSeparator();
            }
        }

        private void printSeparator() {
            // print separator
            Console.Write("|----");
            for (int i = 0; i < 15; i++) {
                if (i > 9) {
                    Console.Write("|----");
                    continue;
                }
                Console.Write("|---");
            }
            Console.WriteLine("|");
        }

        private bool playerMove(Player player) {
            Console.WriteLine($"Please enter the x-coord:");
            string x = nonNullReadLine("Please enter a valid x-coord");
            int xInt;
            while (!int.TryParse(x, out xInt) || xInt < 0 || xInt > 14) {
                Console.WriteLine("Please enter a valid x-coord");
                x = nonNullReadLine("");
            }
            
            Console.WriteLine($"Please enter the y-coord:");
            string y = nonNullReadLine("Please enter a valid y-coord");
            int yInt;
            while (!int.TryParse(y, out yInt) || yInt < 0 || yInt > 14) {
                Console.WriteLine("Please enter a valid y-coord");
                y = nonNullReadLine("");
            }

            if (this.board[xInt,yInt] != ' ') {
                Console.WriteLine("This position is already taken");
                return false;
            }

            this.board[xInt,yInt] = player.getSign();
            return true;
        }

        private bool checkWin(char sign) {
            // check horizontal
            for (int i = 0; i < this.board.GetLength(0); i++) {
                for (int j = 0; j < this.board.GetLength(1)-4; j++) {
                    if (this.board[i,j] == sign && this.board[i,j+1] == sign && this.board[i,j+2] == sign && this.board[i,j+3] == sign && this.board[i,j+4] == sign)
                        return true;
                }
            }

            // check vertical
            for (int i = 0; i < this.board.GetLength(1); i++) {
                for (int j = 0; j < this.board.GetLength(0)-4; j++) {
                    if (this.board[j,i] == sign && this.board[j+1,i] == sign && this.board[j+2,i] == sign && this.board[j+3,i] == sign && this.board[j+4,i] == sign)
                        return true;
                }
            }

            // check forward diagonal /
            for (int i = this.board.GetLength(0) - 1; i > 3; i--) {
                for (int j = 0; j < this.board.GetLength(1) - 4; j++) {
                    if (this.board[i,j] == sign && this.board[i-1,j+1] == sign && this.board[i-2,j+2] == sign && this.board[i-3,j+3] == sign && this.board[i-4,j+4] == sign)
                        return true;
                }
            }

            // check backward diagonal \
            for (int i = 0; i < this.board.GetLength(0) - 4; i++) {
                for (int j = 0; j < this.board.GetLength(1) - 4; j++) {
                    if (this.board[i,j] == sign && this.board[i+1,j+1] == sign && this.board[i+2,j+2] == sign && this.board[i+3,j+3] == sign && this.board[i+4,j+4] == sign)
                        return true;
                }
            }

            return false;
        }

        private void gameover(Player player) {
            Console.WriteLine("Game Over");
            if (gameoverState == "draw") {
                Console.WriteLine("Draw");
                return;
            }
            Console.WriteLine($"{player.getName()} wins!");
        }
        
    }
}

class Player {
    private string name;
    private char sign;

    public Player(string name, char sign)
    {
        this.name = name;
        this.sign = sign;
    }

    public string getName() {
        return this.name;
    }

    public void setName(string name) {
        this.name = name;
    }

    public char getSign() {
        return this.sign;
    }

    public void setSign(char sign) {
        this.sign = sign;
    }
}