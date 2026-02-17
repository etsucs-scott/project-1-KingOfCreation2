namespace AdventureGame.Core

// ---------------------------------------------------------
// File: Monster.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

{
    // Represents a monster enemy in the game.
    public class Monster : ICharacter
    {
        // Gets the name of the monster.
        public string Name { get; private set; }

        // Gets or sets the current health of the monster.
        public int Health { get; set; }

        // Gets the attack power of the monster.
        public int AttackPower { get; private set; }

        // Initializes a new monster with specified attributes.
        public Monster(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        // Checks if the monster is still alive. Similar to player it checks for health greater than 0, else dead.
        public bool IsAlive()
        {
            return Health > 0;
        }

        // Reduces monster health by the specified damage amount. Health cant go below 0.
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
    }
}