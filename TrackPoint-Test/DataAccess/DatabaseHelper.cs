//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\DataAccess\DatabaseHelper.cs
using TrackPoint_Test.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace TrackPoint_Test.DataAccess
{
    /// <summary>
    /// Provides all methods for interacting with the SQL Server database.
    /// This is the only class in the application that should contain SQL code.
    /// </summary>
    public static class DatabaseHelper
    {
        // Reads the connection string from the App.config file once.
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Validates user credentials against the database.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A User object if credentials are valid; otherwise, null.</returns>
        /// 

        public static RfidTag GetRfidTagByEpc(string epc)
        {
            RfidTag tag = null;
            string sql = "SELECT TagEPC, DocumentID FROM RFIDTags WHERE TagEPC = @Epc";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Epc", epc);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tag = new RfidTag
                                {
                                    TagEPC = reader["TagEPC"].ToString(),
                                    // DBNull check is important here
                                    DocumentID = reader["DocumentID"] != DBNull.Value ? (int?)reader["DocumentID"] : null
                                };
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Database error fetching RfidTag: " + ex.Message);
                    }
                }
            }
            return tag;
        }

        public static User ValidateUser(string username, string password)
        {
            // --- SECURITY WARNING ---
            // In a real-world production system, you MUST NOT store plain-text passwords.
            // 1. You should store a hash of the password (e.g., using BCrypt.Net).
            // 2. This method would then hash the provided 'password' and compare it to the stored hash.
            // For this project's purpose, we are comparing directly, but this is NOT secure.
            string passwordHashForDemo = password; // In a real app: BCrypt.Hash(password);

            User user = null;

            string sql = @"
                SELECT u.UserID, u.Username, u.FullName, u.RoleID, r.RoleName, u.DepartmentID
                FROM Users u
                INNER JOIN Roles r ON u.RoleID = r.RoleID
                WHERE u.Username = @Username AND u.PasswordHash = @PasswordHash AND u.IsActive = 1";

            // The 'using' statement ensures the connection is always closed, even if errors occur.
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Use parameters to prevent SQL injection attacks. This is CRITICAL.
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHashForDemo);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching user was found
                            {
                                user = new User
                                {
                                    UserID = (int)reader["UserID"],
                                    Username = reader["Username"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    RoleID = (int)reader["RoleID"],
                                    RoleName = reader["RoleName"].ToString(),
                                    // Handle potential NULL for DepartmentID (e.g., for Super Admin)
                                    DepartmentID = reader["DepartmentID"] != DBNull.Value ? (int?)reader["DepartmentID"] : null
                                };
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // In a real application, you would log this error.
                        Console.WriteLine("Database error during login: " + ex.Message);
                        return null;
                    }
                }
            }
            return user;
        }


        /// <summary>
        /// Retrieves a list of documents based on the user's role and department.
        /// </summary>
        /// <param name="departmentId">The user's department ID. Can be null.</param>
        /// <param name="roleName">The user's role name (e.g., "Admin", "Department Head").</param>
        /// <returns>A list of Document objects.</returns>
        public static List<Document> GetDocuments(int? departmentId, string roleName)
        {
            var documents = new List<Document>();

            // Base SQL query with all necessary joins
            string sql = @"
                SELECT
                    d.DocumentID, d.DocumentTitle, d.DocumentReferenceCode, d.Description,
                    d.OwningDepartmentID, dep.DepartmentName,
                    d.StatusID, ds.StatusName,
                    d.LastKnownLocationID, loc.LocationName,
                    d.LastSeenTimestamp, d.CreatedAt
                FROM Documents d
                INNER JOIN Departments dep ON d.OwningDepartmentID = dep.DepartmentID
                INNER JOIN DocumentStatus ds ON d.StatusID = ds.StatusID
                LEFT JOIN Locations loc ON d.LastKnownLocationID = loc.LocationID";

            // Admins/Super Admins see all documents. Other roles are filtered by their department.
            bool isAdmin = roleName == "Super Admin" || roleName == "Admin";
            if (!isAdmin)
            {
                sql += " WHERE d.OwningDepartmentID = @DepartmentID";
            }

            sql += " ORDER BY d.DocumentTitle ASC";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Only add the parameter if it's needed for filtering
                    if (!isAdmin && departmentId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@DepartmentID", departmentId.Value);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var doc = new Document
                                {
                                    DocumentID = (int)reader["DocumentID"],
                                    DocumentTitle = reader["DocumentTitle"].ToString(),
                                    DocumentReferenceCode = reader["DocumentReferenceCode"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    OwningDepartmentID = (int)reader["OwningDepartmentID"],
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    StatusID = (int)reader["StatusID"],
                                    StatusName = reader["StatusName"].ToString(),
                                    LastKnownLocationID = reader["LastKnownLocationID"] != DBNull.Value ? (int?)reader["LastKnownLocationID"] : null,
                                    LocationName = reader["LocationName"] != DBNull.Value ? reader["LocationName"].ToString() : "N/A",
                                    LastSeenTimestamp = reader["LastSeenTimestamp"] != DBNull.Value ? (DateTime?)reader["LastSeenTimestamp"] : null,
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                };
                                documents.Add(doc);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Database error fetching documents: " + ex.Message);
                        // Return an empty list on error
                        return new List<Document>();
                    }
                }
            }
            return documents;
        }

        // TODO: Add other methods here as needed, such as:
        // - public static void AssignTagToDocument(string epc, int documentId) { ... }
        // - public static void LogMovement(string epc, int locationId) { ... }
        // - public static bool AddNewDocument(Document newDoc) { ... }
    }
}
