using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthApp.Models
{
    public class Picture
    {
        [Key]
        public string Id { get; set; }
        public string OriginalName { get; set; } = "";
        public string ContentType { get; set; } = "";
        public DateTime Uploaded { get; set; }
        public long Size { get; set; }
        public Character Character { get; set; }
        [ForeignKey("Character")]
        public Guid CharacterId { get; set; }

        public Reality Reality { get; set; }
        [ForeignKey("Reality")]
        public Guid RealityId { get; set; }

        public Location Location { get; set; }
        [ForeignKey("Location")]
        public Guid LocationId { get; set; }
    }
}
