using Polly;
using Polly.Retry;

namespace SalesForceApp.Core.Configurations.ResiliencePipelines;

public static class DefaultHttpPipeline
{
    private const int MaxRetryAttempts = 3;
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
    private static readonly RetryStrategyOptions RetryStrategyOptions = new()
    {
        ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        MaxRetryAttempts = MaxRetryAttempts,
        DelayGenerator = static args =>
        {
            var delay = args.AttemptNumber switch
            {
                0 => TimeSpan.Zero,
                1 => TimeSpan.FromSeconds(2),
                _ => TimeSpan.FromSeconds(5)
            };

            return new ValueTask<TimeSpan?>(delay);
        },
    };

    public readonly static ResiliencePipeline Pipeline = new ResiliencePipelineBuilder()
        .AddRetry(RetryStrategyOptions)
        .AddTimeout(DefaultTimeout)
        .Build();
}
