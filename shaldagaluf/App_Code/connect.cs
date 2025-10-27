using System;
using System.Web;
  
/// <summary>
/// מחלקת חיבור למסד נתונים
/// </summary>
public class Connect
{
    private const string calnder = "calnder.db1.accdb";

    public static string GetConnectionString()
    {
        string location = HttpContext.Current.Server.MapPath("~/App_Data/" + calnder);
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + location;
        return connectionString;
    }
}
