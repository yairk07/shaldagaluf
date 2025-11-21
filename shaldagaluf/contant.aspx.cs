using System;
using System.Data;

public partial class Default3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("image");
            dt.Columns.Add("text");
            dt.Columns.Add("url");

            dt.Rows.Add("pics/chat.png",
                        "",
                        "https://chatgpt.com/");

            dt.Rows.Add("pics/scouts.png",
                        "המסע בצופים",
                        "https://prod-hamasa.zofim.org.il/he/login/");

            dt.Rows.Add("pics/math.jpg",
                        "עזרה בלימודים ",
                        "https://tiktek.com/il/heb-index.htm");

            dlCards.DataSource = dt;
            dlCards.DataBind();
        }
    }
}
