using System.Data;

using Oracle.ManagedDataAccess.Client;

namespace Lib.DBAccess.Contexts;

public class OracleDbContext : IAsyncDisposable
{
    private readonly OracleConnection _dbConnection;

    public OracleDbContext(string connectionString)
    {
        _dbConnection = new(connectionString);
    }

    public OracleConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public async Task<OracleTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken,
        IsolationLevel isolationLevel = IsolationLevel.Unspecified)
    {
        if (_dbConnection.State != ConnectionState.Open)
        {
            await _dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        return _dbConnection.BeginTransaction(isolationLevel);
    }

    public async ValueTask DisposeAsync()
    {
        if (_dbConnection is null)
        {
            return;
        }

        if (_dbConnection.State == System.Data.ConnectionState.Open)
        {
            await _dbConnection.CloseAsync().ConfigureAwait(false);
        }

        await _dbConnection.DisposeAsync().ConfigureAwait(false);
    }
}
