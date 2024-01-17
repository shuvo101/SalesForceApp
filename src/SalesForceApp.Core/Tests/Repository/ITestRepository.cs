using Lib.ErrorOr;

using SalesForceApp.Core.Tests.Entity;

namespace SalesForceApp.Core.Tests.Repository;

public interface ITestRepository
{
    Task<ErrorOr<bool>> CreateMarketVisit(CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<Product>>> GetAllProductAsync(CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<Vendor>>> GetVendors(CancellationToken cancellationToken);
    Task<ErrorOr<ApplicationUser>> GetApplicationUserAsync(CancellationToken cancellationToken);
}
