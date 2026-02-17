namespace AdventureGame.Core
// ---------------------------------------------------------
// File: Item.cs
// Author: Jake Littlejohn
// Created: 2-8-2025
// Last Modified: 2-16-2025
// ---------------------------------------------------------

{
    // Base class for all items in the game. Defines all the common properties that tghe items share.
    public abstract class Item
    {
        // Gets the name of the item.
        public string Name { get; protected set; }

        // Gets the message displayed when the item is picked up.
        public string PickupMessage { get; protected set; }

        // Initializes a new item with a name and pickup message.
        protected Item(string name, string pickupMessage)
        {
            Name = name;
            PickupMessage = pickupMessage;
        }
    }
}