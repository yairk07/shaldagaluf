using System;
using System.Data;

public partial class allEvents : System.Web.UI.Page
{
    EventService es = new EventService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData(string filter = "")
    {
        int? userId = null;
        string role = Session["Role"]?.ToString();
        
        if (role != "owner" && Session["userId"] != null)
        {
            userId = Convert.ToInt32(Session["userId"]);
        }

        DataTable dt = es.GetAllEvents(userId);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            DataView dv = dt.DefaultView;
            string escapedFilter = filter.Replace("'", "''");
            dv.RowFilter =
                $"Title LIKE '%{escapedFilter}%' " +
                $"OR UserName LIKE '%{escapedFilter}%' " +
                $"OR Notes LIKE '%{escapedFilter}%'";
            dt = dv.ToTable();
        }

        dlEvents.DataSource = dt;
        dlEvents.DataBind();

        lblResult.Text = string.IsNullOrWhiteSpace(filter)
            ? ""
            : $"נמצאו {dt.Rows.Count} תוצאות עבור: \"{filter}\"";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string search = txtSearch.Text.Trim();
        BindData(search);
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
    }
}
