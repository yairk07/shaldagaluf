using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class sharedCalendarDetails : System.Web.UI.Page
{
    SharedCalendarService service = new SharedCalendarService();
    private int calendarId;
    private int currentUserId;
    private bool isAdmin = false;
    private bool isMember = false;

    public bool IsAdmin { get { return isAdmin; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/html; charset=utf-8";
        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        
        if (Session["username"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!int.TryParse(Request.QueryString["id"], out calendarId))
        {
            ShowNotFound();
            return;
        }

        currentUserId = Convert.ToInt32(Session["userId"]);

        if (!IsPostBack)
        {
            LoadCalendar();
        }
    }

    private string FixEncoding(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        
        try
        {
            byte[] bytes = Encoding.GetEncoding("Windows-1255").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return text;
        }
    }

    private void LoadCalendar()
    {
        DataRow calendar = service.GetSharedCalendar(calendarId);
        if (calendar == null)
        {
            ShowNotFound();
            return;
        }

        string role = Session["Role"]?.ToString() ?? "";
        bool isOwner = string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase);
        
        isAdmin = service.IsCalendarAdmin(calendarId, currentUserId) || isOwner;
        isMember = service.IsCalendarMember(calendarId, currentUserId) || isAdmin;

        System.Diagnostics.Debug.WriteLine($"LoadCalendar: calendarId = {calendarId}, currentUserId = {currentUserId}, role = {role}, isOwner = {isOwner}, isAdmin = {isAdmin}, isMember = {isMember}");

        calendarTitle.Text = FixEncoding(calendar["CalendarName"].ToString());
        calendarDescription.Text = FixEncoding(calendar["Description"]?.ToString() ?? "");

        if (!isMember)
        {
            pnlNotMember.Visible = true;
            pnlMember.Visible = false;
        }
        else
        {
            pnlNotMember.Visible = false;
            pnlMember.Visible = true;
            btnTabRequests.Visible = isAdmin;
            btnAddEvent.Visible = isAdmin;
            System.Diagnostics.Debug.WriteLine($"LoadCalendar: btnAddEvent.Visible = {btnAddEvent.Visible}");
            LoadEvents();
        }
    }

    private void ShowNotFound()
    {
        pnlContent.Visible = false;
        pnlNotFound.Visible = true;
    }

    protected void btnSendJoinRequest_Click(object sender, EventArgs e)
    {
        string message = txtJoinMessage.Text.Trim();
        service.CreateJoinRequest(calendarId, currentUserId, message);
        lblJoinMessage.Text = "בקשתך נשלחה בהצלחה! המנהל יקבל התראה.";
        lblJoinMessage.ForeColor = System.Drawing.Color.Green;
        txtJoinMessage.Text = "";
    }

    protected void btnTabEvents_Click(object sender, EventArgs e)
    {
        pnlEvents.Visible = true;
        pnlRequests.Visible = false;
        btnTabEvents.CssClass = "tab-button active";
        btnTabRequests.CssClass = "tab-button";
        LoadEvents();
    }

    protected void btnTabRequests_Click(object sender, EventArgs e)
    {
        pnlEvents.Visible = false;
        pnlRequests.Visible = true;
        btnTabEvents.CssClass = "tab-button";
        btnTabRequests.CssClass = "tab-button active";
        LoadRequests();
    }

    private void LoadEvents()
    {
        DataTable dt = service.GetSharedCalendarEvents(calendarId, currentUserId);
        
        foreach (DataRow row in dt.Rows)
        {
            if (dt.Columns.Contains("Title") && row["Title"] != DBNull.Value)
                row["Title"] = FixEncoding(row["Title"].ToString());
            if (dt.Columns.Contains("Notes") && row["Notes"] != DBNull.Value)
                row["Notes"] = FixEncoding(row["Notes"].ToString());
            if (dt.Columns.Contains("CreatedByName") && row["CreatedByName"] != DBNull.Value)
                row["CreatedByName"] = FixEncoding(row["CreatedByName"].ToString());
        }
        
        dlEvents.DataSource = dt;
        dlEvents.DataBind();
        
        foreach (DataListItem item in dlEvents.Items)
        {
            LinkButton lnkEdit = item.FindControl("lnkEdit") as LinkButton;
            LinkButton lnkDelete = item.FindControl("lnkDelete") as LinkButton;
            
            if (lnkEdit != null)
                lnkEdit.Visible = isAdmin;
            if (lnkDelete != null)
                lnkDelete.Visible = isAdmin;
        }
    }

    private void LoadRequests()
    {
        DataTable dt = service.GetJoinRequests(calendarId, currentUserId);
        
        foreach (DataRow row in dt.Rows)
        {
            if (dt.Columns.Contains("UserName") && row["UserName"] != DBNull.Value)
                row["UserName"] = FixEncoding(row["UserName"].ToString());
            if (dt.Columns.Contains("Message") && row["Message"] != DBNull.Value)
                row["Message"] = FixEncoding(row["Message"].ToString());
        }
        
        dlRequests.DataSource = dt;
        dlRequests.DataBind();
    }

    protected void btnAddEvent_Click(object sender, EventArgs e)
    {
        pnlAddEvent.Visible = true;
        btnAddEvent.Visible = false;
        ViewState["EditingEventId"] = null;
        ClearEventForm();
    }

    protected void btnCancelEvent_Click(object sender, EventArgs e)
    {
        pnlAddEvent.Visible = false;
        btnAddEvent.Visible = true;
        ClearEventForm();
    }

    protected void btnSaveEvent_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("btnSaveEvent_Click: Called");
        
        string role = Session["Role"]?.ToString() ?? "";
        bool isOwner = string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase);
        bool userIsAdmin = isAdmin || isOwner;
        
        System.Diagnostics.Debug.WriteLine($"btnSaveEvent_Click: isAdmin = {isAdmin}, isOwner = {isOwner}, userIsAdmin = {userIsAdmin}, calendarId = {calendarId}, currentUserId = {currentUserId}");

        if (!userIsAdmin)
        {
            System.Diagnostics.Debug.WriteLine("btnSaveEvent_Click: User is not admin or owner, returning");
            return;
        }

        try
        {
            string title = txtEventTitle.Text.Trim();
            string dateStr = txtEventDate.Text;
            string time = txtEventTime.Text;
            string notes = txtEventNotes.Text.Trim();

            System.Diagnostics.Debug.WriteLine($"btnSaveEvent_Click: title = '{title}', dateStr = '{dateStr}', time = '{time}', notes = '{notes}'");

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(dateStr))
            {
                System.Diagnostics.Debug.WriteLine("btnSaveEvent_Click: Title or date is empty, returning");
                return;
            }

            DateTime eventDate = DateTime.Parse(dateStr);
            System.Diagnostics.Debug.WriteLine($"btnSaveEvent_Click: eventDate = {eventDate}");

            int? editingId = ViewState["EditingEventId"] as int?;
            System.Diagnostics.Debug.WriteLine($"btnSaveEvent_Click: editingId = {editingId}");
            
            if (editingId.HasValue)
            {
                System.Diagnostics.Debug.WriteLine($"btnSaveEvent_Click: Updating event {editingId.Value}");
                service.UpdateSharedCalendarEvent(editingId.Value, title, eventDate, time, notes);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"btnSaveEvent_Click: Adding new event to calendar {calendarId}");
                service.AddSharedCalendarEvent(calendarId, title, eventDate, time, notes, currentUserId);
            }

            System.Diagnostics.Debug.WriteLine("btnSaveEvent_Click: Event saved successfully");
            pnlAddEvent.Visible = false;
            btnAddEvent.Visible = true;
            ClearEventForm();
            LoadEvents();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in btnSaveEvent_Click: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
        }
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        string role = Session["Role"]?.ToString() ?? "";
        bool isOwner = string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner)
        {
            return;
        }

        LinkButton btn = sender as LinkButton;
        int eventId = Convert.ToInt32(btn.CommandArgument);

        DataTable dt = service.GetSharedCalendarEvents(calendarId, currentUserId);
        DataRow[] rows = dt.Select($"Id = {eventId}");
        if (rows.Length > 0)
        {
            DataRow row = rows[0];
            txtEventTitle.Text = row["Title"].ToString();
            txtEventDate.Text = Convert.ToDateTime(row["EventDate"]).ToString("yyyy-MM-dd");
            txtEventTime.Text = row["EventTime"]?.ToString() ?? "";
            txtEventNotes.Text = row["Notes"]?.ToString() ?? "";
            ViewState["EditingEventId"] = eventId;
            pnlAddEvent.Visible = true;
            btnAddEvent.Visible = false;
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string role = Session["Role"]?.ToString() ?? "";
        bool isOwner = string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner)
        {
            return;
        }

        LinkButton btn = sender as LinkButton;
        int eventId = Convert.ToInt32(btn.CommandArgument);
        service.DeleteSharedCalendarEvent(eventId);
        LoadEvents();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        string role = Session["Role"]?.ToString() ?? "";
        bool isOwner = string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner)
        {
            return;
        }

        Button btn = sender as Button;
        int requestId = Convert.ToInt32(btn.CommandArgument);

        DataTable dt = service.GetJoinRequests(calendarId, currentUserId);
        DataRow[] rows = dt.Select($"RequestId = {requestId}");
        if (rows.Length > 0)
        {
            int userId = Convert.ToInt32(rows[0]["UserId"]);
            service.ApproveJoinRequest(requestId, calendarId, userId);
            LoadRequests();
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        string role = Session["Role"]?.ToString() ?? "";
        bool isOwner = string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner)
        {
            return;
        }

        Button btn = sender as Button;
        int requestId = Convert.ToInt32(btn.CommandArgument);
        service.RejectJoinRequest(requestId);
        LoadRequests();
    }

    private void ClearEventForm()
    {
        txtEventTitle.Text = "";
        txtEventDate.Text = "";
        txtEventTime.Text = "";
        txtEventNotes.Text = "";
    }
}

