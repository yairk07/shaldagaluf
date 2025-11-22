using System;
using System.Data;
using System.Web.UI;
using System.Text;

public partial class sharedCalendars : System.Web.UI.Page
{
    SharedCalendarService service = new SharedCalendarService();

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

        if (!IsPostBack)
        {
            BindCalendars();
        }
    }

    private string FixEncoding(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        
        try
        {
            if (IsValidUtf8(text))
            {
                return text;
            }
            
            byte[] bytes = Encoding.GetEncoding("Windows-1255").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return text;
        }
    }
    
    private bool IsValidUtf8(string text)
    {
        try
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(text);
            string decoded = Encoding.UTF8.GetString(utf8Bytes);
            return decoded == text;
        }
        catch
        {
            return false;
        }
    }

    private void BindCalendars()
    {
        try
        {
            int userId = Convert.ToInt32(Session["userId"]);
            System.Diagnostics.Debug.WriteLine($"BindCalendars: Loading calendars for userId: {userId}");
            
            DataTable dt = service.GetAllSharedCalendars(userId);
            
            System.Diagnostics.Debug.WriteLine($"BindCalendars: Loaded {dt.Rows.Count} calendars");
            
            if (dt.Rows.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("BindCalendars: Columns:");
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {col.ColumnName} ({col.DataType.Name})");
                }
            }
            
            foreach (DataRow row in dt.Rows)
            {
                if (dt.Columns.Contains("CalendarName") && row["CalendarName"] != DBNull.Value)
                {
                    string original = row["CalendarName"].ToString();
                    row["CalendarName"] = FixEncoding(original);
                    System.Diagnostics.Debug.WriteLine($"BindCalendars: Fixed CalendarName: '{original}' -> '{row["CalendarName"]}'");
                }
                if (dt.Columns.Contains("Description") && row["Description"] != DBNull.Value)
                {
                    string original = row["Description"].ToString();
                    row["Description"] = FixEncoding(original);
                }
                if (dt.Columns.Contains("CreatorName") && row["CreatorName"] != DBNull.Value)
                {
                    string original = row["CreatorName"].ToString();
                    row["CreatorName"] = FixEncoding(original);
                }
            }
            
            if (dt != null && dt.Rows.Count > 0)
            {
                dlCalendars.DataSource = dt;
                dlCalendars.DataBind();
                dlCalendars.Visible = true;
                lblNoCalendars.Visible = false;
                
                System.Diagnostics.Debug.WriteLine($"BindCalendars: Successfully bound {dt.Rows.Count} calendars to DataList");
            }
            else
            {
                dlCalendars.Visible = false;
                lblNoCalendars.Visible = true;
                System.Diagnostics.Debug.WriteLine("BindCalendars: No calendars found, showing empty message");
            }
            
            System.Diagnostics.Debug.WriteLine($"BindCalendars: DataList bound with {dt.Rows.Count} items");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BindCalendars: Error: {ex.Message}\n{ex.StackTrace}");
            lblMessage.Text = "׳©׳’׳™׳׳” ׳‘׳˜׳¢׳™׳ ׳× ׳”׳˜׳‘׳׳׳•׳×. ׳׳ ׳ ׳ ׳¡׳” ׳©׳•׳‘.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnCreateNew_Click(object sender, EventArgs e)
    {
        pnlCreateForm.Visible = true;
        btnCreateNew.Visible = false;
    }

    protected void btnCancelCreate_Click(object sender, EventArgs e)
    {
        pnlCreateForm.Visible = false;
        btnCreateNew.Visible = true;
        txtCalendarName.Text = "";
        txtDescription.Text = "";
    }

    protected void btnSaveCalendar_Click(object sender, EventArgs e)
    {
        try
        {
            string name = txtCalendarName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblMessage.Text = "׳׳ ׳ ׳”׳–׳ ׳©׳ ׳׳˜׳‘׳׳”.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            System.Diagnostics.Debug.WriteLine($"btnSaveCalendar: Creating calendar - Name: '{name}', Description: '{description}'");

            int userId = Convert.ToInt32(Session["userId"]);
            int calendarId = service.CreateSharedCalendar(name, description, userId);

            System.Diagnostics.Debug.WriteLine($"btnSaveCalendar: Created calendar with ID: {calendarId}");

            if (calendarId > 0)
            {
                lblMessage.Text = "׳”׳˜׳‘׳׳” ׳ ׳•׳¦׳¨׳” ׳‘׳”׳¦׳׳—׳”!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                pnlCreateForm.Visible = false;
                btnCreateNew.Visible = true;
                txtCalendarName.Text = "";
                txtDescription.Text = "";
                BindCalendars();
            }
            else
            {
                lblMessage.Text = "׳©׳’׳™׳׳” ׳‘׳™׳¦׳™׳¨׳× ׳”׳˜׳‘׳׳”. ׳׳ ׳ ׳ ׳¡׳” ׳©׳•׳‘.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"btnSaveCalendar: Error: {ex.Message}\n{ex.StackTrace}");
            lblMessage.Text = $"׳©׳’׳™׳׳” ׳‘׳™׳¦׳™׳¨׳× ׳”׳˜׳‘׳׳”: {ex.Message}";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
}

