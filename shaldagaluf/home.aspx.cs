using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

public partial class home : System.Web.UI.Page
{
    calnderservice calendarService = new calnderservice();
    private DataSet allEvents;
    private static readonly CultureInfo heCulture = new CultureInfo("he-IL");

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
                Response.Redirect("login.aspx");
                return; 
            }

            allEvents = calendarService.GetAllEvents();
            ViewState["AllEvents"] = allEvents;

            calendar.SelectedDate = DateTime.Today;
            ShowEvents(calendar.SelectedDate);
        }
        else
        {
            allEvents = (DataSet)ViewState["AllEvents"];
        }

        UpdateCalendarMeta(calendar.SelectedDate);
    }
    
    protected void calendar_SelectionChanged(object sender, EventArgs e)
    {
        DateTime selectedDate = calendar.SelectedDate;
        ShowEvents(selectedDate);
        UpdateCalendarMeta(selectedDate);
    }

    private void ShowEvents(DateTime date)
    {
        lblEvents.Text = "";
        int count = 0;

        foreach (DataRow row in allEvents.Tables[0].Rows)
        {
            DateTime eventDate;

            if (!DateTime.TryParse(row["date"].ToString(), out eventDate))
            {
                continue;
            }

            if (eventDate.Date == date.Date)
            {
                string title = row["title"].ToString();
                string time = row["time"].ToString();
                string note = row["notes"].ToString();

                lblEvents.Text += $@"
                    <div class='calendar-event'>
                        <div class='calendar-event-title'>{title}</div>";

                if (!string.IsNullOrEmpty(time))
                    lblEvents.Text += $"<div class='calendar-event-meta'>⏰ {time}</div>";

                if (!string.IsNullOrEmpty(note))
                    lblEvents.Text += $"<div class='calendar-event-note'>📝 {note}</div>";

                lblEvents.Text += "</div>";
                count++;
            }
        }

        if (count == 0)
        {
            lblEvents.Text = "<div class='calendar-event empty'>אין אירועים לתאריך הזה.</div>";
        }
    }

    protected void calendar_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime currentDay = e.Day.Date;

        foreach (DataRow row in allEvents.Tables[0].Rows)
        {
            DateTime eventDate;

            if (!DateTime.TryParse(row["date"].ToString(), out eventDate))
                continue;

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

    private void UpdateCalendarMeta(DateTime date)
    {
        lblMetaDay.Text = date.ToString("dd");
        lblMetaWeekday.Text = date.ToString("dddd", heCulture);
        lblMetaFullDate.Text = date.ToString("dd MMM yyyy", heCulture);
        lblSelectedDate.Text = $"בחרת את התאריך: {date:dd/MM/yyyy}";
    }
}
