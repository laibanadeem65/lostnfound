using System;
using System.Collections.Generic;
using LostAndFound.Models;
using LostAndFound.DL;

namespace LostAndFound.BL
{
    public class UserBL
    {
        // Register a new user with validation
        public static string RegisterUser(string username, string password, string confirmPassword, string email, string phoneNumber)
        {
            // Validation 1: Check if fields are empty
            if (string.IsNullOrWhiteSpace(username))
                return "Username cannot be empty.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty.";

            if (string.IsNullOrWhiteSpace(email))
                return "Email cannot be empty.";

            // Validation 2: Check password length
            if (password.Length < 6)
                return "Password must be at least 6 characters long.";

            // Validation 3: Check if passwords match
            if (password != confirmPassword)
                return "Passwords do not match.";

            // Validation 4: Check if username already exists
            if (UserDL.UsernameExists(username))
                return "Username already exists. Please choose another.";

            // Validation 5: Basic email format check
            if (!email.Contains("@") || !email.Contains("."))
                return "Please enter a valid email address.";

            // Create User object
            User newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                PhoneNumber = phoneNumber,
                IsAdmin = false // New users are regular users by default
            };

            // Try to register in database
            bool success = UserDL.RegisterUser(newUser);

            if (success)
                return "SUCCESS";
            else
                return "Registration failed. Please try again.";
        }

        // Login user with validation
        public static User LoginUser(string username, string password)
        {
            // Validation: Check if fields are empty
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            // Authenticate user from database
            User user = UserDL.AuthenticateUser(username, password);

            return user; // Returns User object if found, null if not
        }

        // Update user profile with validation
        public static string UpdateUserProfile(int userId, string newPassword, string confirmPassword, string email, string phoneNumber)
        {
            // Validation 1: Check password if user wants to change it
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length < 6)
                    return "Password must be at least 6 characters long.";

                if (newPassword != confirmPassword)
                    return "Passwords do not match.";
            }

            // Validation 2: Email format
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!email.Contains("@") || !email.Contains("."))
                    return "Please enter a valid email address.";
            }

            // Get existing user to preserve username
            User updatedUser = new User
            {
                UserId = userId,
                Password = newPassword,
                Email = email,
                PhoneNumber = phoneNumber
            };

            // Update in database
            bool success = UserDL.UpdateUser(updatedUser);

            if (success)
                return "SUCCESS";
            else
                return "Update failed. Please try again.";
        }

        // Get all users (Admin only)
        public static List<User> GetAllUsers(User currentUser)
        {
            // Business rule: Only admins can view all users
            if (currentUser == null || !currentUser.IsAdmin)
                return new List<User>(); // Return empty list if not admin

            return UserDL.GetAllUsers();
        }

        // Change user role (Admin only)
        public static string ChangeUserRole(User currentUser, int targetUserId, bool makeAdmin)
        {
            // Business rule: Only admins can change roles
            if (currentUser == null || !currentUser.IsAdmin)
                return "You do not have permission to change user roles.";

            // Business rule: Admin cannot change their own role
            if (currentUser.UserId == targetUserId)
                return "You cannot change your own role.";

            bool success = UserDL.ChangeUserRole(targetUserId, makeAdmin);

            if (success)
                return "SUCCESS";
            else
                return "Failed to change user role.";
        }

    

    // Get user by ID
public static User GetUserById(int userId)
        {
            return UserDL.GetUserById(userId);
        }
    }
}