using System.ComponentModel.DataAnnotations;

namespace OAuthApp.Models;

public class User
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Reality> Realities { get; set; }

}

