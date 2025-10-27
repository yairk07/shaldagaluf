using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class exusers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        populatedata();
}            
    
public void populatedata()
    {
        UsersService us = new UsersService();
        DataSet data = us.getallusers();
        Gridregisterdusers.DataSource = data;
        Gridregisterdusers.DataBind();
    }       
    }
