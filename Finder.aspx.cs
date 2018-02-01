using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DisconnectedExample
{
    public partial class Finder : System.Web.UI.Page
    {
        private List<Pair>[] adjList;
        private int[] parents;
        private int[] costs;
        private Comparator comp = new Comparator();

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select max(DistrictId) as DistrictId from Districts";
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            int max = 0;
            while (reader.Read())
            {
                max = (int)reader["DistrictId"];
            }
            adjList = new List<Pair>[max + 5];
            parents = new int[max + 5];
            costs = new int[max + 5];
            for (int i = 0; i < max + 5; i++)
                costs[i] = int.MaxValue;
            cmd.CommandText = "select Source, Destination, Cost from Routes";
            reader.Close();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int source, destination, cost;
                source = (int)reader["Source"];
                destination = (int)reader["Destination"];
                cost = (int)reader["Cost"];
                if (adjList[source] == null)
                    adjList[source] = new List<Pair>();
                if (adjList[destination] == null)
                    adjList[destination] = new List<Pair>();
                adjList[source].Add(new Pair(destination, cost));
                adjList[destination].Add(new Pair(source, cost));
            }
            int s = (int)Session["Source"];
            int d = (int)Session["Destination"];
            int ans = UCS(s, d);
            if (ans == 0)
            {
                Label1.Text = "No Path Found";
            }
            else
            {
                Stack<int> st = new Stack<int>();
                int i = d;
                while (i != 0)
                {
                    //Response.Write("ok");
                    st.Push(i);
                    i = parents[i];
                }
                Response.Write("<center><span style='color:Blue;'><h1>Least Cost Path From " + GetDistrictName(s) + " To " + GetDistrictName(d));
                Response.Write("</h1></span><hr/><table border='1'> <th>Source</th> <th>Destination</th> <th>Cost</th>");
                while (st.Count != 1)
                {
                    Response.Write("<tr><td>");
                    int rScource, rDestination;
                    rScource = st.Peek();
                    st.Pop();
                    rDestination = st.Peek();
                    Response.Write(GetDistrictName(rScource) + "</td><td>");

                    Response.Write(GetDistrictName(rDestination));
                    Response.Write("</td><td>");
                    Response.Write(FindCost(rScource, rDestination));
                    Response.Write("</td></tr>");
                }
                Response.Write("</table>");
                Response.Write("<span style='color:Green;font-size:40px;'>Total Cost: " + ans + "</span>");
                Response.Write("</table><hr></center>");
            }
            conn.Dispose();
        }

        public int FindCost(int u, int v)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("select min(Cost) as cost from Routes where Source = {0} and Destination = {1}", u, v);
            //Response.Write(cmd.CommandText);
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            int cost = 0;
            while (reader.Read())
            {
                cost = Convert.ToInt32(reader["cost"]);
            }
            return cost;
        }

        public string GetDistrictName(int id)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("select DistrictName from Districts where DistrictId = {0}", id);
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = (string)reader["DistrictName"];
            }
            return name;
        }

        public int UCS(int u, int v)
        {
            //Response.Write(GetDistrictName(u) + " " + GetDistrictName(v));
            ArrayList list = new ArrayList();
            int node, cost, node2, cost2;
            list.Add(new Node(u, 0, 0));
            //costs[u] = 0;
            while (list.Count != 0)
            {
                node = ((Node)list[0]).Id;
                cost = ((Node)list[0]).Cost;
                parents[node] = ((Node)list[0]).Parent;
                //costs[node] = ((Node)list[0]).Cost;

                if (node == v)
                    return cost;
                list.RemoveAt(0);
                //len = adjList[node].Count;
                foreach (Pair temp in adjList[node])
                {
                    node2 = temp.First;
                    cost2 = temp.Second;
                    //Response.Write("Node: " + GetDistrictName(node2) + " Cost: " + (cost + cost2) + " Parent: " + GetDistrictName(node) + "<br />");
                    //if (parents[node2] != node && costs[node2] > cost + cost2)
                    //{
                    if (parents[node] != node2)
                    {
                        list.Add(new Node(node2, cost + cost2, node));
                        list.Sort(comp);
                    }
                    //parents[node2] = node;
                    //costs[node2] = cost + cost2;
                    //}
                }
            }
            return 0;
        }
    }

    public struct Pair
    {
        public int First { get; set; }
        public int Second { get; set; }

        public Pair(int first, int second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    public class Node
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int Parent { get; set; }

        public Node(int id, int cost, int parent)
        {
            this.Id = id;
            this.Cost = cost;
            this.Parent = parent;
        }
    }

    public class Comparator : IComparer
    {
        public int Compare(object x, object y)
        {
            int val1 = ((Node)x).Cost;
            int val2 = ((Node)y).Cost;
            if (val1 >= val2)
                return 1;
            return 0;
        }
    }
}