namespace ConsoleApp {
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World from Main!");

            Gomoku gomoku = new ();
            gomoku.greeting();
            gomoku.printBoard();
        }
    }

}