using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LostAndFound.Models;

namespace LostAndFound.DL
{
    public class ItemDL
    {
        // Add a new item (Report Lost/Found)
        public static bool AddItem(Item item)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Items (ItemName, Description, Category, ItemType, Location, Status, OwnerId, ImagePath) 
                                   VALUES (@ItemName, @Description, @Category, @ItemType, @Location, @Status, @OwnerId, @ImagePath)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmd.Parameters.AddWithValue("@Description", item.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Category", item.Category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ItemType", item.ItemType);
                        cmd.Parameters.AddWithValue("@Location", item.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", item.Status);
                        cmd.Parameters.AddWithValue("@OwnerId", item.OwnerId);
                        cmd.Parameters.AddWithValue("@ImagePath", item.ImagePath ?? (object)DBNull.Value);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding item: " + ex.Message);
                return false;
            }
        }

        // Get all items (for search/dashboard)
        public static List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT i.*, u.Username as OwnerUsername 
                                   FROM Items i 
                                   INNER JOIN Users u ON i.OwnerId = u.UserId 
                                   ORDER BY i.DateReported DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new Item
                                {
                                    ItemId = Convert.ToInt32(reader["ItemId"]),
                                    ItemName = reader["ItemName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Category = reader["Category"].ToString(),
                                    ItemType = reader["ItemType"].ToString(),
                                    Location = reader["Location"].ToString(),
                                    DateReported = Convert.ToDateTime(reader["DateReported"]),
                                    Status = reader["Status"].ToString(),
                                    OwnerId = Convert.ToInt32(reader["OwnerId"]),
                                    ImagePath = reader["ImagePath"].ToString(),
                                    OwnerUsername = reader["OwnerUsername"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting all items: " + ex.Message);
            }
            return items;
        }

        // Get items by owner (for user's profile/gallery)
        public static List<Item> GetItemsByOwner(int ownerId)
        {
            List<Item> items = new List<Item>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Items WHERE OwnerId = @OwnerId ORDER BY DateReported DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OwnerId", ownerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new Item
                                {
                                    ItemId = Convert.ToInt32(reader["ItemId"]),
                                    ItemName = reader["ItemName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Category = reader["Category"].ToString(),
                                    ItemType = reader["ItemType"].ToString(),
                                    Location = reader["Location"].ToString(),
                                    DateReported = Convert.ToDateTime(reader["DateReported"]),
                                    Status = reader["Status"].ToString(),
                                    OwnerId = Convert.ToInt32(reader["OwnerId"]),
                                    ImagePath = reader["ImagePath"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting items by owner: " + ex.Message);
            }
            return items;
        }

        // Update an item
        public static bool UpdateItem(Item item)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Items 
                                   SET ItemName = @ItemName, Description = @Description, Category = @Category, 
                                       ItemType = @ItemType, Location = @Location, Status = @Status, ImagePath = @ImagePath 
                                   WHERE ItemId = @ItemId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                        cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmd.Parameters.AddWithValue("@Description", item.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Category", item.Category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ItemType", item.ItemType);
                        cmd.Parameters.AddWithValue("@Location", item.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", item.Status);
                        cmd.Parameters.AddWithValue("@ImagePath", item.ImagePath ?? (object)DBNull.Value);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating item: " + ex.Message);
                return false;
            }
        }

        // Delete an item
        public static bool DeleteItem(int itemId)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Items WHERE ItemId = @ItemId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemId", itemId);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item: " + ex.Message);
                return false;
            }
        }

        // Search items by keyword
        public static List<Item> SearchItems(string keyword)
        {
            List<Item> items = new List<Item>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT i.*, u.Username as OwnerUsername 
                                   FROM Items i 
                                   INNER JOIN Users u ON i.OwnerId = u.UserId 
                                   WHERE i.ItemName LIKE @Keyword OR i.Description LIKE @Keyword OR i.Category LIKE @Keyword 
                                   ORDER BY i.DateReported DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new Item
                                {
                                    ItemId = Convert.ToInt32(reader["ItemId"]),
                                    ItemName = reader["ItemName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Category = reader["Category"].ToString(),
                                    ItemType = reader["ItemType"].ToString(),
                                    Location = reader["Location"].ToString(),
                                    DateReported = Convert.ToDateTime(reader["DateReported"]),
                                    Status = reader["Status"].ToString(),
                                    OwnerId = Convert.ToInt32(reader["OwnerId"]),
                                    ImagePath = reader["ImagePath"].ToString(),
                                    OwnerUsername = reader["OwnerUsername"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching items: " + ex.Message);
            }
            return items;
        }

        // Get item count by type for user stats (Lost vs Found)
        public static Dictionary<string, int> GetItemStatsByOwner(int ownerId)
        {
            Dictionary<string, int> stats = new Dictionary<string, int>
            {
                { "Lost", 0 },
                { "Found", 0 }
            };

            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT ItemType, COUNT(*) as Count FROM Items WHERE OwnerId = @OwnerId GROUP BY ItemType";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OwnerId", ownerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemType = reader["ItemType"].ToString();
                                int count = Convert.ToInt32(reader["Count"]);
                                stats[itemType] = count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting item stats: " + ex.Message);
            }
            return stats;
        }
    }
}