namespace AdventureGame.Core

// ---------------------------------------------------------
// File: ICharacter.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

{
    // Interface for all characters in the game.
    // Defines the contract for combating entities.
    public interface ICharacter
    {
        // Gets and sets the current health of the character.
        int Health { get; set; }

        // Gets the attack power of the character.
        int AttackPower { get; }

        // Determines if the character is still alive. If tHe return is greater than 0, then alive. If not then dead.
        bool IsAlive();

        // Reduces the character's health by the specified amount.
        void TakeDamage(int amount);

        // Attacks another character, dealing damage based on this character's attack power.
        void Attack(ICharacter target);
    }
}