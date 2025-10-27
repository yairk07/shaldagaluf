using System;
using System.Web.UI;

public partial class login : System.Web.UI.Page
{
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUserName.Text.Trim();
        string password = txtPassword.Text.Trim();

        UsersService service = new UsersService();

        if (service.IsExist(username, password))
        {
            Session["username"] = username;
            Session["loggedIn"] = true;
            Response.Redirect("Home.aspx");
        }
        else
        {
            lblError.Text = "שם משתמש או סיסמה שגויים.";
        }
    }
}
