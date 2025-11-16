using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
/// <summary>
/// Summary description for UsersService
/// </summary>
public class UsersService
{
    static OleDbConnection myConnection;
    public UsersService()
    {
        string connectionString = Connect.GetConnectionString();
        myConnection = new OleDbConnection(connectionString);
    }

    public void insertIntoDB(string userName, string firstName, string lastName, string email, string password,
                int gender, int yearOfBirth, string userId, string phonenum, int city)
    {
        try
        {
            myConnection.Open();
            string sSql = "INSERT INTO Users (userName, firstName, lastName, email, [password], gender, yearOfBirth, userId, phonenum, city) " +
                          "VALUES('" + userName + "', '" + firstName + "', '" + lastName + "', '" + email + "', '" + password + "', " + gender + ", " + yearOfBirth + ", '" + userId + "', '" + phonenum + "', " + city + ")";
            OleDbCommand myCmd = new OleDbCommand(sSql, myConnection);
            myCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        } 
    }


    public DataSet getallusers()
    {
        var data = new DataSet();

        try
        {
            myConnection.Open();

            // 1) טוען טבלת משתמשים
            var usersCmd = new OleDbCommand(
                "SELECT userName, firstName, lastName, email, phonenum, city FROM [Users]",
                myConnection);
            var usersAdp = new OleDbDataAdapter(usersCmd);
            var usersTable = new DataTable("Users");
            usersAdp.Fill(usersTable);

            // 2) טוען טבלת ערים
            var citiesCmd = new OleDbCommand(
                "SELECT id, cityname FROM [Citys]",
                myConnection);
            var citiesAdp = new OleDbDataAdapter(citiesCmd);
            var citiesTable = new DataTable("Citys");
            citiesAdp.Fill(citiesTable);

            // 3) בונה מילון id -> cityname
            var dict = new Dictionary<int, string>();
            foreach (DataRow r in citiesTable.Rows)
            {
                if (r["id"] != DBNull.Value)
                {
                    int id;
                    // מנסה להמיר גם אם זה טקסט במסד
                    if (int.TryParse(Convert.ToString(r["id"]).Trim(), out id))
                        dict[id] = Convert.ToString(r["cityname"]);
                }
            }

            // 4) מוסיף עמודות "מוכנות להצגה" לטבלת המשתמשים
            if (!usersTable.Columns.Contains("CityId"))
                usersTable.Columns.Add("CityId", typeof(string));
            if (!usersTable.Columns.Contains("CityName"))
                usersTable.Columns.Add("CityName", typeof(string));

            foreach (DataRow u in usersTable.Rows)
            {
                // הערך הגולמי שנשמר בטבלת Users (לעיתים טקסט)
                var raw = (u["city"] == DBNull.Value) ? "" : Convert.ToString(u["city"]).Trim();
                u["CityId"] = raw;

                // מנסה להמיר לקוד עיר
                int code;
                if (int.TryParse(raw, out code) && dict.TryGetValue(code, out var name))
                    u["CityName"] = name;
                else
                    u["CityName"] = ""; // לא נמצאה התאמה – משאיר ריק
            }

            data.Tables.Add(usersTable);
            return data;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            myConnection.Close();
        }
    }

    public bool IsExist(string userName, string password)
    {
        DataSet dataset = new DataSet();
        try
        {
            myConnection.Open();
            string sSql = "SELECT * FROM Users WHERE userName = '" + userName + "' AND [password] = '" + password + "'";
            OleDbCommand myCmd = new OleDbCommand(sSql, myConnection);
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = myCmd;
            adapter.Fill(dataset, "Users");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }

        return dataset.Tables[0].Rows.Count > 0;
    }
}
