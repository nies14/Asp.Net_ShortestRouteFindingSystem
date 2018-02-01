using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DisconnectedExample
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PanelLogin.Visible = false;
                if (!IsPostBack)
                {
                    if (Cache["DS"] == null)
                        this.LoadFromDb();
                    DataSet ds = (DataSet)Cache["DS"];
                    DropDownListSource.DataSource = ds.Tables["Districts"];
                    DropDownListDestination.DataSource = ds.Tables["Districts"];
                    DropDownListSource.DataTextField = "DistrictName";
                    DropDownListSource.DataValueField = "DistrictId";
                    DropDownListDestination.DataTextField = "DistrictName";
                    DropDownListDestination.DataValueField = "DistrictId";
                    DropDownListSource.DataBind();
                    DropDownListDestination.DataBind();
                }
            }
            if (Session["id"] != null)
                Response.Redirect("~\\Admin.aspx");
        }

        protected void LoadFromDb()
        {
            string query1 = "select * from Districts";
            string query2 = "select * from Routes";
            string connString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = connString;
            conn.Open();
            SqlDataAdapter districtAdapter = new SqlDataAdapter(query1, conn);
            DataSet ds = new DataSet();
            districtAdapter.Fill(ds, "Districts");
            SqlCommandBuilder builder = new SqlCommandBuilder(districtAdapter);
            conn.Close();
            conn.Open();
            SqlDataAdapter routeAdapter = new SqlDataAdapter(query2, conn);
            routeAdapter.Fill(ds, "Routes");

            builder = new SqlCommandBuilder(routeAdapter);
            ds.Tables["Districts"].PrimaryKey = new DataColumn[] { ds.Tables["Districts"].Columns["DistrictId"] };
            ds.Tables["Routes"].PrimaryKey = new DataColumn[] { ds.Tables["Routes"].Columns["RouteId"] };
            ds.Tables["Routes"].Columns["RouteId"].AutoIncrement = true;
            ds.Tables["Districts"].Columns["DistrictId"].AutoIncrement = true;
            ds.Tables["Districts"].Columns["DistrictName"].Unique = true;
            Cache["DS"] = ds;
            Cache["DistrictAdapter"] = districtAdapter;
            Cache["RouteAdapter"] = routeAdapter;
        }

        protected void imbtnAdminLogin_Click(object sender, ImageClickEventArgs e)
        {
            PanelLogin.Visible = true;
            PanelFinder.Visible = false;
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Length > 0 && txtPassword.Text.ToString().Length > 0)
            {
                lblMSG.Text = "";
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = string.Format("select * from Login where UserName = '{0}' and Password = '{1}'", txtUserName.Text, txtPassword.Text);
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Session["id"] = "admin";
                        Response.Redirect("~\\Admin.aspx");
                    }
                    else
                    {
                        lblMSG.Text = "Invalid UserName or Password";
                    }
                }
            }
            else
            {
                lblMSG.Text = "UserName or Password cannot be empty!";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            PanelLogin.Visible = false;
            PanelFinder.Visible = true;

            lblMSG.Text = "";
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            if (DropDownListSource.SelectedValue != DropDownListDestination.SelectedValue)
            {
                Session["Source"] = Convert.ToInt32(DropDownListSource.SelectedValue);
                Session["Destination"] = Convert.ToInt32(DropDownListDestination.SelectedValue);
                Response.Redirect("~\\Finder.aspx");
            }
            else
            {
                lblError.Text = "Source and Destination cannot be the same";
            }
        }
    }
}