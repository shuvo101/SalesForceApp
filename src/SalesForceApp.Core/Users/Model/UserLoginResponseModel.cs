using SalesForceApp.Core.Users.Entity;

using Mapster;

namespace SalesForceApp.Core.Users.Model;

public class UserLoginResponseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int AccessTokenExpireInMinutes { get; set; }

    public UserModel? UserModel { get; set; }

    public class UserLoginResponseModelMapperConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<User, UserLoginResponseModel>()
                .Map(x => x.UserModel, y => y)
                .TwoWays();
        }
    }
}

