namespace SalesForceApp.Core.Users.Entity;

public class User
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string NormalizedUsername { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;
    public string NormalizedEmailAddress { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;
}
