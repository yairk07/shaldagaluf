using System;
using System.Data;
using System.Data.OleDb;

public class EventService
{
    private bool TableExists(string tableName, OleDbConnection con)
    {
        try
        {
            OleDbCommand cmd = new OleDbCommand($"SELECT TOP 1 * FROM [{tableName}]", con);
            cmd.ExecuteScalar();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public DataTable GetAllEvents(int? userId = null)
    {
        string conStr = Connect.GetConnectionString();
        DataTable dt = new DataTable();

        using (OleDbConnection con = new OleDbConnection(conStr))
        {
            con.Open();

            string sql = @"
SELECT
    C.Id            AS Id,
    C.Userid        AS UserId,
    U.username      AS UserName,
    C.title         AS Title,
    C.[date]        AS EventDate,
    C.[time]        AS EventTime,
    C.notes         AS Notes,
    'personal'      AS EventType
FROM calnder AS C
LEFT JOIN users AS U
    ON C.Userid = U.id";

            if (userId.HasValue)
            {
                sql += " WHERE C.Userid = ?";
            }

            bool hasSharedTables = TableExists("SharedCalendarEvents", con) && 
                                   TableExists("SharedCalendarMembers", con);

            if (hasSharedTables && userId.HasValue)
            {
                string sharedSql = @"
SELECT
    SCE.Id          AS Id,
    SCE.CreatedBy   AS UserId,
    U2.username     AS UserName,
    SCE.Title       AS Title,
    SCE.[Date]      AS EventDate,
    SCE.[Time]      AS EventTime,
    SCE.Notes       AS Notes,
    'shared'        AS EventType
FROM SharedCalendarEvents SCE
LEFT JOIN Users U2 ON SCE.CreatedBy = U2.id
INNER JOIN SharedCalendarMembers SCM ON SCE.CalendarId = SCM.CalendarId
WHERE SCM.UserId = ?";

                sql += " UNION ALL " + sharedSql;
            }

            sql += " ORDER BY EventDate DESC, EventTime DESC";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            if (userId.HasValue)
            {
                cmd.Parameters.AddWithValue("?", userId.Value);
                if (hasSharedTables)
                {
                    cmd.Parameters.AddWithValue("?", userId.Value);
                }
            }

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
        }

        return dt;
    }
}
