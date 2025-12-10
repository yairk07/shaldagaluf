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
    Category TEXT(50),
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

                if (userId.HasValue)
                {
                    string sql1 = @"
SELECT 
    SC.Id AS CalendarId,
    SC.Name AS CalendarName,
    SC.Description,
    SC.CreatedBy,
    U.username AS CreatorName,
    SC.CreatedDate
FROM SharedCalendars SC
INNER JOIN Users U ON CLng(SC.CreatedBy) = CLng(U.id)
WHERE SC.CreatedBy = ?";

                    OleDbCommand cmd1 = new OleDbCommand(sql1, con);
                    cmd1.Parameters.AddWithValue("?", userId.Value);
                    OleDbDataAdapter da1 = new OleDbDataAdapter(cmd1);
                    da1.Fill(dt);

                    dt.Columns.Add("IsMember", typeof(int));
                    dt.Columns.Add("IsAdmin", typeof(int));

                    foreach (DataRow row in dt.Rows)
                    {
                        row["IsAdmin"] = 1;
                        row["IsMember"] = 0;
                    }

                    string sql2 = @"
SELECT DISTINCT
    SC.Id AS CalendarId,
    SC.Name AS CalendarName,
    SC.Description,
    SC.CreatedBy,
    U.username AS CreatorName,
    SC.CreatedDate
FROM (SharedCalendars SC
INNER JOIN SharedCalendarMembers SCM ON SC.Id = SCM.CalendarId)
INNER JOIN Users U ON CLng(SC.CreatedBy) = CLng(U.id)
WHERE CLng(SCM.UserId) = ? AND CLng(SC.CreatedBy) <> ?";

                    OleDbCommand cmd2 = new OleDbCommand(sql2, con);
                    cmd2.Parameters.AddWithValue("?", userId.Value);
                    cmd2.Parameters.AddWithValue("?", userId.Value);
                    OleDbDataAdapter da2 = new OleDbDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);

                    dt2.Columns.Add("IsMember", typeof(int));
                    dt2.Columns.Add("IsAdmin", typeof(int));

                    foreach (DataRow row in dt2.Rows)
                    {
                        row["IsAdmin"] = 0;
                        row["IsMember"] = 1;
                    }

                    foreach (DataRow row in dt2.Rows)
                    {
                        dt.ImportRow(row);
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "CreatedDate DESC";
                    dt = dv.ToTable();
                }
                else
                {
                    string sql = @"
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
LEFT JOIN Users U ON CLng(SC.CreatedBy) = CLng(U.id)
ORDER BY SC.CreatedDate DESC";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in GetAllSharedCalendars: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
        }
        
        System.Diagnostics.Debug.WriteLine($"GetAllSharedCalendars: Returning {dt.Rows.Count} calendars");
        if (dt.Rows.Count > 0)
        {
            System.Diagnostics.Debug.WriteLine("GetAllSharedCalendars: First calendar:");
            DataRow firstRow = dt.Rows[0];
            foreach (DataColumn col in dt.Columns)
            {
                System.Diagnostics.Debug.WriteLine($"  {col.ColumnName}: {firstRow[col.ColumnName]}");
            }
        }

        return dt;
    }

    public int CreateSharedCalendar(string name, string description, int createdBy)
    {
        string conStr = Connect.GetConnectionString();
        int calendarId = 0;

        try
        {
            System.Diagnostics.Debug.WriteLine($"CreateSharedCalendar: Creating calendar - Name: '{name}', Description: '{description}', CreatedBy: {createdBy}");
            
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                EnsureTablesExist();

                string sql = "INSERT INTO SharedCalendars (Name, Description, CreatedBy, CreatedDate) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", name ?? "");
                cmd.Parameters.AddWithValue("?", description ?? "");
                OleDbParameter createdByParam = new OleDbParameter("?", OleDbType.BigInt);
                createdByParam.Value = (long)createdBy;
                cmd.Parameters.Add(createdByParam);
                OleDbParameter dateParam = new OleDbParameter("?", OleDbType.Date);
                dateParam.Value = DateTime.Now;
                cmd.Parameters.Add(dateParam);

                cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("CreateSharedCalendar: Calendar inserted successfully");

                cmd.CommandText = "SELECT @@IDENTITY";
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    calendarId = Convert.ToInt32(result);
                    System.Diagnostics.Debug.WriteLine($"CreateSharedCalendar: Calendar ID: {calendarId}");

                    sql = "INSERT INTO SharedCalendarMembers (CalendarId, UserId, Role, JoinedDate) VALUES (?, ?, ?, ?)";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    OleDbParameter calendarIdParam = new OleDbParameter("?", OleDbType.BigInt);
                    calendarIdParam.Value = (long)calendarId;
                    cmd.Parameters.Add(calendarIdParam);
                    OleDbParameter userIdParam = new OleDbParameter("?", OleDbType.BigInt);
                    userIdParam.Value = (long)createdBy;
                    cmd.Parameters.Add(userIdParam);
                    cmd.Parameters.AddWithValue("?", "admin");
                    OleDbParameter joinedDateParam = new OleDbParameter("?", OleDbType.Date);
                    joinedDateParam.Value = DateTime.Now;
                    cmd.Parameters.Add(joinedDateParam);
                    cmd.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine("CreateSharedCalendar: Member added successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("CreateSharedCalendar: Warning - @@IDENTITY returned null");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in CreateSharedCalendar: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
            throw;
        }

        System.Diagnostics.Debug.WriteLine($"CreateSharedCalendar: Returning calendarId: {calendarId}");
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
LEFT JOIN Users U ON CLng(JR.UserId) = CLng(U.id)
WHERE JR.CalendarId = ? AND JR.Status = 'pending'";

                if (adminUserId.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM SharedCalendars SC WHERE SC.Id = ? AND CLng(SC.CreatedBy) = ?)";
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
    SCE.Category,
    SCE.CreatedBy,
    U.username AS CreatedByName,
    SCE.CreatedDate
FROM SharedCalendarEvents SCE
LEFT JOIN Users U ON CLng(SCE.CreatedBy) = CLng(U.id)
WHERE SCE.CalendarId = ?";

                if (userId.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM SharedCalendarMembers SCM WHERE SCM.CalendarId = ? AND CLng(SCM.UserId) = ?)";
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

    public void AddSharedCalendarEvent(int calendarId, string title, DateTime date, string time, string notes, string category, int createdBy)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "INSERT INTO SharedCalendarEvents (CalendarId, Title, [Date], [Time], Notes, Category, CreatedBy, CreatedDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                OleDbParameter calendarIdParam = new OleDbParameter("?", OleDbType.BigInt);
                calendarIdParam.Value = (long)calendarId;
                cmd.Parameters.Add(calendarIdParam);
                cmd.Parameters.AddWithValue("?", title ?? "");
                OleDbParameter dateParam = new OleDbParameter("?", OleDbType.Date);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);
                cmd.Parameters.AddWithValue("?", time ?? "");
                cmd.Parameters.AddWithValue("?", notes ?? "");
                cmd.Parameters.AddWithValue("?", category ?? "אחר");
                OleDbParameter createdByParam = new OleDbParameter("?", OleDbType.BigInt);
                createdByParam.Value = (long)createdBy;
                cmd.Parameters.Add(createdByParam);
                OleDbParameter createdDateParam = new OleDbParameter("?", OleDbType.Date);
                createdDateParam.Value = DateTime.Now;
                cmd.Parameters.Add(createdDateParam);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in AddSharedCalendarEvent: " + ex.Message);
            throw;
        }
    }

    public void UpdateSharedCalendarEvent(int eventId, string title, DateTime date, string time, string notes, string category)
    {
        string conStr = Connect.GetConnectionString();

        try
        {
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();

                string sql = "UPDATE SharedCalendarEvents SET Title = ?, [Date] = ?, [Time] = ?, Notes = ?, Category = ? WHERE Id = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", title ?? "");
                OleDbParameter dateParam = new OleDbParameter("?", OleDbType.Date);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);
                cmd.Parameters.AddWithValue("?", time ?? "");
                cmd.Parameters.AddWithValue("?", notes ?? "");
                cmd.Parameters.AddWithValue("?", category ?? "אחר");
                OleDbParameter eventIdParam = new OleDbParameter("?", OleDbType.BigInt);
                eventIdParam.Value = (long)eventId;
                cmd.Parameters.Add(eventIdParam);

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

                string sql = "SELECT COUNT(*) FROM SharedCalendars WHERE Id = ? AND CLng(CreatedBy) = ?";
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

                string sql = "SELECT COUNT(*) FROM SharedCalendarMembers WHERE CalendarId = ? AND CLng(UserId) = ?";
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
LEFT JOIN Users U ON CLng(SC.CreatedBy) = CLng(U.id)
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