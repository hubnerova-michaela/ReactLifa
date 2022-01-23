using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthApp.Models
{
    public class Reality
    {

        public Guid RealityId { get; set; }
        public string RealityName { get; set; }
        public ICollection<Character> Characters { get; set; }
        public ICollection<Location> Locations { get; set; }
        public string GeneralInfo { get; set; }
        public string ImgPath { get; set; }
        public Reality User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

    }
}
