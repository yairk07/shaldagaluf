using System;
using System.Web;
using System.Web.UI;

public partial class danimaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // שמירה על הקלאס של ה-body (הקוד שהיה אצלך)
            string pageName = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
            Body.Attributes["class"] = "page-" + pageName.ToLower();

            // בדיקת רמת גישה
            string role = Convert.ToString(Session["Role"]);
            bool isOwner = role.Equals("owner", StringComparison.OrdinalIgnoreCase);

            // הסתרת דפים לבעלים בלבד
            if (linkUsers != null)
                linkUsers.Visible = isOwner;

            if (linkAllEvents != null)
                linkAllEvents.Visible = isOwner;

            if (linkEditEvent != null)
                linkEditEvent.Visible = isOwner;
        }
    }

    protected void lnkUserName_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("login.aspx");
    }
}
