using LostAndFound.Models;
using System;

namespace LostAndFound.Models
{
    public class Item
    {
        // Properties matching the Items table columns
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ItemType { get; set; } // "Lost" or "Found"
        public string Location { get; set; }
        public DateTime DateReported { get; set; }
        public string Status { get; set; } // "Active", "Resolved", or "Closed"
        public int OwnerId { get; set; }
        public string ImagePath { get; set; }

        // Navigation property - to link to the User who owns this item
        public string OwnerUsername { get; set; }

        // Constructor
        public Item()
        {
            DateReported = DateTime.Now;
            Status = "Active";
            ItemType = "Lost"; // Default to Lost
        }

        // Constructor with parameters
        public Item(string itemName, string description, string itemType, int ownerId)
        {
            ItemName = itemName;
            Description = description;
            ItemType = itemType;
            OwnerId = ownerId;
            DateReported = DateTime.Now;
            Status = "Active";
        }
    }
}
