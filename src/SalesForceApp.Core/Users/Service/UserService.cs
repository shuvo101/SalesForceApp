using SalesForceApp.Core.Configurations.Helpers;
using System.Security.Claims;

using SalesForceApp.Core.Configurations.UnitOfWorks;
using SalesForceApp.Core.Users.Model;
using SalesForceApp.Core.Users.Repository;

using FluentValidation;

using Mapster;
using Lib.ErrorOr;

namespace SalesForceApp.Core.Users.Service;

public partial class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenHelper _jwtTokenHelper;
    private readonly IValidator<UserLoginRequestModel> _userLoginRequestModelValidator;

    public UserService(
        IUnitOfWork unitOfWork,
        IJwtTokenHelper jwtTokenHelper,
        IValidator<UserLoginRequestModel> userLoginRequestModelValidator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenHelper = jwtTokenHelper;
        _userLoginRequestModelValidator = userLoginRequestModelValidator;
    }

    public static IEnumerable<Claim> GetUserClaims(long userId)
    {
        return
        [
            new(CustomClaimTypes.UserId, value: userId.ToString(provider: null)),
        ];
    }

    public async Task<ErrorOr<UserLoginResponseModel>> LoginAsync(UserLoginRequestModel request, CancellationToken cancellationToken)
    {
        // Validate and transform
        _userLoginRequestModelValidator.ValidateAndThrow(request); // Use async version if validator uses any async methods
        request.UserName = request.UserName.Trim().ToUpperInvariant();
        request.Password = request.Password.Trim();

        var userRepository = _unitOfWork.Repository<IUserRepository>();
        var userResult = await userRepository.GetUserByUserName(request.UserName, cancellationToken).ConfigureAwait(false);
        if (userResult.IsError)
        {
            return userResult.Error;
        }

        var user = userResult.Value;
        var result = user.Adapt<UserLoginResponseModel>();

        result.AccessToken = _jwtTokenHelper.GenerateNewToken(GetUserClaims(user.Id));
        result.RefreshToken = _jwtTokenHelper.GenerateNewRefreshToken().ToString();
        result.AccessTokenExpireInMinutes = _jwtTokenHelper.JwtSettings.AccessTokenExpireInMinutes;

        return result;
    }
}
