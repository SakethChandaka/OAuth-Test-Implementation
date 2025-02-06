using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Server.Models
{
    [Table("Client_Auth")] // Explicitly map to the correct table name
    public class ClientAuth
    {
        [Key]
        [Column("Id")] 
        public int SerielId { get; set; }

        [Required]
        [Column("ClientId")] 
        public string ClientId { get; set; } = string.Empty;

        [Required]
        [Column("ClientSecret")] 
        public string ClientSecret { get; set; } = string.Empty;

        [Required]
        [Column("Name")]
        public string Name { get; set; } = string.Empty;
    }
}
