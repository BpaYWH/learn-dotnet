
namespace ConsoleApp {
    public class Gomoku
    {
        // private char [,] board = new char [15,15];
        // private Player player1, player2;

        public void greeting() {
            Console.WriteLine("Hello World from Gomoku!");
        }
        public void init() {

        }

        public void printBoard() {
            Console.WriteLine("Gomoku board");

            // print x-coord
            Console.Write("|    ");
            for (int i = 0; i < 15; i++) {
                Console.Write($"| {i} ");
            }
            Console.WriteLine("|");

            printSeparator();

            // print y-coord
            for (int i = 0; i < 15; i++) {
                if (i < 10) {
                    Console.Write($"|  {i} ");
                } else {
                    Console.Write($"| {i} ");
                }
                for (int j = 0; j < 15; j++) {
                    if (j > 9) {
                        Console.Write("|    ");
                        continue;
                    }
                    Console.Write("|   ");
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

        private bool checkWin() {
            return false;
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

    public char getSign() {
        return this.sign;
    }
}