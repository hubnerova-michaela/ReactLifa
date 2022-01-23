using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthApp.Models
{
    public class Character
    {
        public Guid CharacterId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public Location BirthPlace { get; set; }
        public string ImagePath { get; set; }

        public Reality Reality { get; set; }
        [ForeignKey("Reality")]
        public Guid RealityId { get; set; }

        public Picture Picture { get; set; }
        [ForeignKey("Picture")]
        public Guid PictureId { get; set; }
    }
}
