using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResourceServer_Images.Models
{
    [Table("Image_Access_Data")] // Explicitly map to the correct table name
    public class AccessMap
    {
        [Key] // Marks UserId as the primary key
        [Column("S.No")]
        public int SNo { get; set; }

        [Required]
        [Column("ImageId")] 
        public string? ImageId { get; set; }

        [Required]
        [Column("User")]
        public string? User { get; set; }

        [Required]
        [Column("ClientId")]
        public string? ClientId { get; set; }

        [Required]
        [Column("AccessedTime")]
        public DateTimeOffset? AccessTime { get; set; }
    }
}
