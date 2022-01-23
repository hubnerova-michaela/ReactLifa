namespace OAuthApp.Models
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public Reality Reality { get; set; }
        public Guid RealityId { get; set; }
        public Picture Picture { get; set; }
        public Guid PictureId { get; set; }
        public string ImagePath { get; set; }
    }
}
