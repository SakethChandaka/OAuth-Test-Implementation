using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Server.Models
{
    [Table("Authorization_Grant")] // Explicitly map to the correct table name
    public class AuthGrant
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("clientId")] 
        public string ClientId { get; set; } = string.Empty;

        [Required]
        [Column("clientSecret")] 
        public string ClientSecret { get; set; } = string.Empty;

        [Required]
        [Column("authGrant")]
        public string AuthGrantCode { get; set; } = string.Empty;

        [Required]
        [Column("createdTime")]
        public DateTimeOffset? CreatedTime { get; set; } = DateTimeOffset.UtcNow;// Default to current time

        [Required]
        [Column("expiryTime")]
        public DateTimeOffset? ExpiryTime { get; set; } // Expiry time of the authorization grant

        [Required]
        [Column("isValid")]
        public bool IsValid { get; set; } = true;
    }
}
