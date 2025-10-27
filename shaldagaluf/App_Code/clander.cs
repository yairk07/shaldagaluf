using System;
using System.Web;

public class Calendar
{
    public int id { get; set; }
    public string title { get; set; }
    public DateTime date { get; set; }
    public string time { get; set; }
    public string notes { get; set; }
    public Calendar()
    {
        id = 0;
        title = "";
        date = DateTime.Today;
        time = "00:00";
        notes = "";
    }

    public Calendar(string title, DateTime date, string time, string notes)
    {
        id = 0;
        title = title;
        date = date;
        time = time;
        notes = notes;
    }

    public void InsertIntoDb()
    {
        calnderservice cs = new calnderservice();

        cs.InsertEvent(this.title, this.date, this.time, this.notes);
    }
}
