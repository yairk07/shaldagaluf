using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class danimaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string pageName = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
            Body.Attributes["class"] = "page-" + pageName.ToLower(); // למשל: page-register
        }
    }


}
