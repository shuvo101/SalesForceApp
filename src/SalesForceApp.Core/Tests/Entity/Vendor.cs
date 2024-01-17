using System.Data;
using System.Data.Common;

namespace SalesForceApp.Core.Tests.Entity;

public class Vendor
{
    public string RsoCode { get; set; } = string.Empty;
    public string CommissionItemName { get; set; } = string.Empty;

    public static void MapFromDbWithReader(DbDataReader reader, Vendor vendor)
    {
        vendor.RsoCode = reader.GetString(reader.GetOrdinal("RSO_CODE"));
        vendor.CommissionItemName = reader.GetString(reader.GetOrdinal("COMMISSION_ITEM_NAME"));
    }

    public static void MapFromDbWithDataRow(DataRow row, Vendor vendor)
    {
        vendor.RsoCode = row.Field<string>("RSO_CODE") ?? string.Empty;
        vendor.CommissionItemName = row.Field<string>("COMMISSION_ITEM_NAME") ?? string.Empty;
    }
}
