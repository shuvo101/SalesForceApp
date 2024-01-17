using System.Data;
using System.Globalization;

using Lib.DBAccess.Contexts;
using Lib.DBAccess.Helpers;
using Lib.DBAccess.Model;

using MySqlConnector;

using Oracle.ManagedDataAccess.Client;

namespace Lib.DBAccess.NonQueries;

public static class NonQuery
{
    #region Oracle
    public static Task<DatabaseNonQueryResponse> ExecuteNonQueryAsync(
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

    private static async Task<DatabaseNonQueryResponse> ExecuteNonQuery(
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

            return new DatabaseNonQueryResponse
            {
                Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
            };
        }
        catch (Exception ex)
        {
            return DatabaseNonQueryResponse.Failure(ex.Message);
        }
    }
    #endregion

    #region MySQL
    public static Task<DatabaseNonQueryResponse> ExecuteNonQueryAsync(
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

    private static async Task<DatabaseNonQueryResponse> ExecuteNonQuery(
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

            return new DatabaseNonQueryResponse
            {
                Success = errorMessage?.Equals(DefaultConstants.SuccessfulMessage, StringComparison.Ordinal) == true,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
            };
        }
        catch (Exception ex)
        {
            return DatabaseNonQueryResponse.Failure(ex.Message);
        }
    }
    #endregion
}
