using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

public class UsersService
{
    static OleDbConnection myConnection;

    public UsersService()
    {
        string connectionString = Connect.GetConnectionString();
        myConnection = new OleDbConnection(connectionString);
    }

    // ------------------------------
    // INSERT USER
    // ------------------------------
    public void insertIntoDB(string userName, string firstName, string lastName, string email, string password,
                int gender, int yearOfBirth, string userId, string phonenum, int city)
    {
        try
        {
            myConnection.Open();

            string sSql =
                "INSERT INTO Users (userName, firstName, lastName, email, [password], gender, yearOfBirth, userId, phonenum, city) " +
                "VALUES(@userName, @firstName, @lastName, @Email, @Password, @Gender, @YearOfBirth, @UserId, @Phone, @City)";

            OleDbCommand cmd = new OleDbCommand(sSql, myConnection);

            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@YearOfBirth", yearOfBirth);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Phone", phonenum);
            cmd.Parameters.AddWithValue("@City", city);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            myConnection.Close();
        }
    }

    // ------------------------------
    // GET ALL USERS
    // ------------------------------
    public DataSet getallusers()
    {
        var data = new DataSet();

        try
        {
            myConnection.Open();

            // טוען את כל המשתמשים
            var usersCmd = new OleDbCommand("SELECT * FROM [Users]", myConnection);
            var usersAdp = new OleDbDataAdapter(usersCmd);
            var usersTable = new DataTable("Users");
            usersAdp.Fill(usersTable);

            // --- תיקון עמודת ROLE ---
            string roleCol = usersTable.Columns
                .Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .FirstOrDefault(name => name.Trim().ToLower() == "role");

            if (roleCol == null)
                usersTable.Columns.Add("Role", typeof(string));
            else if (roleCol != "Role")
                usersTable.Columns[roleCol].ColumnName = "Role";

            // הדפסת שמות עמודות לדיאגנוסטיקה
            foreach (DataColumn col in usersTable.Columns)
                System.Diagnostics.Debug.WriteLine("COL: [" + col.ColumnName + "]");

            // ------------------------------
            // CITY TABLE
            // ------------------------------
            var citiesCmd = new OleDbCommand("SELECT id, cityname FROM [Citys]", myConnection);
            var citiesAdp = new OleDbDataAdapter(citiesCmd);
            var citiesTable = new DataTable("Citys");
            citiesAdp.Fill(citiesTable);

            var dict = new Dictionary<int, string>();
            foreach (DataRow r in citiesTable.Rows)
            {
                if (int.TryParse(Convert.ToString(r["id"]).Trim(), out int id))
                    dict[id] = Convert.ToString(r["cityname"]);
            }

            // הוספת CityName
            if (!usersTable.Columns.Contains("CityName"))
                usersTable.Columns.Add("CityName", typeof(string));

            foreach (DataRow u in usersTable.Rows)
            {
                string raw = Convert.ToString(u["city"]).Trim();
                if (int.TryParse(raw, out int code) && dict.ContainsKey(code))
                    u["CityName"] = dict[code];
                else
                    u["CityName"] = "";
            }

            data.Tables.Add(usersTable);
            return data;
        }
        finally
        {
            myConnection.Close();
        }
    }

    // ------------------------------
    // LOGIN CHECK
    // ------------------------------
    public bool IsExist(string userName, string password)
    {
        DataSet ds = new DataSet();

        try
        {
            myConnection.Open();

            string sql = "SELECT * FROM Users WHERE userName=@user AND [password]=@pass";
            OleDbCommand cmd = new OleDbCommand(sql, myConnection);

            cmd.Parameters.AddWithValue("@user", userName);
            cmd.Parameters.AddWithValue("@pass", password);

            var adp = new OleDbDataAdapter(cmd);
            adp.Fill(ds, "Users");
        }
        finally
        {
            myConnection.Close();
        }

        return ds.Tables[0].Rows.Count > 0;
    }
}
