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
                bool isQuitting = false;
                while (isChoosing)
                {
                    WriteConnect4Title("         ");
                    Console.WriteLine("What would you like to do?\n" +
                    "[1] - New Single-Player Game\n" +
                    "[2] - New Multi-Player Game\n" +
                    "[3] - Join Multi-Player Game\n" +
                    "[4] - View Game Results\n" +
                    "[5] - Quit\n");
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
                        case "5":
                            isQuitting = true;
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Invalid response please choose 1, 2, 3, 4, or 5");
                            Console.Write("--> ");
                            isChoosing = true;
                            break;
                    }
                    Console.Clear();
                    if (isQuitting == false)
                    {
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
                        Console.Clear();
                    }
                }
            }
        }
        private static void WriteConnect4Title(string spacing)
        {
            Console.Write(spacing);
            WriteInColor("C", ConsoleColor.DarkRed);
            WriteInColor("O", ConsoleColor.DarkYellow);
            WriteInColor("NNE", ConsoleColor.DarkCyan);
            WriteInColor("C", ConsoleColor.DarkRed);
            WriteInColor("T4\n\n", ConsoleColor.DarkCyan);
            Console.ResetColor();
        }
        private static void WriteInColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }
    }
}
