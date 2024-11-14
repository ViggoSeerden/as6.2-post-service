namespace PostServiceBusiness.Models;

public class Post
{
    public Guid Id { get; set; }
    
    public Guid Poster { get; set; }

    public string City { get; set; }
    
    public string Street { get; set; }
    
    public string Description { get; set; }
}