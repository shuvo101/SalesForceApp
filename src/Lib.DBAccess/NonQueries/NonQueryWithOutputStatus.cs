using System.Data;
using System.Globalization;

using Lib.DBAccess.Contexts;
using Lib.DBAccess.Helpers;
using Lib.DBAccess.Model;

using MySqlConnector;

using Oracle.ManagedDataAccess.Client;

namespace Lib.DBAccess.NonQueries;

public static class NonQueryWithOutputStatus
{
    #region Oracle
    public static Task<DatabaseNonQueryResponseWithOutputStatus> ExecuteNonQueryWithOutputStatusAsync(
        this OracleDbContext dbContext,
        string procedureName,
        IEnumerable<OracleParameter> parameters,
        CancellationToken cancellationToken,
        OracleTransaction? transaction = null)
    {
        var connection = dbContext.GetDbConnection();
        using var command = connection.CreateCommand();

        CommonHelper.SetupOracleCommandWithParameters(command, procedureName, parameters, transaction);

        return ExecuteNonQuery(connection, command, cancellationToken);
    }

    private static async Task<DatabaseNonQueryResponseWithOutputStatus> ExecuteNonQuery(
        OracleConnection connection,
        OracleCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            var errorMessage = command.Parameters[DefaultConstants.ErrorMessageParameter].Value?.ToString();
            _ = int.TryParse(command.Parameters[DefaultConstants.ErrorCodeParameter].Value?.ToString(), NumberStyles.None, provider: null, out var errorCode);
            _ = int.TryParse(command.Parameters[DefaultConstants.OutputParameter].Value?.ToString(), NumberStyles.None, provider: null, out var outputStatus);

            return new DatabaseNonQueryResponseWithOutputStatus
            {
                Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                OutputStatus = outputStatus,
            };
        }
        catch (Exception ex)
        {
            return DatabaseNonQueryResponseWithOutputStatus.Failure(ex.Message);
        }
    }
    #endregion

    #region MySQL
    public static Task<DatabaseNonQueryResponseWithOutputStatus> ExecuteNonQueryWithOutputStatusAsync(
        this MySqlDbContext dbContext,
        string procedureName,
        IEnumerable<MySqlParameter> parameters,
        CancellationToken cancellationToken,
        MySqlTransaction? transaction = null)
    {
        var connection = dbContext.GetDbConnection();
        var command = connection.CreateCommand();

        CommonHelper.SetupMySqlCommandWithParameters(command, procedureName, parameters, transaction);

        return ExecuteNonQuery(connection, command, cancellationToken);
    }

    private static async Task<DatabaseNonQueryResponseWithOutputStatus> ExecuteNonQuery(
        MySqlConnection connection,
        MySqlCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            var errorMessage = command.Parameters[DefaultConstants.ErrorMessageParameter].Value?.ToString();
            _ = int.TryParse(command.Parameters[DefaultConstants.ErrorCodeParameter].Value?.ToString(), NumberStyles.None, provider: null, out var errorCode);
            _ = int.TryParse(command.Parameters[DefaultConstants.OutputParameter].Value?.ToString(), NumberStyles.None, provider: null, out var outputStatus);

            return new DatabaseNonQueryResponseWithOutputStatus
            {
                Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                OutputStatus = outputStatus,
            };
        }
        catch (Exception ex)
        {
            return DatabaseNonQueryResponseWithOutputStatus.Failure(ex.Message);
        }
    }
    #endregion
}
