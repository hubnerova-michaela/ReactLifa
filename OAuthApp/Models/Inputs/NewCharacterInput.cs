namespace OAuthApp.Models.Inputs
{
    public class NewCharacterInput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public Location BirthPlace { get; set; }
        public string ImgPath {get;set;}
        public Guid RealityId { get; set; }

    }
}
