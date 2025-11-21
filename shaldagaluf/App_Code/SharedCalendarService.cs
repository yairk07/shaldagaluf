using System;
using System.Data;
using System.Data.OleDb;

public class SharedCalendarService
{
    public SharedCalendarService()
    {
        EnsureTablesExist();
    }

    private void EnsureTablesExist()
    {
        string conStr = Connect.GetConnectionString();
        using (OleDbConnection con = new OleDbConnection(conStr))
        {
            con.Open();

            try
            {
                OleDbCommand testCmd = new OleDbCommand("SELECT TOP 1 * FROM SharedCalendars", con);
                testCmd.ExecuteScalar();
            }
            catch
            {
                CreateTables(con);
            }
        }
    }

    private void CreateTables(OleDbConnection con)
    {
        try
        {
            string createSharedCalendars = @"
CREATE TABLE SharedCalendars (
    Id AUTOINCREMENT PRIMARY KEY,
    Name TEXT(255),
    Description MEMO,
    CreatedBy LONG,
    CreatedDate DATETIME
)";

            string createSharedCalendarMembers = @"
CREATE TABLE SharedCalendarMembers (
    Id AUTOINCREMENT PRIMARY KEY,
    CalendarId LONG,
    UserId LONG,
    Role TEXT(50),
    JoinedDate DATETIME
)";

            string createJoinRequests = @"
CREATE TABLE JoinRequests (
    Id AUTOINCREMENT PRIMARY KEY,
    CalendarId LONG,
    UserId LONG,
    Status TEXT(50),
    RequestDate DATETIME,
    Message MEMO
)";

            string createSharedCalendarEvents = @"
CREATE TABLE SharedCalendarEvents (
    Id AUTOINCREMENT PRIMARY KEY,
    CalendarId LONG,
    Title TEXT(255),
    [Date] DATETIME,
    [Time] TEXT(50),
    Notes MEMO,
    CreatedBy LONG,
    CreatedDate DATETIME
)";

            OleDbCommand cmd = new OleDbCommand(createSharedCalendars, con);
            cmd.ExecuteNonQuery();

            cmd.CommandText = createSharedCalendarMembers;
            cmd.ExecuteNonQuery();

            cmd.CommandText = createJoinRequests;
            cmd.ExecuteNonQuery();

            cmd.CommandText = createSharedCalendarEvents;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating tables: " + ex.Message);
        }
    }
    public DataTable GetAllSharedCalendars(int? userId = null)
    {
        string conStr = Connect.GetConnectionString();
        DataTable dt = new DataTable();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "";
                OleDbCommand cmd = null;

                if (userId.HasValue)
                {
                    sql = @"
SELECT 
    SC.Id AS CalendarId,
    SC.Name AS CalendarName,
    SC.Description,
    SC.CreatedBy,
    U.username AS CreatorName,
    SC.CreatedDate,
    IIf(SCM.UserId = ?, 1, 0) AS IsMember,
    IIf(SC.CreatedBy = ?, 1, 0) AS IsAdmin
FROM SharedCalendars SC
LEFT JOIN Users U ON SC.CreatedBy = U.id
LEFT JOIN SharedCalendarMembers SCM ON SC.Id = SCM.CalendarId AND SCM.UserId = ?
WHERE SC.CreatedBy = ? OR SCM.UserId = ?
ORDER BY SC.CreatedDate DESC";

                    cmd = new OleDbCommand(sql, con);
                    cmd.Parameters.AddWithValue("?", userId.Value);
                    cmd.Parameters.AddWithValue("?", userId.Value);
                    cmd.Parameters.AddWithValue("?", userId.Value);
                    cmd.Parameters.AddWithValue("?", userId.Value);
                    cmd.Parameters.AddWithValue("?", userId.Value);
                }
                else
                {
                    sql = @"
SELECT 
    SC.Id AS CalendarId,
    SC.Name AS CalendarName,
    SC.Description,
    SC.CreatedBy,
    U.username AS CreatorName,
    SC.CreatedDate,
    0 AS IsMember,
    0 AS IsAdmin
FROM SharedCalendars SC
LEFT JOIN Users U ON SC.CreatedBy = U.id
ORDER BY SC.CreatedDate DESC";

                    cmd = new OleDbCommand(sql, con);
                }

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in GetAllSharedCalendars: " + ex.Message);
        }

