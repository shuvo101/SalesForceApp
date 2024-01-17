using System.Data;

using MySqlConnector;

using Oracle.ManagedDataAccess.Client;

namespace Lib.DBAccess.Helpers;

internal class CommonHelper
{
    internal static void SetupOracleCommandWithParameters(OracleCommand command, string procedureName, IEnumerable<OracleParameter> parameters, OracleTransaction? transaction = null)
    {
        command.CommandText = procedureName;
        command.BindByName = true;
        command.CommandType = CommandType.StoredProcedure;
        command.Transaction = transaction;

        foreach (var parameter in parameters)
        {
            parameter.Value ??= DBNull.Value;

            command.Parameters.Add(parameter);
        }
    }

    internal static void SetupMySqlCommandWithParameters(MySqlCommand command, string procedureName, IEnumerable<MySqlParameter> parameters, MySqlTransaction? transaction = null)
    {
        command.CommandText = procedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Transaction = transaction;

        foreach (var parameter in parameters)
        {
            parameter.Value ??= DBNull.Value;

            command.Parameters.Add(parameter);
        }
    }
}
