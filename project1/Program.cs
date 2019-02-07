using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
    class Program
    {
        static void Main(string[] args)
        {
            Player[,] playerData = JsonConvert.DeserializeObject<Player[,]>(File.ReadAllText(@"C:\Users\haljas\Desktop\CWEB2010\Project1\player.JSON"));

            bool[,] selectedPlayer = new bool[playerData.GetLength(0), playerData.GetLength(1)];
            for (int i = 0; i < playerData.GetLength(0); i++)
            {
                for(int j = 0; j<playerData.GetLength(1); j++)
                {
                    selectedPlayer[i, j] = playerData[i, j].playerSelected;
                }
            }

            string[,] playerName = new string[playerData.GetLength(0), playerData.GetLength(1)];
            for (int i = 0; i < playerData.GetLength(0); i++)
            {
                for(int j = 0; j < playerData.GetLength(1); j++)
                {
                    playerName[i, j] = playerData[i, j].Name;
                }
            }

            double[,] playerRequestedSalary = new double[playerData.GetLength(0), playerData.GetLength(1)];
            for (int i = 0; i < playerData.GetLength(0); i++)
            {
                for (int j = 0; j < playerData.GetLength(1); j++)
                {
                    playerRequestedSalary[i, j] = playerData[i, j].requestedSalary;
                }
            }

            string[,] playerCollege = new string[playerData.GetLength(0), playerData.GetLength(1)];
            for (int i = 0; i < playerData.GetLength(0); i++)
            {
                for (int j = 0; j < playerData.GetLength(1); j++)
                {
                    playerCollege[i, j] = playerData[i, j].College;
                }
            }

            string[] playerPosition = new string[playerData.GetLength(0)];
            for (int i = 0; i < playerData.GetLength(0); i++)
            {
                for (int j = 0; j < playerData.GetLength(1); j++)
                {
                    playerPosition[i] = playerData[i, j].Position;
                }
            }

            string[,] playerRank = new string[playerData.GetLength(0), playerData.GetLength(1)];
            for (int i = 0; i < playerData.GetLength(0); i++)
            {
                for (int j = 0; j < playerData.GetLength(1); j++)
                {
                    playerRank[i, j] = playerData[i, j].Rank;
                }
            }

            //Create list to hold player rank selections
            List<int> playerRankSelection = new List<int>();

            //Global variables
            ConsoleKey sentinel;
            double salaryCap = 95000000;
            int row, column;
            int selectionCount = 0;
            bool effectiveNFLDraft = false;
            string savingMoney = "Congrats! You made cost effective draft selections for 2019!\n";

            //Call greeting method
            greeting(salaryCap);
            
            //Call pressedKey method
            pressedKey(out sentinel, ref selectionCount);

            //While loop used to call methods when the user has not pressed Escape
            while (sentinel != ConsoleKey.Escape)
            {
                showPlayerTable(playerName, playerRequestedSalary, playerCollege, playerPosition, playerRank, selectedPlayer);

                row = getRow(playerPosition, salaryCap);

                verifyRowSelection(ref row);

                showPositionTable(row, playerName, playerRequestedSalary, playerCollege, playerPosition, playerRank, selectedPlayer);

                column = getColumn(ref playerRankSelection, playerRank, row, playerPosition, salaryCap);

                verifyColumnSelection(ref column);

                playerSelected(playerName, playerCollege, playerRequestedSalary, ref salaryCap, row, column, selectedPlayer, ref selectionCount);

                costEffective(ref playerRankSelection, ref selectionCount, ref salaryCap, ref effectiveNFLDraft, savingMoney);

                pressedKey(out sentinel, ref selectionCount);
            }

            //Call the closing message after the user has five draft selections or has pressed Escape.
            showPrice(selectionCount, salaryCap, ref effectiveNFLDraft, savingMoney, selectedPlayer, playerRank, playerName, playerCollege, playerRequestedSalary, playerPosition);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        //Greeting Method
        static void greeting(double salary)
        {
            Console.WriteLine("Welcome to the 2019 NFL Draft!");
            Console.WriteLine("You have a salary cap of: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{salary.ToString("c")}\n");
            Console.ResetColor();
            Console.WriteLine("You can select up to 5 players.");
            Console.WriteLine("Players may only be selected once, and you may only select players that you can afford.");
        }

        //pressedKey Method
        static void pressedKey(out ConsoleKey key, ref int selectionCount)
        {
            Console.WriteLine("Please press any key to start selecting your draft picks.");
            Console.WriteLine("In order to see the player table clearly you will need to maximize your screen.");
            Console.WriteLine("To end the draft press the Escape key.");
            key = Console.ReadKey().Key;

            if (selectionCount == 5)
            {
                Console.Clear();
                Console.WriteLine("You have reached your max number of player selections.");
                Console.WriteLine("Your draft is complete. Press any key to continue.");
                Console.ReadKey();
                key = ConsoleKey.Escape;
            }
        }

        //Method to show the player table to the user
        static void showPlayerTable(string[,] playerName, double[,] playerRequestedSalary, string[,] playerCollege, string[] playerPosition, string[,] playerRank, bool[,] selectedPlayer)
        {
            Console.Clear();
            //Providing column names for the table with proper spacing between table objects
            //Position column created
            Console.Write("Position".PadRight(25));

            //This for loop will handle pulling the name of each rank from the rank array and placing the 
            //names of each rank at the top of the table
            for (int i = 0; i < playerRank.GetLength(1); i++)
            {
                Console.Write($"{playerRank[i, i]}".PadRight(25));
            }
            //Provides a space between the table headers and the first row of player data
            Console.WriteLine("\n");

            //This for loop will add the players into the table and organize them by Position and Rank
            //The inner for loops will show the player data if the player has not been selected, 
            //and black the player data out if the player has been selected.
            for (var i = 0; i < playerName.GetLength(0); i++)
            {
                Console.Write($"{playerPosition[i].PadRight(25)}");
                for (var x = 0; x < playerName.GetLength(1); x++)
                {
                    if (selectedPlayer[i, x] == false)
                    {
                        Console.Write($"{playerName[i, x].PadRight(25)}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{playerName[i, x].PadRight(25)}");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("");
                Console.Write("".PadRight(25));

                for (var x = 0; x < playerCollege.GetLength(1); x++)
                {
                    if (selectedPlayer[i, x] == false)
                    {
                        Console.Write($"{playerCollege[i, x].PadRight(25)}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{playerCollege[i, x].PadRight(25)}");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("");
                Console.Write("".PadRight(25));

                for (var x = 0; x < playerRequestedSalary.GetLength(1); x++)
                {
                    if (selectedPlayer[i, x] == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{playerRequestedSalary[i, x].ToString("c").PadRight(25)}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{playerRequestedSalary[i, x].ToString("c").PadRight(25)}");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("\n");
            }
        }
        //After the coach picks a position for the draft from the larger player table this method will show a table of the players within that position for the coach to select the specific player
        static void showPositionTable(int row, string[,] playerName, double[,] playerRequestedSalary, string[,] playerCollege, string[] playerPosition, string[,] playerRank, bool[,] selectedPlayer)
        {
            Console.Clear();
            Console.Write("".PadRight(25));
            for (int i = 0; i < playerRank.GetLength(1); i++)
            {
                Console.Write($"{playerRank[row, i]}".PadRight(25));
            }
            Console.WriteLine("\n");

            Console.Write($"{playerPosition[row].PadRight(25)}");

            for (var x = 0; x < playerName.GetLength(1); x++)
            {
                if (selectedPlayer[row, x] == false)
                {
                    Console.Write($"{playerName[row, x].PadRight(25)}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{playerName[row, x].PadRight(25)}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("");
            Console.Write("".PadRight(25));

            for (var x = 0; x < playerName.GetLength(1); x++)
            {
                if (selectedPlayer[row, x] == false)
                {
                    Console.Write($"{playerCollege[row, x].PadRight(25)}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{playerCollege[row, x].PadRight(25)}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("");
            Console.Write("".PadRight(25));

            for (var x = 0; x < playerName.GetLength(1); x++)
            {
                if (selectedPlayer[row, x] == false)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{playerRequestedSalary[row, x].ToString("c").PadRight(25)}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{playerRequestedSalary[row, x].ToString("c").PadRight(25)}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\n");
        }
        //The getRow method prompts the coach to select a position they would like to draft from the player table
        static int getRow(string[] playerPosition, double cost)
        {
            Console.Write($"You have ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{ cost.ToString("c")}");
            Console.ResetColor();
            Console.Write(" remaining.\n");
            int row;
            Console.WriteLine("Please select the position of the player you would like to draft and press Enter.\n");
            for (int i = 0; i < playerPosition.Length; i++)
            {
                Console.WriteLine($"{i + 1}.) {playerPosition[i]}");
            }
            try
            {
                row = Convert.ToInt32(Console.ReadLine());
                return row = row - 1;
            }
            catch
            {
                return row = -1;
            }

        }
        //The getColumn method prompts the coach to pick the rank of the player they would like to select from the position table.
        static int getColumn(ref List<int> playerRankSelection, string[,] playerRank, int row, string[] playerPosition, double cost)
        {
            int column;
            Console.Write($"You have ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{ cost.ToString("c")}");
            Console.ResetColor();
            Console.Write(" of your draft allotment remaining.\n");
            Console.WriteLine($"You have selected:\n{row + 1}.) {playerPosition[row]}\n");
            Console.WriteLine("Please enter the rank of the player you would like to select and press Enter\n");
            for (int i = 0; i < playerRank.GetLength(1); i++)
            {
                Console.WriteLine($"{i + 1}.) {playerRank[i, i]}");
            }
            try
            {
                column = Convert.ToInt32(Console.ReadLine());
                playerRankSelection.Add(column);
                return column = column - 1;
            }
            catch
            {
                return column = -1;
            }

        }
        //This method will ensure that the coach picks a row between 1 and 8, and if they don't they will be prompted to.
        static void verifyRowSelection(ref int row)
        {
            while ((row < 0) || (row > 7))
            {
                try
                {
                    Console.WriteLine("Please enter a number between 1 and 8");
                    row = (Convert.ToInt32(Console.ReadLine()) - 1);
                }
                catch
                {
                    row = -1;
                }

            }
        }
        //This method will ensure that the coach picks a column between 1 and 5, and if they don't they will be prompted to.
        static void verifyColumnSelection(ref int column)
        {
            while ((column < 0) || (column > 4))
            {
                try
                {
                    Console.WriteLine("Please enter a number between 1 and 5.");
                    column = (Convert.ToInt32(Console.ReadLine()) - 1);
                }
                catch
                {
                    column = -1;
                }
            }
        }

        //The playerSelected method cofirms the player that has been selected and updates the coach on their remaining draft allowance.
        //If the coach cannot afford a specific draft selection this method also explains that they do not have enough for that player selection.
        //If the coach should pick a player that has already been selected, that will be explained through this method as well.
        static void playerSelected(string[,] playerName, string[,] playerCollege, double[,] cost, ref double balanceRemaining, int row, int column, bool[,] selectedPlayer, ref int selectionCount)
        {
            if (selectedPlayer[row, column] == false)
            {
                if (balanceRemaining >= cost[row, column])
                {
                    Console.Clear();
                    balanceRemaining -= cost[row, column];
                    Console.Write($"You have selected {playerName[row, column]} from {playerCollege[row, column]} for ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{cost[row, column].ToString("c")}");
                    Console.ResetColor();
                    Console.WriteLine("!");
                    Console.Write("You have ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{ balanceRemaining.ToString("c")} ");
                    Console.ResetColor();
                    Console.WriteLine("of your alloted draft amount remaining.\n");
                    selectedPlayer[row, column] = true;
                    selectionCount++;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Sorry, but you only have {balanceRemaining.ToString("c")} of your alloted draft amount remaining. Please select a player you can afford.\n");
                    Console.ResetColor();
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("This player has previously been selected.");
                Console.ResetColor();
                return;
            }
        }
        
        //The costEffective method sorts the coaches player selections, and determines whether the costEffective criteria has been met.
        static void costEffective(ref List<int> playerRankSelection, ref int selection, ref double balanceRemaining, ref bool effectiveNFLDraft, string savingMoney)
        {
            if (selection == 3)
            {
                playerRankSelection.Sort();
                playerRankSelection.Reverse();

                for (int i = 0; i < playerRankSelection.Count; i++)
                {
                    if (playerRankSelection[i] > 3)
                    {
                        break;
                    }
                    else
                    {
                        if (balanceRemaining > 30000000)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(savingMoney);
                            Console.ResetColor();
                            effectiveNFLDraft = true;
                            break;
                        }
                    }
                }
            }
        }
        //This method provides the final message to the coach regarding their player selections.  If no selections have been made it 
        static void showPrice(int selectionCount, double salary, ref bool effectiveNFLDraft, string savingMoney, bool[,] selectedPlayer, string[,] playerRank, string[,] playerName, string[,] playerCollege, double[,] cost, string[] playerPosition)
        {
            Console.Clear();
            if (selectionCount == 0)
            {
                Console.WriteLine("When you are ready, please come back and select your new players!");
            }
            else
            {
                if (effectiveNFLDraft == true)
                {
                    Console.Write("Congratulations, ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{savingMoney}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Congratulations on completing your draft!");
                }
                
                Console.Write("After your draft selections you have ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{salary.ToString("c")} ");
                Console.ResetColor();
                Console.WriteLine("remaining.");
                Console.WriteLine("You have drafted the following players:\n");

                
                for (var i = 0; i < playerName.GetLength(0); i++)
                {
                    for (var x = 0; x < playerName.GetLength(1); x++)
                    {
                        if (selectedPlayer[i, x] == true)
                        {
                            Console.Write($"{playerRank[i, x].PadRight(25)}");
                        }
                    }
                }
                Console.WriteLine("");
                for (var i = 0; i < playerName.GetLength(0); i++)
                {
                    for (var x = 0; x < playerName.GetLength(1); x++)
                    {
                        if (selectedPlayer[i, x] == true)
                        {
                            Console.Write($"{playerPosition[i].PadRight(25)}");
                        }
                    }
                }
                Console.WriteLine("");
                for (var i = 0; i < playerName.GetLength(0); i++)
                {
                    for (var x = 0; x < playerName.GetLength(1); x++)
                    {
                        if (selectedPlayer[i, x] == true)
                        {
                            Console.Write($"{playerName[i, x].PadRight(25)}");
                        }
                    }
                }
                Console.WriteLine("");
                for (var i = 0; i < playerCollege.GetLength(0); i++)
                {
                    for (var x = 0; x < playerCollege.GetLength(1); x++)
                    {
                        if (selectedPlayer[i, x] == true)
                        {
                            Console.Write($"{playerCollege[i, x].PadRight(25)}");
                        }
                    }
                }
                Console.WriteLine("");
                for (var i = 0; i < cost.GetLength(0); i++)
                {
                    for (var x = 0; x < cost.GetLength(1); x++)
                    {
                        if (selectedPlayer[i, x] == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write($"{cost[i, x].ToString("c").PadRight(25)}");
                            Console.ResetColor();
                        }
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Thank you for participating in the 2019 NFL Draft!\n");
            Console.WriteLine("This application will now close.");
        }
    }
}


