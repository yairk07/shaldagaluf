using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;

public class UsersService
{
    static OleDbConnection myConnection;

    public UsersService()
    {
        string connectionString = Connect.GetConnectionString();
        myConnection = new OleDbConnection(connectionString);
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
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
                "VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            OleDbCommand cmd = new OleDbCommand(sSql, myConnection);

            string hashedPassword = HashPassword(password);

            cmd.Parameters.AddWithValue("?", userName);
            cmd.Parameters.AddWithValue("?", firstName);
            cmd.Parameters.AddWithValue("?", lastName);
            cmd.Parameters.AddWithValue("?", email);
            cmd.Parameters.AddWithValue("?", hashedPassword);
            cmd.Parameters.AddWithValue("?", gender);
            cmd.Parameters.AddWithValue("?", yearOfBirth);
            cmd.Parameters.AddWithValue("?", userId);
            cmd.Parameters.AddWithValue("?", phonenum);
            cmd.Parameters.AddWithValue("?", city);

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

            string hashedPassword = HashPassword(password);
            string sql = "SELECT * FROM Users WHERE userName=? AND [password]=?";
            OleDbCommand cmd = new OleDbCommand(sql, myConnection);

            cmd.Parameters.AddWithValue("?", userName);
            cmd.Parameters.AddWithValue("?", hashedPassword);

            var adp = new OleDbDataAdapter(cmd);
            adp.Fill(ds, "Users");
        }
        finally
        {
            myConnection.Close();
        }

        return ds.Tables[0].Rows.Count > 0;
    }

    public DataRow GetUserByEmail(string email)
    {
        DataSet ds = new DataSet();

        try
        {
            myConnection.Open();

            string sql = "SELECT * FROM Users WHERE email=?";
            OleDbCommand cmd = new OleDbCommand(sql, myConnection);

            cmd.Parameters.AddWithValue("?", email);

            var adp = new OleDbDataAdapter(cmd);
            adp.Fill(ds, "Users");
        }
        finally
        {
            myConnection.Close();
        }

        if (ds.Tables[0].Rows.Count > 0)
            return ds.Tables[0].Rows[0];
        return null;
    }

    public void UpdatePassword(int userId, string newPassword)
    {
        try
        {
            myConnection.Open();

            string hashedPassword = HashPassword(newPassword);
            string sql = "UPDATE Users SET [password]=? WHERE id=?";
            OleDbCommand cmd = new OleDbCommand(sql, myConnection);

            cmd.Parameters.AddWithValue("?", hashedPassword);
            cmd.Parameters.AddWithValue("?", userId);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            myConnection.Close();
        }
    }
}
