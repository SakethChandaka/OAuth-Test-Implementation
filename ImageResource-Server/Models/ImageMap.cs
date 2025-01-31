using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResourceServer_Images.Models
{
    [Table("Image_Map")] // Explicitly map to the correct table name
    public class ImageMap
    {
        [Key] // Marks UserId as the primary key
        [Column("Id")] // Maps to the column in the database
        public int Id { get; set; }

        [Required]
        [Column("ImageId")] // Maps to the "Username" column
        public string? ImageId { get; set; }

        [Required]
        [Column("ImageURL")] // Maps to the "Password" column
        public string? ImageURL { get; set; } // Store hashed passwords, not plain text!
    }
}
