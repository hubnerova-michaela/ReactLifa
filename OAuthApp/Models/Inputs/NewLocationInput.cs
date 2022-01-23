namespace OAuthApp.Models.Inputs
{
    public class NewLocationInput
    {
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public Guid RealityId { get; set; }
    }
}
