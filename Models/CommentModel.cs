using VegEaseBackend.Models;

public class Comment
{
    public int Id { get; set; }
    public string CommentText { get; set; }
    public int PId { get; set; } 
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Product? Product { get; set; } 
}
