using System;
using AdventureGame.Core;

namespace AdventureGame.Console
// ---------------------------------------------------------
// File: Program.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

{
    // Main console UI for the Adventure Game. Handles all user input and display rendering.
    class Program
    {
        private static GameEngine engine;

        // Main entry point for the application.
        static void Main(string[] args)
        {
            System.Console.Title = "Legends of League";
            System.Console.CursorVisible = false;

            DisplayIntro();

            engine = new GameEngine(21);

            RunGameLoop();
        }

        // Displays the introduction screen with instructions.
        static void DisplayIntro()
        {
            System.Console.Clear();
            System.Console.WriteLine("Welcome to Legends of League");
            System.Console.WriteLine();
            System.Console.WriteLine("You have spawned in on the summoners rift");
            System.Console.WriteLine("Your goal: Find the enemy nexus and make it out alive!");
            System.Console.WriteLine();
            System.Console.WriteLine("CONTROLS:");
            System.Console.WriteLine("  Use WASD or Arrow Keys to move");
            System.Console.WriteLine();
            System.Console.WriteLine("LEGEND:");
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("  @ ");
            System.Console.ResetColor();
            System.Console.WriteLine("= You (Player)");

            System.Console.WriteLine("  # = Wall");
            System.Console.WriteLine("    = Empty Space");

            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.Write("  M ");
            System.Console.ResetColor();
            System.Console.WriteLine("= Monster");

            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write("  W ");
            System.Console.ResetColor();
            System.Console.WriteLine("= Weapon");

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.Write("  P ");
            System.Console.ResetColor();
            System.Console.WriteLine("= Potion (+30 HP)");

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write("  E ");
            System.Console.ResetColor();
            System.Console.WriteLine("= Exit");

            System.Console.WriteLine();
            System.Console.WriteLine("STATS:");
            System.Console.WriteLine("  Starting HP: 120 (Max: 150)");
            System.Console.WriteLine("  Base Damage: 10");
            System.Console.WriteLine("  Monster HP: 32-46");
            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to begin...");
            System.Console.ReadKey(true);
        }

        // Main game loop - handles rendering and input.
        static void RunGameLoop()
        {
            string message = "";

            while (!engine.IsGameOver)
            {
                DrawScreen(message);
                message = "";

                // Get player input
                ConsoleKeyInfo keyInfo = System.Console.ReadKey(true);

                // Clear any queued inputs
                while (System.Console.KeyAvailable)
                {
                    System.Console.ReadKey(true);
                }

                // Process movement
                int dx = 0, dy = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        dy = -1;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        dy = 1;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        dx = -1;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        dx = 1;
                        break;
                    default:
                        continue;
                }

                // Move player and get result
                message = engine.MovePlayer(dx, dy);

                // If we entered combat, handle the battle
                if (engine.CurrentMonster != null && engine.CurrentMonster.IsAlive())
                {
                    HandleBattle();
                }
            }

            DisplayEndScreen();
        }

        // Draws the current game state to the console.
        static void DrawScreen(string message = "")
        {
            System.Console.Clear();
            DrawMaze();
            DrawStats();

            if (!string.IsNullOrEmpty(message))
            {
                System.Console.WriteLine();
                System.Console.WriteLine(message);
            }
        }

        // Renders the maze grid with all entities
        static void DrawMaze()
        {
            for (int y = 0; y < engine.CurrentMaze.Rows; y++)
            {
                for (int x = 0; x < engine.CurrentMaze.Cols; x++)
                {
                    // Check if player is at this position
                    if (x == engine.CurrentPlayer.X && y == engine.CurrentPlayer.Y)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.Write("@");
                        System.Console.ResetColor();
                        continue;
                    }

                    // Get tile and render appropriate symbol
                    Maze.TileType tile = engine.CurrentMaze.GetTile(x, y);
                    switch (tile)
                    {
                        case Maze.TileType.Wall:
                            System.Console.Write("#");
                            break;
                        case Maze.TileType.Empty:
                            System.Console.Write(" ");
                            break;
                        case Maze.TileType.Monster:
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            System.Console.Write("M");
                            System.Console.ResetColor();
                            break;
                        case Maze.TileType.Weapon:
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            System.Console.Write("W");
                            System.Console.ResetColor();
                            break;
                        case Maze.TileType.Potion:
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            System.Console.Write("P");
                            System.Console.ResetColor();
                            break;
                        case Maze.TileType.Exit:
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            System.Console.Write("E");
                            System.Console.ResetColor();
                            break;
                    }
                }
                System.Console.WriteLine();
            }
        }

        // Displays player statistics below the maze.
        static void DrawStats()
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"HP: {engine.CurrentPlayer.Health}/150 | Attack: {engine.CurrentPlayer.AttackPower} | Weapons: {engine.CurrentPlayer.Weapons.Count}");
        }

        // Handles turn based combat until battle ends.
        static void HandleBattle()
        {
            System.Console.Clear();
            DrawMaze();
            System.Console.WriteLine();
            System.Console.WriteLine($"=== BATTLE: {engine.CurrentMonster.Name} ===");
            System.Console.WriteLine($"Monster HP: {engine.CurrentMonster.Health} | Attack: {engine.CurrentMonster.AttackPower}");
            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to fight...");
            System.Console.ReadKey(true);

            int turn = 1;
            while (engine.CurrentMonster != null && engine.CurrentMonster.IsAlive() && engine.CurrentPlayer.IsAlive())
            {
                System.Console.Clear();
                DrawMaze();
                System.Console.WriteLine();
                System.Console.WriteLine($"=== TURN {turn} ===");

                string[] combatLog = engine.ExecuteCombatTurn();
                foreach (string logEntry in combatLog)
                {
                    System.Console.WriteLine(logEntry);
                }

                if (engine.CurrentMonster != null && engine.CurrentMonster.IsAlive() && engine.CurrentPlayer.IsAlive())
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("Press any key for next turn...");
                    System.Console.ReadKey(true);
                    turn++;
                }
            }

            // Show battle result
            System.Console.WriteLine();
            if (engine.CurrentPlayer.IsAlive())
            {
                System.Console.WriteLine("Victory! You defeated the monster!");
            }
            else
            {
                System.Console.WriteLine("You have been killed :(");
            }
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey(true);
        }

        // Displays the end game screen (win or lose).
        static void DisplayEndScreen()
        {
            System.Console.Clear();

            if (engine.PlayerWon)
            {
                System.Console.WriteLine("OKAY LETS GOOO W WINNN");
                System.Console.WriteLine();
                System.Console.WriteLine("You escaped the rift!");
                System.Console.WriteLine("Congratulations you poor soul!");
            }
            else
            {
                System.Console.WriteLine("WOMP WOMP SKILL ISSUE");
                System.Console.WriteLine();
                System.Console.WriteLine("You have been defeated, back to the fountain");
                System.Console.WriteLine("Better luck next time!");
            }

            System.Console.WriteLine();
            System.Console.WriteLine($"Final Stats:");
            System.Console.WriteLine($"  HP Remaining: {engine.CurrentPlayer.Health}");
            System.Console.WriteLine($"  Final Attack Power: {engine.CurrentPlayer.AttackPower}");
            System.Console.WriteLine($"  Weapons Collected: {engine.CurrentPlayer.Weapons.Count}");
            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey(true);
        }
    }
}