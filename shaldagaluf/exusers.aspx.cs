using System;
using System.Data;
using System.Linq;

public partial class exusers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!IsPostBack)
            BindUsers();
    }

    private void BindUsers(string search = "")
    {
        var us = new UsersService();
        var ds = us.getallusers();

        if (ds == null || ds.Tables.Count == 0)
        {
            DataListUsers.DataSource = null;
            DataListUsers.DataBind();
            return;
        }

        DataTable t = ds.Tables[0];

        if (!string.IsNullOrWhiteSpace(search))
        {
            // סינון עם DataView כדי לשמור את מבנה העמודות (כולל CityName)
            var view = new DataView(t);
            string q = search.Replace("'", "''");
            view.RowFilter =
                $"userName LIKE '%{q}%' OR email LIKE '%{q}%' OR firstName LIKE '%{q}%' OR lastName LIKE '%{q}%' OR phonenum LIKE '%{q}%' OR CityName LIKE '%{q}%'";

            DataListUsers.DataSource = view;
        }
        else
        {
            DataListUsers.DataSource = t;
        }

        DataListUsers.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindUsers(txtSearchemail.Text);
    }
    // מחזיר את שם העיר לפי העמודה שקיימת בפועל בדאטהסט
    protected string GetCity(object dataItem)
    {
        var drv = dataItem as System.Data.DataRowView;
        if (drv == null) return string.Empty;

        var table = drv.DataView?.Table;
        if (table == null) return string.Empty;

        // נסיונות לפי שמות עמודות אפשריים
        string[] names = { "CityName", "cityname", "citys.cityname", "c.cityname", "city" };

        foreach (var name in names)
        {
            if (table.Columns.Contains(name))
                return Convert.ToString(drv[name]);
        }
        return string.Empty;
    }

}
