using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class home : System.Web.UI.Page
{
    calnderservice calendarService = new calnderservice();
    private DataSet allEvents;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // בדיקת התחברות
            if (Session["username"] != null)
            {
                string name = Session["username"].ToString();
                lblWelcome.Text = $"<h3>ברוך הבא, {name}!</h3>";
            }
            else
            {
                // אם המשתמש לא מחובר – נחזיר לדף התחברות
                Response.Redirect("login.aspx");
                return; // חשוב – כדי לא להמשיך להריץ את שאר הקוד
            }

            // טעינת אירועים
            allEvents = calendarService.GetAllEvents();
            ViewState["AllEvents"] = allEvents;

            calendar.SelectedDate = DateTime.Today;
            ShowEvents(calendar.SelectedDate);
        }
        else
        {
            allEvents = (DataSet)ViewState["AllEvents"];
        }
    }
    
    protected void calendar_SelectionChanged(object sender, EventArgs e)
    {
        DateTime selectedDate = calendar.SelectedDate;
        lblSelectedDate.Text = $"בחרת את התאריך: {selectedDate:dd/MM/yyyy}";
        ShowEvents(selectedDate);
    }

    private void ShowEvents(DateTime date)
    {
        lblEvents.Text = "";
        int count = 0;

        foreach (DataRow row in allEvents.Tables[0].Rows)
        {
            DateTime eventDate = Convert.ToDateTime(row["date"]);
            if (eventDate.Date == date.Date)
            {
                string title = row["title"].ToString();
                string time = row["time"].ToString();
                string note = row["notes"].ToString();

                lblEvents.Text += $@"
                    <div class='event-card'>
                        <div class='event-title'>📌 {title}</div>";

                if (!string.IsNullOrEmpty(time))
                    lblEvents.Text += $"<div class='event-time'>⏰ בשעה {time}</div>";

                if (!string.IsNullOrEmpty(note))
                    lblEvents.Text += $"<div class='event-note'>📝 {note}</div>";

                lblEvents.Text += "</div>";
                count++;
            }
        }

        if (count == 0)
        {
            lblEvents.Text = "<div style='color:gray;'>אין אירועים לתאריך הזה.</div>";
        }
    }

    protected void calendar_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime currentDay = e.Day.Date;

        foreach (DataRow row in allEvents.Tables[0].Rows)
        {
            DateTime eventDate = Convert.ToDateTime(row["date"]);
            if (eventDate.Date == currentDay)
            {
                string title = row["title"].ToString();
                if (title.Length > 12)
                    title = title.Substring(0, 12) + "...";

                e.Cell.Controls.Add(new Literal
                {
                    Text = $"<br/><span style='font-size:10px; color:blue;'>{title}</span>"
                });
            }
        }
    }

}
