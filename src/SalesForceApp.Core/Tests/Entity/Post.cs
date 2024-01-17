namespace SalesForceApp.Core.Tests.Entity;

public class Post
{
    public long UserId { get; set; }
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
