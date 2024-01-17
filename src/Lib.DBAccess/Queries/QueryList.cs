using System.Data;
using System.Data.Common;
using System.Globalization;

using Lib.DBAccess.Contexts;
using Lib.DBAccess.Helpers;
using Lib.DBAccess.Model;

using MySqlConnector;

using Oracle.ManagedDataAccess.Client;

namespace Lib.DBAccess.Queries;

public static class QueryList
{
    #region Oracle
    public static Task<DatabaseQueryResponse<List<T>>> QueryListAsync<T>(
        this OracleDbContext dbContext,
        string procedureName,
        IEnumerable<OracleParameter> parameters,
        Action<DbDataReader, T> mapAction,
        CancellationToken cancellationToken,
        OracleTransaction? transaction = null)
        where T : class, new()
    {
        var connection = dbContext.GetDbConnection();
        using var command = connection.CreateCommand();

        CommonHelper.SetupOracleCommandWithParameters(command, procedureName, parameters, transaction);

        return ExecuteListAsync(connection, command, mapAction, cancellationToken);
    }

    public static Task<DatabaseQueryResponse<List<T>>> QueryListAsync<T>(
        this OracleDbContext dbContext,
        string procedureName,
        IEnumerable<OracleParameter> parameters,
        Action<DataRow, T> mapAction,
        CancellationToken cancellationToken,
        OracleTransaction? transaction = null)
        where T : class, new()
    {
        var connection = dbContext.GetDbConnection();
        using var command = connection.CreateCommand();

        CommonHelper.SetupOracleCommandWithParameters(command, procedureName, parameters, transaction);

        return ExecuteListAsync(connection, command, mapAction, cancellationToken);
    }

    private static async Task<DatabaseQueryResponse<List<T>>> ExecuteListAsync<T>(
        DbConnection connection,
        OracleCommand command,
        Action<DbDataReader, T> mapAction,
        CancellationToken cancellationToken)
        where T : class, new()
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await using (reader.ConfigureAwait(false))
            {
                var result = new List<T>();

                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    var data = new T();
                    mapAction.Invoke(reader, data);

                    result.Add(data);
                }

                var errorMessage = command.Parameters[DefaultConstants.ErrorMessageParameter].Value?.ToString();
                _ = int.TryParse(command.Parameters[DefaultConstants.ErrorCodeParameter].Value?.ToString(), NumberStyles.None, provider: null, out var errorCode);

                return new DatabaseQueryResponse<List<T>>
                {
                    Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                    ErrorMessage = errorMessage,
                    ErrorCode = errorCode,
                    Data = result,
                };
            }
        }
        catch (Exception ex)
        {
            return DatabaseQueryResponse.Failure<List<T>>(ex.Message);
        }
    }

    private static async Task<DatabaseQueryResponse<List<T>>> ExecuteListAsync<T>(
        DbConnection connection,
        OracleCommand command,
        Action<DataRow, T> mapAction,
        CancellationToken cancellationToken)
        where T : class, new()
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            var result = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                var data = new T();
                mapAction.Invoke(row, data);

                result.Add(data);
            }

            var errorMessage = command.Parameters[DefaultConstants.ErrorMessageParameter].Value?.ToString();
            _ = int.TryParse(command.Parameters[DefaultConstants.ErrorCodeParameter].Value?.ToString(), NumberStyles.None, provider: null, out var errorCode);

            return new DatabaseQueryResponse<List<T>>
            {
                Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return DatabaseQueryResponse.Failure<List<T>>(ex.Message);
        }
    }
    #endregion

    #region MySQL
    public static Task<DatabaseQueryResponse<List<T>>> QueryListAsync<T>(
        this MySqlDbContext dbContext,
        string procedureName,
        IEnumerable<MySqlParameter> parameters,
        Action<DbDataReader, T> mapAction,
        CancellationToken cancellationToken,
        MySqlTransaction? transaction = null)
        where T : class, new()
    {
        var connection = dbContext.GetDbConnection();
        var command = connection.CreateCommand();

        CommonHelper.SetupMySqlCommandWithParameters(command, procedureName, parameters, transaction);

        return ExecuteListAsync(connection, command, mapAction, cancellationToken);
    }

    public static Task<DatabaseQueryResponse<List<T>>> QueryListAsync<T>(
        this MySqlDbContext dbContext,
        string procedureName,
        IEnumerable<MySqlParameter> parameters,
        Action<DataRow, T> mapAction,
        CancellationToken cancellationToken,
        MySqlTransaction? transaction = null)
        where T : class, new()
    {
        var connection = dbContext.GetDbConnection();
        var command = connection.CreateCommand();

        CommonHelper.SetupMySqlCommandWithParameters(command, procedureName, parameters, transaction);

        return ExecuteListAsync(connection, command, mapAction, cancellationToken);
    }

    private static async Task<DatabaseQueryResponse<List<T>>> ExecuteListAsync<T>(
        DbConnection connection,
        MySqlCommand command,
        Action<DbDataReader, T> mapAction,
        CancellationToken cancellationToken)
        where T : class, new()
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await using (reader.ConfigureAwait(false))
            {
                var result = new List<T>();

                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    var data = new T();
                    mapAction.Invoke(reader, data);

                    result.Add(data);
                }

                var errorMessage = command.Parameters[DefaultConstants.ErrorMessageParameter].Value?.ToString();
                _ = int.TryParse(command.Parameters[DefaultConstants.ErrorCodeParameter].Value?.ToString(), NumberStyles.None, provider: null, out var errorCode);

                return new DatabaseQueryResponse<List<T>>
                {
                    Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                    ErrorMessage = errorMessage,
                    ErrorCode = errorCode,
                    Data = result,
                };
            }
        }
        catch (Exception ex)
        {
            return DatabaseQueryResponse.Failure<List<T>>(ex.Message);
        }
    }

    private static async Task<DatabaseQueryResponse<List<T>>> ExecuteListAsync<T>(
        DbConnection connection,
        MySqlCommand command,
        Action<DataRow, T> mapAction,
        CancellationToken cancellationToken)
        where T : class, new()
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            var result = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                var data = new T();
                mapAction.Invoke(row, data);

                result.Add(data);
            }

            var errorMessage = command.Parameters[DefaultConstants.ErrorMessageParameter].Value?.ToString();
            _ = int.TryParse(command.Parameters[DefaultConstants.ErrorCodeParameter].Value?.ToString(), NumberStyles.None, provider: null, out var errorCode);

            return new DatabaseQueryResponse<List<T>>
            {
                Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return DatabaseQueryResponse.Failure<List<T>>(ex.Message);
        }
    }
    #endregion
}
