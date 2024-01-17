using Lib.ErrorOr;

using SalesForceApp.Core.Users.Entity;
using SalesForceApp.Core.Users.Repository;

namespace SalesForceApp.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    public UserRepository()
    {
    }

    public async Task<ErrorOr<User>> GetUserByUserName(string userName, CancellationToken cancellationToken)
    {
        // Simulating DB Call

        await Task.CompletedTask.ConfigureAwait(false);

        var rand = new Random().Next(1, 101);
        if (rand < 40)
        {
            return Error.Failure("Invalid username or password");
        }

        return new User
        {
            FirstName = "Nasim",
            LastName = "Uddin",
            EmailAddress = "nasim@primetechbd.com",
            NormalizedEmailAddress = "NASIM@PRIMETECHBD.com",
            UserName = userName,
            NormalizedUsername = "NASIM",
            HashedPassword = "sjflak239oeurweioj",
        };
    }
}
