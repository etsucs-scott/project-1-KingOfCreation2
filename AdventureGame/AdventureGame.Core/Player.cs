using System.Collections.Generic;
using System.Linq;

// ---------------------------------------------------------
// File: Player.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

namespace AdventureGame.Core
{
    // Represents the player character in the game. Implements ICharacter for combat capabilities. Manages player position, health, inventory, and attack power.

    public class Player : ICharacter
    {
        // Constants for player stats
        private const int MAX_HEALTH = 150;
        private const int STARTING_HEALTH = 120;
        private const int BASE_ATTACK = 10;

        // Gets or sets the current health of the player.
        public int Health { get; set; }

        // Gets the player's current attack power (base damage + best weapon).
        public int AttackPower => BASE_ATTACK + GetBestWeaponModifier();

        // Gets or sets the player's X position in the maze.
        public int X { get; set; }

        // Gets or sets the player's Y position in the maze.
        public int Y { get; set; }


        // List of weapons in the player's inventory.
        public List<Weapon> Weapons { get; private set; }

        // Initializes a new player at the specified position.
        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Health = STARTING_HEALTH;
            Weapons = new List<Weapon>();
        }

        // Checks if the player is still alive. Health greater than 0 means alivem, anything else is dead.
        public bool IsAlive()
        {
            return Health > 0;
        }

        // Reduces player health by the specified damage amount, Health cannot go below 0.
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0)
            {
                Health = 0;
            }
        }

        // Attacks the target character, dealing damage equal to AttackPower.
        public void Attack(ICharacter target)
        {
            target.TakeDamage(AttackPower);
        }

        // Adds a weapon to the player's inventory, The weapon with the highest modifier automatically applies to attacks.
        public void AddWeapon(Weapon weapon)
        {
            Weapons.Add(weapon);
        }

        // Uses a potion to restore health.
        public void UsePotion(Potion potion)
        {
            Health += potion.HealAmount;
            if (Health > MAX_HEALTH)
            {
                Health = MAX_HEALTH;
            }
        }

        // Gets the damage modifier from the best weapon in inventory. If no weapon than returns 0.
        private int GetBestWeaponModifier()
        {
            return Weapons.Count > 0 ? Weapons.Max(w => w.Modifier) : 0;
        }
    }
}