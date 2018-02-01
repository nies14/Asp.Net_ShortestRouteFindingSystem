using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DisconnectedExample
{
    public partial class Districts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PanelSearch.Visible = false;
            if (Session["id"] == null)
                Response.Redirect("~\\Index.aspx");
        }

        protected void LoadFromCacheDistricts()
        {
            if (Cache["DS"] == null)
                this.LoadFromDb();
            DataSet ds = (DataSet)Cache["DS"];
            GridViewDistricts.DataSource = ds.Tables["Districts"];
            GridViewDistricts.DataBind();
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

        protected void GridViewDistricts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewDistricts.EditIndex = e.NewEditIndex;
            if (Convert.ToInt32(Session["flag"]) == 1)
                this.LoadFromCacheDistricts();
            else
                this.LoadByPattern();
        }

        protected void GridViewDistricts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Cache["DS"] == null)
                this.LoadFromDb();
            DataSet ds = (DataSet)Cache["DS"];
            DataRow rowToUpdate = ds.Tables["Districts"].Rows.Find(e.Keys["DistrictId"]);
            rowToUpdate["DistrictName"] = e.NewValues["DistrictName"];
            Cache["DS"] = ds;
            //ds.AcceptChanges();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["DistrictAdapter"];
            adapter.Update(ds.Tables["Districts"]);
            GridViewDistricts.EditIndex = -1;
            if (Convert.ToInt32(Session["flag"]) == 1)
                this.LoadFromCacheDistricts();
            else
                this.LoadByPattern();
        }

        protected void GridViewDistricts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewDistricts.EditIndex = -1;
            if (Convert.ToInt32(Session["flag"]) == 1)
                this.LoadFromCacheDistricts();
            else
                this.LoadByPattern();
        }

        protected void GridViewDistricts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Cache["DS"] == null)
                this.LoadFromDb();
            DataSet ds = (DataSet)Cache["DS"];
            DataRow rowToDelete = ds.Tables["Districts"].Rows.Find(e.Keys["DistrictId"]);
            rowToDelete.Delete();
            Cache["DS"] = ds;
            //ds.AcceptChanges();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["DistrictAdapter"];
            adapter.Update(ds.Tables["Districts"]);
            if (Convert.ToInt32(Session["flag"]) == 1)
                this.LoadFromCacheDistricts();
            else
                this.LoadByPattern();
        }

        protected void btnAddDistrict_Click(object sender, EventArgs e)
        {
            PanelSearch.Visible = false;
            //validate txtDistrict
            if (txtDistrict.Text.Trim().Length > 0)
            {
                if (Cache["DS"] == null)
                    this.LoadFromDb();
                DataSet ds = (DataSet)Cache["DS"];
                DataRow dr = ds.Tables["Districts"].NewRow();
                dr["DistrictName"] = txtDistrict.Text;
                dr["DistrictId"] = 0;
                ds.Tables["Districts"].Rows.Add(dr);
                Cache["DS"] = ds;
                //ds.AcceptChanges();
                SqlDataAdapter adapter = (SqlDataAdapter)Cache["DistrictAdapter"];
                adapter.Update(ds.Tables["Districts"]);
                this.LoadFromCacheDistricts();
                this.LoadFromDb();
            }
            else
            {
                lblerrormsg.Text = " * District Can not be Empty!";
            }
        }

        protected void btnLoadDistricts_Click(object sender, EventArgs e)
        {
            PanelSearch.Visible = false;
            Session["flag"] = 1;
            this.LoadFromCacheDistricts();
        }

        protected void btnGoback_Click(object sender, EventArgs e)
        {
            Response.Redirect("~\\Admin.aspx");
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            Session["flag"] = 2;
            this.LoadByPattern();
        }

        protected void LoadByPattern()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("select * from Districts where DistrictName like '%{0}%'", txtSearch.Text);
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            GridViewDistricts.DataSource = reader;
            GridViewDistricts.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PanelSearch.Visible = !PanelSearch.Visible;
        }
    }
}