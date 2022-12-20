using System;

namespace ConnectFour.App
{
    internal static class Program
    {
        static void Main()
        {
            MainMenu();
        }
        private static void MainMenu()
        {
            bool isChoosing = true;
            while (isChoosing)
            {
                Console.WriteLine("What would you like to do?\n[1] - New Single-Player Game\n[2] - New Multi-Player Game\n[3] - Join Multi-Player Game\n[4] - View Game Results");
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
                        isChoosing = true;
                        break;
                }
            }
        }
    }
}
