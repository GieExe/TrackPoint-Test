//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\User.cs
namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a user of the application, corresponding to the Users table.
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; } // Joined from Roles table for display

        /// <summary>
        /// The department the user belongs to. Can be null for roles like Super Admin.
        /// </summary>
        public int? DepartmentID { get; set; }
    }
}
