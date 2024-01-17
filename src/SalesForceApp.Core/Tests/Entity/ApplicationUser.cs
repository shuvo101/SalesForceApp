using System.Data.Common;

namespace SalesForceApp.Core.Tests.Entity;

public class ApplicationUser
{
    private string _isActive = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public bool IsActive { get => _isActive.Equals("Y", StringComparison.Ordinal); }

    public static void MapFromDbWithReader(DbDataReader reader, ApplicationUser user)
    {
        user.Name = reader.GetString(reader.GetOrdinal("NAME"));
        user.Key = reader.GetString(reader.GetOrdinal("Key"));
        user._isActive = reader.GetString(reader.GetOrdinal("ISACTIVE"));
    }
}
