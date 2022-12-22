using System;

namespace ConnectFour.App
{
    internal static class Program
    {
        static void Main()
        {
            bool isRunning = true;
            while (isRunning)
            {
                bool isChoosing = true;
                while (isChoosing)
                {
                    WriteConnect4Title("         ");
                    Console.WriteLine("What would you like to do?\n" +
                    "[1] - New Single-Player Game\n" +
                    "[2] - New Multi-Player Game\n" +
                    "[3] - Join Multi-Player Game\n" +
                    "[4] - View Game Results\n");
                    Console.Write("--> ");
                    string userResponse = Console.ReadLine();
                    isChoosing = false;
                    switch (userResponse)
                    {
                        case "1":
                            //New Single-Player Game
                            break;
                        case "2":
                            //New Multi-Player Game
                            break;
                        case "3":
                            //Join Multi-Player Game
                            break;
                        case "4":
                            //View Game Results
                            break;
                        default:
                            Console.WriteLine("Invalid response please choose 1, 2, 3, or 4");
                            Console.Write("--> ");
                            isChoosing = true;
                            break;
                    }
                    Console.Clear();
                }
                WriteConnect4Title("         ");
                Console.WriteLine("What would you like to do?\n" +
                "[1] Return to the Main Menu\n" +
                "[2] Quit");
                Console.Write("--> ");
                string continueProgram = Console.ReadLine();
                if (continueProgram == "2")
                {
                    isRunning = false;
                }
            }
        }
        private static void WriteConnect4Title(string spacing)
        {
            Console.Write(spacing);
            WriteInDarkRed("C");
            WriteInDarkYellow("O");
            WriteInDarkCyan("NNE");
            WriteInDarkRed("C");
            WriteInDarkCyan("T4\n\n");
        }
        private static void WriteInDarkYellow(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(text);
            Console.ResetColor();
        }
        private static void WriteInDarkRed(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(text);
            Console.ResetColor();
        }
        private static void WriteInDarkCyan(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
