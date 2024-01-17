using SalesForceApp.Core.Users.Entity;

using Mapster;

namespace SalesForceApp.Core.Users.Model;

public class UserModel
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string FullName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;

    public class UserModelMapperConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<User, UserModel>()
                .Map(x => x.FullName, y => $"{y.FirstName}_{y.LastName}") // Example of mapping. You should not do this. Keep your logic in Entity class
                .TwoWays();
        }
    }
}
