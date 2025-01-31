using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Server.Models
{
    [Table("User_Auth")] // Explicitly map to the correct table name
    public class UserAuth
    {
        [Key] // Marks UserId as the primary key
        [Column("UserId")] // Maps to the column in the database
        public int UserId { get; set; }

        [Required]
        [Column("Username")] // Maps to the "Username" column
        public string? Username { get; set; }

        [Required]
        [Column("Password")] // Maps to the "Password" column
        public string? Password { get; set; } // Store hashed passwords, not plain text!

        [Required]
        [Column("Email")]
        public string? Email { get; set; }

        [Column("LastUpdatedTime")]
        public DateTimeOffset? LastUpdatedTime { get; set; } // Nullable in case it's not always updated

        [Required]
        [Column("CreatedTime")]
        public DateTimeOffset? CreatedTime { get; set; }
    }
}
