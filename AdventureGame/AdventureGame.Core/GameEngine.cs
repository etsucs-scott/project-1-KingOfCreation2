using System;
using System.Collections.Generic;

namespace AdventureGame.Core
// ---------------------------------------------------------
// File: GameEngine.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------
{
    // Main game engine that manages game state and logic. Handles player movement, combat, item collection, and win/lose conditions.
    public class GameEngine
    {
        private Random random;
        private List<Monster> monsterTypes;
        private List<Weapon> weaponTypes;

        // Gets the current maze.
        public Maze CurrentMaze { get; private set; }

        // Gets the player.
        public Player CurrentPlayer { get; private set; }

        // Gets the current monster being fought (null if no active battle).
        public Monster CurrentMonster { get; private set; }

        // Gets whether the game is over.
        public bool IsGameOver { get; private set; }

        // Gets whether the player won.
        public bool PlayerWon { get; private set; }

        // Initializes a new game engine with specified maze size.
        public GameEngine(int mazeSize = 21)
        {
            random = new Random();
            InitializeMonsterTypes();
            InitializeWeaponTypes();

            CurrentMaze = new Maze(mazeSize, mazeSize);
            CurrentPlayer = new Player(1, 1);
            IsGameOver = false;
            PlayerWon = false;
        }

        // Initializes the list of possible monsters.
        private void InitializeMonsterTypes()
        {
            monsterTypes = new List<Monster>
            {
                new Monster("Rek'Sai", 45, 12),
                new Monster("Elise", 35, 8),
                new Monster("Seraphine", 30, 7),
                new Monster("Vayne", 32, 11),
                new Monster("Gragas", 48, 12),
                new Monster("Mordekaiser", 50, 15),
                new Monster("Sivir", 33, 8),
                new Monster("Vel'Koz", 36, 9),
                new Monster("Hecarim", 42, 13),
                new Monster("Yuumi", 30, 5),
                new Monster("Sett", 44, 12),
                new Monster("Lee Sin", 46, 14)
            };
        }

        // Initializes the list of possible weapons.
        private void InitializeWeaponTypes()
        {
            weaponTypes = new List<Weapon>
            {
                new Weapon("Needlessly Large Rod", 5),
                new Weapon("Kraken Slayer", 7),
                new Weapon("Long Sword", 3),
                new Weapon("Hull Breaker", 6),
                new Weapon("Trinity Force", 33),
                new Weapon("Jak'sho the Protean", 4),
                new Weapon("Infinity Edge", 8),
                new Weapon("Sanguine Blade", 6),
                new Weapon("Pickaxe", 4),
                new Weapon("Amp Tome", 3),
                new Weapon("Cull", 1),
                new Weapon("Tiamat", 5),
                new Weapon("Steel Sigil", 4),
                new Weapon("B.F. Sword", 7),
                new Weapon("Rabadon's Deathcap", 9)
            };
        }

        // Attempts to move the player in the specified direction. Returns information about what happened 
        public string MovePlayer(int dx, int dy)
        {
            if (IsGameOver)
                return "Game is over!";

            int newX = CurrentPlayer.X + dx;
            int newY = CurrentPlayer.Y + dy;

            // Check if position is walkable
            if (!CurrentMaze.IsWalkable(newX, newY))
            {
                return "Thats a wall dummy, i think. Assuming you just tried to talk into a wall.";
            }

            // Move player
            CurrentPlayer.X = newX;
            CurrentPlayer.Y = newY;

            // Check what's at the new position
            Maze.TileType tile = CurrentMaze.GetTile(newX, newY);

            switch (tile)
            {
                case Maze.TileType.Exit:
                    IsGameOver = true;
                    PlayerWon = true;
                    return "The rift will call to you again, but for now you are free.";

                case Maze.TileType.Monster:
                    CurrentMonster = CreateRandomMonster();
                    return $"A wild {CurrentMonster.Name} appears!";

                case Maze.TileType.Weapon:
                    Weapon weapon = GetRandomWeapon();
                    CurrentPlayer.AddWeapon(weapon);
                    CurrentMaze.SetTile(newX, newY, Maze.TileType.Empty);
                    return weapon.PickupMessage;

                case Maze.TileType.Potion:
                    Potion potion = new Potion();
                    CurrentPlayer.UsePotion(potion);
                    CurrentMaze.SetTile(newX, newY, Maze.TileType.Empty);
                    return potion.PickupMessage;

                default:
                    return "";
            }
        }

        // Player attacks first, then monster counterattacks if alive.
        public string[] ExecuteCombatTurn()
        {
            if (CurrentMonster == null || !CurrentMonster.IsAlive())
                return new string[] { "No active battle!" };

            List<string> messages = new List<string>();

            // Player attacks
            CurrentPlayer.Attack(CurrentMonster);
            messages.Add($"Player attacks {CurrentMonster.Name} for {CurrentPlayer.AttackPower} damage!");
            messages.Add($"{CurrentMonster.Name} HP: {CurrentMonster.Health}");

            // Check if monster died
            if (!CurrentMonster.IsAlive())
            {
                messages.Add($"{CurrentMonster.Name} is defeated!");
                CurrentMaze.SetTile(CurrentPlayer.X, CurrentPlayer.Y, Maze.TileType.Empty);
                CurrentMonster = null;
                return messages.ToArray();
            }

            // Monster attacks
            CurrentMonster.Attack(CurrentPlayer);
            messages.Add($"{CurrentMonster.Name} attacks Player for {CurrentMonster.AttackPower} damage!");
            messages.Add($"Player HP: {CurrentPlayer.Health}");

            // Check if player died
            if (!CurrentPlayer.IsAlive())
            {
                messages.Add("Player has been defeated!");
                IsGameOver = true;
                PlayerWon = false;
            }

            return messages.ToArray();
        }

        // Creates a random monster from the available types.
        private Monster CreateRandomMonster()
        {
            Monster template = monsterTypes[random.Next(monsterTypes.Count)];
            return new Monster(template.Name, template.Health, template.AttackPower);
        }

        // Gets a random weapon from the available types.
        private Weapon GetRandomWeapon()
        {
            Weapon template = weaponTypes[random.Next(weaponTypes.Count)];
            return new Weapon(template.Name, template.Modifier);
        }
    }
}