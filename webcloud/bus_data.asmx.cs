using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;

namespace webcloud
{
    /// <summary>
    /// Summary description for bus_data
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class bus_data : System.Web.Services.WebService
    {

        SqlConnection con = new SqlConnection("Data Source=SQL5004.myASP.NET;Initial Catalog=DB_9BB044_tuandc;User Id=DB_9BB044_tuandc_admin;Password=530440530440");
            
        [WebMethod]
        public DataSet getdata( string cmd)
        {
            con.Open();
            SqlCommand com = new SqlCommand(cmd,con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        [WebMethod]
        public string excutedata(string cmd)
        {
            string baocao="";
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand(cmd, con);
                com.ExecuteNonQuery();
                baocao = "OK";
            }
            catch (Exception)
            {
                baocao = "NO";
            }
            return baocao;            
        }

    }
}
