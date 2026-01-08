using LostAndFound.Models;

namespace LostAndFound.UI
{
    public static class SessionManager
    {
        // Stores the currently logged-in user
        public static User CurrentUser { get; set; }

        // Check if anyone is logged in
        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        // Check if current user is admin
        public static bool IsAdmin()
        {
            return CurrentUser != null && CurrentUser.IsAdmin;
        }

        // Logout
        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}