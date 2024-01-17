
using Lib.ErrorOr;

using SalesForceApp.Core.Users.Entity;

namespace SalesForceApp.Core.Users.Repository;

public interface IUserRepository
{
    Task<ErrorOr<User>> GetUserByUserName(string userName, CancellationToken cancellationToken);
}
