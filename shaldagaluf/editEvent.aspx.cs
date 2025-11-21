using System;
using System.Data;
using System.Data.OleDb;

public partial class editEvent : System.Web.UI.Page
{
    private int eventId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!int.TryParse(Request.QueryString["id"], out eventId))
        {
            Response.Redirect("allEvents.aspx");
            return;
        }

        if (!IsPostBack)
            LoadEvent();
    }

    private void LoadEvent()
    {
        string conStr = Connect.GetConnectionString();

        using (OleDbConnection con = new OleDbConnection(conStr))
        {
            string sql = "SELECT * FROM calnder WHERE Id = @id";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", eventId);

            con.Open();
            OleDbDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                Response.Redirect("allEvents.aspx");
                return;
            }

            int rowUserId = Convert.ToInt32(dr["UserId"]);
            int currentUserId = Convert.ToInt32(Session["userId"]);
            string role = Session["role"].ToString();

            // בדיקת הרשאות
            if (role != "owner" && rowUserId != currentUserId)
            {
                Response.Write("אין לך הרשאה לערוך את האירוע הזה.");
                Response.End();
            }

            txtTitle.Text = dr["title"].ToString();
            txtDate.Text = Convert.ToDateTime(dr["date"]).ToString("yyyy-MM-dd");
            txtTime.Text = dr["time"].ToString();
            txtNotes.Text = dr["notes"].ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string conStr = Connect.GetConnectionString();

        using (OleDbConnection con = new OleDbConnection(conStr))
        {
            string sql = @"
                UPDATE calnder
                SET title = @title,
                    [date] = @date,
                    [time] = @time,
                    notes = @notes
                WHERE Id = @id";

            OleDbCommand cmd = new OleDbCommand(sql, con);

            cmd.Parameters.AddWithValue("@title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtDate.Text));
            cmd.Parameters.AddWithValue("@time", txtTime.Text);
            cmd.Parameters.AddWithValue("@notes", txtNotes.Text);
            cmd.Parameters.AddWithValue("@id", eventId);

            con.Open();
            cmd.ExecuteNonQuery();

            Response.Redirect("allEvents.aspx");
        }
    }
}
