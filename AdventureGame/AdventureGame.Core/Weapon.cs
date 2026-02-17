namespace AdventureGame.Core
// ---------------------------------------------------------
// File: Weapon.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

{
    // Represents a weapon item that increases player attack power. Like potion it inherits from Item base class.
    public class Weapon : Item
    {
        // Gets the attack damage modifier this weapon provides.
        public int Modifier { get; private set; }

        // Initializes a new weapon with specified name and damage modifier.
        public Weapon(string name, int modifier)
            : base(name, $"You found a {name}! +{modifier} Attack Power")
        {
            Modifier = modifier;
        }
    }
}