using System.Data;
using System.Data.Common;

namespace SalesForceApp.Core.Tests.Entity;

public class Product
{
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;

    public static void MapFromDbWithReader(DbDataReader reader, Product product)
    {
        product.ProductCode = reader.GetString(reader.GetOrdinal("PRODUCTCODE"));
        product.ProductName = reader.GetString(reader.GetOrdinal("PRODUCTNAME"));
    }

    public static void MapFromDbWithDataRow(DataRow row, Product product)
    {
        product.ProductCode = row.Field<string>("PRODUCTCODE") ?? string.Empty;
        product.ProductName = row.Field<string>("PRODUCTNAME") ?? string.Empty;
    }
}
