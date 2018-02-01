using System;

namespace DisconnectedExample
{
    public partial class Admin1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null)
                Response.Redirect("~\\Index.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Response.Redirect("~\\Index.aspx");
        }
    }
}