using System;

namespace AdventureGame.Core
// ---------------------------------------------------------
// File: Maze.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

{
    // Represents the game maze with a 2D grid of tiles.
    // Handles maze generation, tile management, and pathfinding.
    public class Maze
    {
        // Enum representing different tile types in the maze.
        public enum TileType
        {
            Wall,
            Empty,
            Monster,
            Weapon,
            Potion,
            Exit
        }

        private TileType[,] grid;
        private Random random;

        // Gets the number of rows in the maze.
        public int Rows { get; private set; }

        // Gets the number of columns in the maze.
        public int Cols { get; private set; }

        // Creates a new maze with the specified dimensions and generates the layout.
        public Maze(int rows, int cols, int? seed = null)
        {
            Rows = rows;
            Cols = cols;
            random = seed.HasValue ? new Random(seed.Value) : new Random();
            grid = new TileType[rows, cols];
            GenerateMaze();
        }

        // Gets the tile type at the specified coordinates.
        public TileType GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Cols || y >= Rows)
                return TileType.Wall;
            return grid[y, x];
        }

        // Sets the tile type at the specified coordinates.
        public void SetTile(int x, int y, TileType tileType)
        {
            if (x >= 0 && y >= 0 && x < Cols && y < Rows)
            {
                grid[y, x] = tileType;
            }
        }

        // Checks if the specified position is walkable (not a wall).
        public bool IsWalkable(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Cols || y >= Rows)
                return false;
            return grid[y, x] != TileType.Wall;
        }

        // Generates a random maze layout using a simple algorithm. Creates walls, empty spaces, then places monsters, items, and exit.
        private void GenerateMaze()
        {
            // Initialize all tiles as walls
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    grid[y, x] = TileType.Wall;
                }
            }

            // Create a simple maze using recursive backtracking
            CarvePassages(1, 1);

            // Place exit in a reachable location (opposite corner from start)
            PlaceExit();

            // Place monsters randomly in empty spaces
            PlaceEntities(TileType.Monster, random.Next(5, 10));

            // Place weapons
            PlaceEntities(TileType.Weapon, random.Next(3, 7));

            // Place potions
            PlaceEntities(TileType.Potion, random.Next(3, 7));
        }

        // Uses recursive backtracking algorithm to carve passages in the maze and creates a connected path through the maze.
        private void CarvePassages(int x, int y)
        {
            // Directions: up, right, down, left
            int[,] directions = { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

            // Shuffle directions
            for (int i = 0; i < 4; i++)
            {
                int j = random.Next(i, 4);
                // Swap
                for (int k = 0; k < 2; k++)
                {
                    int temp = directions[i, k];
                    directions[i, k] = directions[j, k];
                    directions[j, k] = temp;
                }
            }

            grid[y, x] = TileType.Empty;

            // Try each direction
            for (int i = 0; i < 4; i++)
            {
                int nx = x + directions[i, 0] * 2;
                int ny = y + directions[i, 1] * 2;

                if (nx > 0 && ny > 0 && nx < Cols - 1 && ny < Rows - 1 && grid[ny, nx] == TileType.Wall)
                {
                    grid[y + directions[i, 1], x + directions[i, 0]] = TileType.Empty;
                    CarvePassages(nx, ny);
                }
            }
        }

        // Places the exit in a reachable location far from the start.
        private void PlaceExit()
        {
            // Try to place exit in bottom-right area
            for (int attempts = 0; attempts < 100; attempts++)
            {
                int x = random.Next(Cols * 2 / 3, Cols - 1);
                int y = random.Next(Rows * 2 / 3, Rows - 1);

                if (grid[y, x] == TileType.Empty)
                {
                    grid[y, x] = TileType.Exit;
                    return;
                }
            }

            // Fallback: find any empty tile
            for (int y = Rows - 2; y > 0; y--)
            {
                for (int x = Cols - 2; x > 0; x--)
                {
                    if (grid[y, x] == TileType.Empty)
                    {
                        grid[y, x] = TileType.Exit;
                        return;
                    }
                }
            }
        }

        // Places a specific number of monsters and items in random empty tiles.
        private void PlaceEntities(TileType tileType, int count)
        {
            int placed = 0;
            int attempts = 0;
            int maxAttempts = count * 20;

            while (placed < count && attempts < maxAttempts)
            {
                int x = random.Next(1, Cols - 1);
                int y = random.Next(1, Rows - 1);

                // Don't place near starting position (1,1)
                if (grid[y, x] == TileType.Empty && (Math.Abs(x - 1) > 2 || Math.Abs(y - 1) > 2))
                {
                    grid[y, x] = tileType;
                    placed++;
                }
                attempts++;
            }
        }
    }
}