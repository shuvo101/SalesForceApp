using System.Data;

using MySqlConnector;

namespace Lib.DBAccess.Contexts;

public class MySqlDbContext : IAsyncDisposable
{
    private readonly MySqlConnection _dbConnection;

    public MySqlDbContext(string connectionString)
    {
        _dbConnection = new(connectionString);
    }

    public MySqlConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public async Task<MySqlTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken,
        IsolationLevel isolationLevel = IsolationLevel.Unspecified)
    {
        if (_dbConnection.State != ConnectionState.Open)
        {
            await _dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        return await _dbConnection.BeginTransactionAsync(isolationLevel: isolationLevel, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_dbConnection is null)
        {
            return;
        }

        if (_dbConnection.State == ConnectionState.Open)
        {
            await _dbConnection.CloseAsync().ConfigureAwait(false);
        }

        await _dbConnection.DisposeAsync().ConfigureAwait(false);
    }
}
