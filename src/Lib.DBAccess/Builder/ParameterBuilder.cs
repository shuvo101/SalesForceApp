namespace Lib.DBAccess.Builder;

public class ParameterBuilder
{
    public static OracleParameterBuilder Oracle()
    {
        return OracleParameterBuilder.Create();
    }

    public static MySQLParameterBuilder MySQL()
    {
        return MySQLParameterBuilder.Create();
    }
}
