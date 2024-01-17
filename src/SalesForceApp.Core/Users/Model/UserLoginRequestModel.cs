namespace SalesForceApp.Core.Users.Model;

public class UserLoginRequestModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}
