using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

public partial class tasks : System.Web.UI.Page
{
    calnderservice calnderService = new calnderservice();
    private DataSet allEvents;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int? userId = null;
            string role = Session["Role"]?.ToString();
            
            if (role != "owner" && Session["userId"] != null)
            {
                userId = Convert.ToInt32(Session["userId"]);
            }

            allEvents = calnderService.GetAllEvents(userId);
            ViewState["AllEvents"] = allEvents;

            calendar.SelectedDate = DateTime.Today;
            lblSelectedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
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
        lblSelectedDate.Text = selectedDate.ToString("dd/MM/yyyy");
        ShowEvents(selectedDate);
    }

    protected void AddEvent(object sender, EventArgs e)
    {
        DateTime selectedDate = calendar.SelectedDate.Date;
        string title = txtTitle.Text.Trim();
        string time = txtTime.Text.Trim();
        string note = txtNote.Text.Trim();
        string category = ddlCategory.SelectedValue;

        if (!string.IsNullOrEmpty(title))
        {
            int? userId = null;
            if (Session["userId"] != null)
            {
                userId = Convert.ToInt32(Session["userId"]);
            }

            calnderService.InsertEvent(title, selectedDate, time, note, category, userId);

            txtTitle.Text = "";
            txtTime.Text = "";
            txtNote.Text = "";
            ddlCategory.SelectedIndex = 0;

            string role = Session["Role"]?.ToString();
            int? filterUserId = null;
            if (role != "owner" && userId.HasValue)
            {
                filterUserId = userId;
            }
            allEvents = calnderService.GetAllEvents(filterUserId);
            ViewState["AllEvents"] = allEvents;

            ShowEvents(selectedDate);
        }
    }

    private void ShowEvents(DateTime date)
    {
        var builder = new StringBuilder();
        int count = 0;

        foreach (DataTable table in allEvents.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row.IsNull("date"))
                    continue;

                DateTime eventDate;
                if (!DateTime.TryParse(row["date"].ToString(), out eventDate))
                    continue;
                if (eventDate.Date == date.Date)
                {
                    string title = HttpUtility.HtmlEncode(row["title"].ToString());
                    string time = HttpUtility.HtmlEncode(row["time"]?.ToString() ?? "");
                    string note = HttpUtility.HtmlEncode(row["notes"]?.ToString() ?? "");
                    string category = HttpUtility.HtmlEncode(row["category"]?.ToString() ?? "אחר");
                    string eventType = table.TableName == "SharedEvents" ? " (טבלה משותפת)" : "";

                    builder.Append("<article class='task-event-card'>");
                    builder.Append("<div class='task-event-title'>📌 ").Append(title).Append(eventType).Append("</div>");
                    builder.Append("<div class='task-event-category'>🏷️ ").Append(category).Append("</div>");

                    if (!string.IsNullOrEmpty(time))
                        builder.Append("<div class='task-event-meta'>⏰ ").Append(time).Append("</div>");

                    if (!string.IsNullOrEmpty(note))
                        builder.Append("<div class='task-event-note'>📝 ").Append(note).Append("</div>");

                    builder.Append("</article>");
                    count++;
                }
            }
        }

        if (count == 0)
        {
            builder.Append("<div class='task-empty'>אין אירועים לתאריך הזה.</div>");
        }

        lblEvents.Text = builder.ToString();
    }

    protected void calendar_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime currentDay = e.Day.Date;
        int dayCount = 0;

        foreach (DataTable table in allEvents.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row.IsNull("date"))
                    continue;

                DateTime eventDate;
                if (!DateTime.TryParse(row["date"].ToString(), out eventDate))
                    continue;
                if (eventDate.Date == currentDay.Date)
                {
                    dayCount++;
                }
            }
        }

        if (dayCount > 0)
        {
            e.Cell.Controls.Add(new Literal
            {
                Text = $"<span class='task-day-count'>{dayCount} אירועים</span>"
            });
        }
    }
}
