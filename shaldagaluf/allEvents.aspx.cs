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
        DataTable dt = es.GetAllEvents();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter =
                $"Title LIKE '%{filter}%' " +
                $"OR UserName LIKE '%{filter}%' " +
                $"OR Notes LIKE '%{filter}%'";
            dt = dv.ToTable();
        }

        dlEvents.DataSource = dt;
        dlEvents.DataBind();

        lblResult.Text = filter == ""
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
