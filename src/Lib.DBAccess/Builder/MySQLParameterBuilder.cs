using System.Data;

using Lib.DBAccess.Helpers;

using MySqlConnector;

namespace Lib.DBAccess.Builder;

public class MySQLParameterBuilder
{
    private readonly List<MySqlParameter> _mysqlParameters = [];

    private MySQLParameterBuilder()
    {
        _mysqlParameters.Add(new MySqlParameter(DefaultConstants.ErrorCodeParameter, MySqlDbType.Int32)
        {
            Direction = ParameterDirection.Output,
        });
        _mysqlParameters.Add(new MySqlParameter(DefaultConstants.ErrorMessageParameter, MySqlDbType.VarChar)
        {
            Direction = ParameterDirection.Output,
        });
    }

    public static MySQLParameterBuilder Create()
    {
        return new MySQLParameterBuilder();
    }

    public MySQLParameterBuilder AddOutputParameter()
    {
        _mysqlParameters.Add(new MySqlParameter(DefaultConstants.OutputParameter, MySqlDbType.Int32)
        {
            Direction = ParameterDirection.Output,
        });

        return this;
    }

    public MySQLParameterBuilder AddOutParameter(string parameterName, MySqlDbType dbType)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, dbType)
        {
            Direction = ParameterDirection.Output,
        });

        return this;
    }

    public MySQLParameterBuilder AddIntParameter(string parameterName, int value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.Int32)
        {
            Value = value,
        });

        return this;
    }

    public MySQLParameterBuilder AddLongParameter(string parameterName, long value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.Int64)
        {
            Value = value,
        });

        return this;
    }

    public MySQLParameterBuilder AddDoubleParameter(string parameterName, double value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.Double)
        {
            Value = value,
        });

        return this;
    }

    public MySQLParameterBuilder AddDecimalParameter(string parameterName, decimal value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.Decimal)
        {
            Value = value,
        });

        return this;
    }

    public MySQLParameterBuilder AddVarcharParameter(string parameterName, string value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.VarChar)
        {
            Value = value,
        });

        return this;
    }

    public MySQLParameterBuilder AddDateTimeParameter(string parameterName, DateTime? value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.Date)
        {
            Value = value is null || value == DateTime.MinValue || value == DateTime.MaxValue
                ? DBNull.Value
                : value,
        });

        return this;
    }

    public MySQLParameterBuilder AddBlobParameter(string parameterName, byte[] value)
    {
        _mysqlParameters.Add(new MySqlParameter(parameterName, MySqlDbType.Blob)
        {
            Value = value,
        });

        return this;
    }

    public IEnumerable<MySqlParameter> Build()
    {
        return _mysqlParameters;
    }
}
