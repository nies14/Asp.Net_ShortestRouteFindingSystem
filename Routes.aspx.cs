using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DisconnectedExample
{
    public partial class Routes : System.Web.UI.Page
    {
        protected void LoadFromCache()
        {
            if (Cache["DS"] == null)
                this.LoadFromDb();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select RouteId, d1.DistrictName as Source, d2.DistrictName as Destination, Cost from Routes, Districts d1, Districts d2 where Source = d1.DistrictId and Destination = d2.DistrictId";
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            GridView1.DataSource = reader;
            GridView1.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null)
                Response.Redirect("~\\Index.aspx");
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
            lblErrorMsgDlist.Text = "";
            lblerrormsg.Text = "";
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            this.LoadFromCache();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Cache["DS"] == null)
                this.LoadFromDb();
            DataSet ds = (DataSet)Cache["DS"];
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["RouteAdapter"];
            DataRow rowToDelete = ds.Tables["Routes"].Rows.Find(e.Keys["RouteId"]);
            rowToDelete.Delete();
            adapter.Update(ds.Tables["Routes"]);
            this.LoadFromCache();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.LoadFromCache();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Cache["DS"] == null)
                this.LoadFromDb();
            DataSet ds = (DataSet)Cache["DS"];
            DataRow rowToUpdate = ds.Tables["Routes"].Rows.Find(e.Keys["RouteId"]);
            rowToUpdate["Cost"] = e.NewValues["Cost"];
            Cache["DS"] = ds;
            //ds.AcceptChanges();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["RouteAdapter"];
            adapter.Update(ds.Tables["Routes"]);
            GridView1.EditIndex = -1;
            this.LoadFromCache();
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

        protected void btnGoback_Click(object sender, EventArgs e)
        {
            Response.Redirect("~\\Admin.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //validate txtcost
            if (txtCost.Text.Length > 0 && (int.Parse(txtCost.Text) > 0) && (DropDownListDestination.SelectedValue != DropDownListSource.SelectedValue))
            {
                if (Cache["DS"] == null)
                    this.LoadFromDb();
                DataSet ds = (DataSet)Cache["DS"];
                DataRow dr = ds.Tables["Routes"].NewRow();
                dr["Source"] = Convert.ToInt32(DropDownListSource.SelectedValue);
                dr["Destination"] = Convert.ToInt32(DropDownListDestination.SelectedValue);
                dr["Cost"] = Convert.ToInt32(txtCost.Text);
                dr["RouteId"] = 0;
                ds.Tables["Routes"].Rows.Add(dr);
                Cache["DS"] = ds;
                //ds.AcceptChanges();
                SqlDataAdapter adapter = (SqlDataAdapter)Cache["RouteAdapter"];
                adapter.Update(ds.Tables["Routes"]);
                this.LoadFromCache();
                this.LoadFromDb();
            }
            else if (DropDownListDestination.SelectedValue == DropDownListSource.SelectedValue)
            {
                lblErrorMsgDlist.Text = " * Source and Destianation can not be same ";
            }
            if (txtCost.Text.Length == 0 || (int.Parse(txtCost.Text) <= 0))
            {
                lblerrormsg.Text = " * Cost must be positive number ";
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            this.LoadFromCache();
        }
    }
}