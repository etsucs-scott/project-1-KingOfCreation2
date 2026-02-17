// ---------------------------------------------------------
// File: Potion.cs
// Author: Jake Littlejohn
// Created: 2-8-2025   
// Last Modified: 2-16-2025
// ---------------------------------------------------------

namespace AdventureGame.Core
{
    // Represents a healing potion item. Like weapon it inherits from Item base class.
    public class Potion : Item
    {
        // Constant for how much health potions restore
        private const int POTION_HEAL_AMOUNT = 30;

        // Gets the amount of health this potion restores.
        public int HealAmount { get; private set; }

        // Initializes potion.
        public Potion()
            : base("Healing Potion", $"You found a Health Pot! +{POTION_HEAL_AMOUNT} HP")
        {
            HealAmount = POTION_HEAL_AMOUNT;
        }
    }
}