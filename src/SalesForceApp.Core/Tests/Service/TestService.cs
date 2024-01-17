using System.Net.Http.Json;

using Lib.ErrorOr;

using SalesForceApp.Core.Configurations.Constants;
using SalesForceApp.Core.Configurations.ResiliencePipelines;
using SalesForceApp.Core.Configurations.UnitOfWorks;
using SalesForceApp.Core.Tests.Entity;
using SalesForceApp.Core.Tests.Repository;

namespace SalesForceApp.Core.Tests.Service;

public class TestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpClientFactory _httpClientFactory;

    public TestService(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
    {
        _unitOfWork = unitOfWork;
        _httpClientFactory = httpClientFactory;
    }

    public Task<ErrorOr<ApplicationUser>> GetApplicationUserAsync(CancellationToken cancellationToken)
    {
        return _unitOfWork.Repository<ITestRepository>().GetApplicationUserAsync(cancellationToken);
    }

    public Task<ErrorOr<IEnumerable<Product>>> GetAllProductAsync(CancellationToken cancellationToken)
    {
        return _unitOfWork.Repository<ITestRepository>().GetAllProductAsync(cancellationToken);
    }

    public Task<ErrorOr<IEnumerable<Vendor>>> GetVendors(CancellationToken cancellationToken)
    {
        return _unitOfWork.Repository<ITestRepository>().GetVendors(cancellationToken);
    }

    public Task<ErrorOr<bool>> CreateMarketVisit(CancellationToken cancellationToken)
    {
        return _unitOfWork.Repository<ITestRepository>().CreateMarketVisit(cancellationToken);
    }

    public async Task<ErrorOr<IEnumerable<Post>>> GetPostsAsync(CancellationToken cancellationToken)
    {
        const string url = "/posts";

        var client = _httpClientFactory.CreateClient(HttpClientKey.JsonPlaceHolder);
        var result = await DefaultHttpPipeline.Pipeline.ExecuteAsync(
            async _ => await client.GetFromJsonAsync<IEnumerable<Post>>(url, cancellationToken: cancellationToken).ConfigureAwait(false),
            cancellationToken)
        .ConfigureAwait(false);

        return ErrorOr.From(result ?? Enumerable.Empty<Post>());
    }
}
