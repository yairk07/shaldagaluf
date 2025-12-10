using System;
using System.Data;
using System.Data.OleDb;

public class calnderservice
{
    public void InsertEvent(string title, DateTime date, string time, string notes, string category, int? userId = null)
    {
        string sql = "INSERT INTO calnder ([title], [date], [time], [notes], [category], [Userid]) VALUES (?, ?, ?, ?, ?, ?)";

        using (OleDbConnection conn = new OleDbConnection(Connect.GetConnectionString()))
        using (OleDbCommand cmd = new OleDbCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("?", title);
            cmd.Parameters.AddWithValue("?", date);
            cmd.Parameters.AddWithValue("?", time);
            cmd.Parameters.AddWithValue("?", notes);
            cmd.Parameters.AddWithValue("?", category ?? "אחר");
            cmd.Parameters.AddWithValue("?", userId.HasValue ? (object)userId.Value : DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public DataSet GetAllEvents(int? userId = null)
    {
        DataSet data = new DataSet();
        
        using (OleDbConnection conn = new OleDbConnection(Connect.GetConnectionString()))
        {
            conn.Open();

            string sql = "SELECT * FROM calnder";
            if (userId.HasValue)
            {
                sql += " WHERE Userid = ?";
            }

            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                if (userId.HasValue)
                {
                    cmd.Parameters.AddWithValue("?", userId.Value);
                }

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    adapter.Fill(data, "PersonalEvents");
                }
            }

            if (userId.HasValue)
            {
                try
                {
                    OleDbCommand testCmd = new OleDbCommand("SELECT TOP 1 * FROM SharedCalendarEvents", conn);
                    testCmd.ExecuteScalar();

                    string sharedSql = @"
SELECT 
    SCE.Id,
    SCE.CalendarId AS Userid,
    SCE.Title AS title,
    SCE.[Date] AS [date],
    SCE.[Time] AS [time],
    SCE.Notes AS notes,
    SCE.Category AS category
FROM SharedCalendarEvents SCE
INNER JOIN SharedCalendarMembers SCM ON SCE.CalendarId = SCM.CalendarId
WHERE SCM.UserId = ?";

                    using (OleDbCommand sharedCmd = new OleDbCommand(sharedSql, conn))
                    {
                        sharedCmd.Parameters.AddWithValue("?", userId.Value);
                        using (OleDbDataAdapter sharedAdapter = new OleDbDataAdapter(sharedCmd))
                        {
                            sharedAdapter.Fill(data, "SharedEvents");
                        }
                    }
                }
                catch
                {
                }
            }
        }

        return data;
    }
}

