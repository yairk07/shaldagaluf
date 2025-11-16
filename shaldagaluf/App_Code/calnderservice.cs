using System;
using System.Data;
using System.Data.OleDb;

public class calnderservice
{
    public void InsertEvent(string title, DateTime date, string time, string notes)
    {
        string sql = "INSERT INTO calnder ([title], [date], [time], [notes]) VALUES (?, ?, ?, ?)";

        using (OleDbConnection conn = new OleDbConnection(Connect.GetConnectionString()))
        using (OleDbCommand cmd = new OleDbCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("?", title);
            cmd.Parameters.AddWithValue("?", date);
            cmd.Parameters.AddWithValue("?", time);
            cmd.Parameters.AddWithValue("?", notes);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public DataSet GetAllEvents()
    {
        DataSet data = new DataSet();
        string sql = "SELECT * FROM calnder";

        using (OleDbConnection conn = new OleDbConnection(Connect.GetConnectionString()))
        using (OleDbCommand cmd = new OleDbCommand(sql, conn))
        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
        {
            conn.Open();
            adapter.Fill(data);
            

        }

        return data;
    }
}

