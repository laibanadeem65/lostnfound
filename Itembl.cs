using System;
using System.Collections.Generic;
using LostAndFound.Models;
using LostAndFound.DL;

namespace LostAndFound.BL
{
    public class ItemBL
    {
        // Add a new item with validation
        public static string AddItem(string itemName, string description, string category, string itemType, string location, int ownerId, string imagePath)
        {
            // Validation 1: Check required fields
            if (string.IsNullOrWhiteSpace(itemName))
                return "Item name cannot be empty.";

            if (string.IsNullOrWhiteSpace(itemType))
                return "Please select item type (Lost or Found).";

            // Validation 2: Check item type is valid
            if (itemType != "Lost" && itemType != "Found")
                return "Item type must be either 'Lost' or 'Found'.";

            // Create Item object
            Item newItem = new Item
            {
                ItemName = itemName,
                Description = description,
                Category = category,
                ItemType = itemType,
                Location = location,
                OwnerId = ownerId,
                ImagePath = imagePath,
                Status = "Active",
                DateReported = DateTime.Now
            };

            // Add to database
            bool success = ItemDL.AddItem(newItem);

            if (success)
                return "SUCCESS";
            else
                return "Failed to add item. Please try again.";
        }

        // Get all items
        public static List<Item> GetAllItems()
        {
            return ItemDL.GetAllItems();
        }

        // Get items by owner (for user profile)
        public static List<Item> GetUserItems(int userId)
        {
            return ItemDL.GetItemsByOwner(userId);
        }

        // Update item with validation and permission check
        public static string UpdateItem(User currentUser, int itemId, string itemName, string description, string category, string itemType, string location, string status, int itemOwnerId, string imagePath)
        {
            // Business rule: Check if user has permission to edit
            if (currentUser == null)
                return "You must be logged in to edit items.";

            // Business rule: User can only edit their own items, unless they're admin
            if (!currentUser.IsAdmin && currentUser.UserId != itemOwnerId)
                return "You can only edit your own items.";

            // Validation: Check required fields
            if (string.IsNullOrWhiteSpace(itemName))
                return "Item name cannot be empty.";

            if (itemType != "Lost" && itemType != "Found")
                return "Item type must be either 'Lost' or 'Found'.";

            if (status != "Active" && status != "Resolved" && status != "Closed")
                return "Invalid status selected.";

            // Create updated Item object
            Item updatedItem = new Item
            {
                ItemId = itemId,
                ItemName = itemName,
                Description = description,
                Category = category,
                ItemType = itemType,
                Location = location,
                Status = status,
                ImagePath = imagePath
            };

            // Update in database
            bool success = ItemDL.UpdateItem(updatedItem);

            if (success)
                return "SUCCESS";
            else
                return "Failed to update item. Please try again.";
        }

        // Delete item with permission check
        public static string DeleteItem(User currentUser, int itemId, int itemOwnerId)
        {
            // Business rule: Check if user has permission to delete
            if (currentUser == null)
                return "You must be logged in to delete items.";

            // Business rule: User can only delete their own items, unless they're admin
            if (!currentUser.IsAdmin && currentUser.UserId != itemOwnerId)
                return "You can only delete your own items.";

            // Delete from database
            bool success = ItemDL.DeleteItem(itemId);

            if (success)
                return "SUCCESS";
            else
                return "Failed to delete item. Please try again.";
        }

        // Search items by keyword
        public static List<Item> SearchItems(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return ItemDL.GetAllItems(); // Return all if no keyword

            return ItemDL.SearchItems(keyword);
        }

        // Get item statistics for user profile
        public static Dictionary<string, int> GetUserItemStats(int userId)
        {
            return ItemDL.GetItemStatsByOwner(userId);
        }

        // Filter items by type (Lost or Found)
        public static List<Item> FilterItemsByType(string itemType)
        {
            List<Item> allItems = ItemDL.GetAllItems();
            List<Item> filteredItems = new List<Item>();

            foreach (Item item in allItems)
            {
                if (item.ItemType == itemType)
                {
                    filteredItems.Add(item);
                }
            }

            return filteredItems;
        }

        // Filter items by status
        public static List<Item> FilterItemsByStatus(string status)
        {
            List<Item> allItems = ItemDL.GetAllItems();
            List<Item> filteredItems = new List<Item>();

            foreach (Item item in allItems)
            {
                if (item.Status == status)
                {
                    filteredItems.Add(item);
                }
            }

            return filteredItems;
        }
    }
}