        return dt;
    }

    public int CreateSharedCalendar(string name, string description, int createdBy)
    {
        string conStr = Connect.GetConnectionString();
        int calendarId = 0;

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "INSERT INTO SharedCalendars (Name, Description, CreatedBy, CreatedDate) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", name);
                cmd.Parameters.AddWithValue("?", description ?? "");
                cmd.Parameters.AddWithValue("?", createdBy);
                cmd.Parameters.AddWithValue("?", DateTime.Now);

                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT @@IDENTITY";
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    calendarId = Convert.ToInt32(result);

                    sql = "INSERT INTO SharedCalendarMembers (CalendarId, UserId, Role, JoinedDate) VALUES (?, ?, ?, ?)";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("?", calendarId);
                    cmd.Parameters.AddWithValue("?", createdBy);
                    cmd.Parameters.AddWithValue("?", "admin");
                    cmd.Parameters.AddWithValue("?", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in CreateSharedCalendar: " + ex.Message);
            throw;
        }

        return calendarId;
    }

    public void AddMemberToCalendar(int calendarId, int userId, string role = "member")
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "INSERT INTO SharedCalendarMembers (CalendarId, UserId, Role, JoinedDate) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                cmd.Parameters.AddWithValue("?", userId);
                cmd.Parameters.AddWithValue("?", role);
                cmd.Parameters.AddWithValue("?", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in AddMemberToCalendar: " + ex.Message);
            throw;
        }
    }

    public void CreateJoinRequest(int calendarId, int userId, string message = "")
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "INSERT INTO JoinRequests (CalendarId, UserId, Status, RequestDate, Message) VALUES (?, ?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                cmd.Parameters.AddWithValue("?", userId);
                cmd.Parameters.AddWithValue("?", "pending");
                cmd.Parameters.AddWithValue("?", DateTime.Now);
                cmd.Parameters.AddWithValue("?", message ?? "");

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in CreateJoinRequest: " + ex.Message);
            throw;
        }
    }

    public DataTable GetJoinRequests(int calendarId, int? adminUserId = null)
    {
        string conStr = Connect.GetConnectionString();
        DataTable dt = new DataTable();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = @"
SELECT 
    JR.Id AS RequestId,
    JR.CalendarId,
    JR.UserId,
    U.username AS UserName,
    U.firstName,
    U.lastName,
    JR.Status,
    JR.RequestDate,
    JR.Message
FROM JoinRequests JR
LEFT JOIN Users U ON JR.UserId = U.id
WHERE JR.CalendarId = ? AND JR.Status = 'pending'";

                if (adminUserId.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM SharedCalendars SC WHERE SC.Id = ? AND SC.CreatedBy = ?)";
                }

                sql += " ORDER BY JR.RequestDate DESC";

                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                if (adminUserId.HasValue)
                {
                    cmd.Parameters.AddWithValue("?", calendarId);
                    cmd.Parameters.AddWithValue("?", adminUserId.Value);
                }

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in GetJoinRequests: " + ex.Message);
        }

        return dt;
    }

    public void ApproveJoinRequest(int requestId, int calendarId, int userId)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "UPDATE JoinRequests SET Status = 'approved' WHERE Id = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", requestId);
                cmd.ExecuteNonQuery();

                AddMemberToCalendar(calendarId, userId, "member");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in ApproveJoinRequest: " + ex.Message);
            throw;
        }
    }

    public void RejectJoinRequest(int requestId)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "UPDATE JoinRequests SET Status = 'rejected' WHERE Id = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", requestId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in RejectJoinRequest: " + ex.Message);
            throw;
        }
    }

    public DataTable GetSharedCalendarEvents(int calendarId, int? userId = null)
    {
        string conStr = Connect.GetConnectionString();
        DataTable dt = new DataTable();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = @"
SELECT 
    SCE.Id AS Id,
    SCE.CalendarId,
    SCE.Title,
    SCE.[Date] AS EventDate,
    SCE.[Time] AS EventTime,
    SCE.Notes,
    SCE.CreatedBy,
    U.username AS CreatedByName,
    SCE.CreatedDate
FROM SharedCalendarEvents SCE
LEFT JOIN Users U ON SCE.CreatedBy = U.id
WHERE SCE.CalendarId = ?";

                if (userId.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM SharedCalendarMembers SCM WHERE SCM.CalendarId = ? AND SCM.UserId = ?)";
                }

                sql += " ORDER BY SCE.[Date] DESC, SCE.[Time] DESC";

                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                if (userId.HasValue)
                {
                    cmd.Parameters.AddWithValue("?", calendarId);
                    cmd.Parameters.AddWithValue("?", userId.Value);
                }

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in GetSharedCalendarEvents: " + ex.Message);
        }

        return dt;
    }

    public void AddSharedCalendarEvent(int calendarId, string title, DateTime date, string time, string notes, int createdBy)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "INSERT INTO SharedCalendarEvents (CalendarId, Title, [Date], [Time], Notes, CreatedBy, CreatedDate) VALUES (?, ?, ?, ?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                cmd.Parameters.AddWithValue("?", title);
                cmd.Parameters.AddWithValue("?", date);
                cmd.Parameters.AddWithValue("?", time ?? "");
                cmd.Parameters.AddWithValue("?", notes ?? "");
                cmd.Parameters.AddWithValue("?", createdBy);
                cmd.Parameters.AddWithValue("?", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in AddSharedCalendarEvent: " + ex.Message);
            throw;
        }
    }

    public void UpdateSharedCalendarEvent(int eventId, string title, DateTime date, string time, string notes)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "UPDATE SharedCalendarEvents SET Title = ?, [Date] = ?, [Time] = ?, Notes = ? WHERE Id = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", title);
                cmd.Parameters.AddWithValue("?", date);
                cmd.Parameters.AddWithValue("?", time ?? "");
                cmd.Parameters.AddWithValue("?", notes ?? "");
                cmd.Parameters.AddWithValue("?", eventId);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in UpdateSharedCalendarEvent: " + ex.Message);
            throw;
        }
    }

    public void DeleteSharedCalendarEvent(int eventId)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "DELETE FROM SharedCalendarEvents WHERE Id = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", eventId);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in DeleteSharedCalendarEvent: " + ex.Message);
            throw;
        }
    }

    public bool IsCalendarAdmin(int calendarId, int userId)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "SELECT COUNT(*) FROM SharedCalendars WHERE Id = ? AND CreatedBy = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                cmd.Parameters.AddWithValue("?", userId);

                object result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) > 0;
            }
        }
        catch
        {
            return false;
        }
    }

    public bool IsCalendarMember(int calendarId, int userId)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "SELECT COUNT(*) FROM SharedCalendarMembers WHERE CalendarId = ? AND UserId = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);
                cmd.Parameters.AddWithValue("?", userId);

                object result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) > 0;
            }
        }
        catch
        {
            return false;
        }
    }

    public DataRow GetSharedCalendar(int calendarId)
    {
        string conStr = Connect.GetConnectionString();
        DataTable dt = new DataTable();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = @"
SELECT 
    SC.Id AS CalendarId,
    SC.Name AS CalendarName,
    SC.Description,
    SC.CreatedBy,
    U.username AS CreatorName,
    SC.CreatedDate
FROM SharedCalendars SC
LEFT JOIN Users U ON SC.CreatedBy = U.id
WHERE SC.Id = ?";

                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", calendarId);

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in GetSharedCalendar: " + ex.Message);
        }

        if (dt.Rows.Count > 0)
            return dt.Rows[0];

        return null;
    }
}