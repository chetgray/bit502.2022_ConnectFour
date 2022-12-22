using System;

namespace ConnectFour.App
{
    internal static class Program
    {
        private static void Main()
        {
            DisplayBoard();

            Console.ReadKey();
        }

        private static void DisplayBoard()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            /* TBA:
             * Get player names from Player model
             * Grab the initial with substring(1), ToUpper()
             * Compare—if they're the same, use 1 and 2
             * Otherwise insert into respective player markers
             * Let's do this via methods in the player model?
             */
            string p1 = $"░ A ░";
            string p2 = $"░ B ░";

            string[,] space = new string[6, 7];

            for (int i = 0; i < space.GetLength(0); i++)
            {
                for (int j = 0; j < space.GetLength(1); j++)
                {
                    space[i, j] = "     ";
                }
            }

            //Test moves for visualization
            space[5, 0] = p1;
            space[5, 1] = p2;
            space[5, 2] = p1;
            space[4, 0] = p2;
            space[4, 2] = p1;
            space[4, 1] = p2;
            space[3, 2] = p1;
            space[2, 2] = p2;

            SetToBoardColor();
            Console.WriteLine("\n       ╔═════╦═════╦═════╦═════╦═════╦═════╦═════╗");

            for (int i = 0; i < space.GetLength(0); i++)
            {
                Console.ResetColor();
                switch (i)
                {
                    case 0:
                        Console.Write("    \u2086  ");
                        break;

                    case 1:
                        Console.Write("    \u2085  ");
                        break;

                    case 2:
                        Console.Write("    \u2084  ");
                        break;

                    case 3:
                        Console.Write("    \u2083  ");
                        break;

                    case 4:
                        Console.Write("    \u2082  ");
                        break;

                    case 5:
                        Console.Write("    \u2081  ");
                        break;

                    default:
                        break;
                }
                SetToBoardColor();

                for (int j = 0; j < space.GetLength(1); j++)
                {
                    Console.Write("║");
                    Console.ResetColor();
                    if (space[i, j] == p1)
                    {
                        SetToP1Color();
                        Console.Write(space[i, j]);
                    }
                    else if (space[i, j] == p2)
                    {
                        SetToP2Color();
                        Console.Write(space[i, j]);
                    }
                    else
                    {
                        Console.Write(space[i, j]);
                    }
                    SetToBoardColor();
                }
                Console.Write("║");
                Console.ResetColor();

                switch (i)
                {
                    case 1:
                        Console.Write("    Room ID: 1");
                        break;

                    case 2:
                        Console.Write("    Player 1: ");
                        WriteP1Name();
                        Console.ResetColor();
                        break;

                    case 3:
                        Console.Write("    Player 2: ");
                        WriteP2Name();
                        Console.ResetColor();
                        break;

                    case 4:
                        //Will determine what the last move was and output visual/board coordinates
                        Console.Write("    Last play: 3, 4");
                        break;

                    case 5:
                        Console.Write("    Current turn: ");

                        //Will determine whose turn it currently is and output correct color/name
                        SetToP1Color();
                        Console.Write("Aaaaa");

                        SetToBoardColor();
                        break;

                    default:
                        break;
                }

                if (i != 5)
                {
                    SetToBoardColor();
                    Console.WriteLine("\n       ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣");
                }
            }

            Console.WriteLine("\n       ╚═════╩═════╩═════╩═════╩═════╩═════╩═════╝");
            Console.ResetColor();
            Console.WriteLine("          \u2081     \u2082     \u2083     \u2084     \u2085     \u2086     \u2087");
        }

        private static void SetToBoardColor()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }

        /* TBA:
         * Would also prefer to do these through the model
         * e.g. p1.SetColor(), p2.WriteName(), etc. */

        private static void SetToP1Color()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        private static void SetToP2Color()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void WriteP1Name()
        {
            SetToP1Color();
            Console.Write("Aaaaa");
        }

        private static void WriteP2Name()
        {
            SetToP2Color();
            Console.Write("Bbbbb");
        }
    }
}