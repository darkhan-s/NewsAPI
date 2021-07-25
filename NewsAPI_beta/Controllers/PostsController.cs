using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Http;

namespace NewsAPI_beta.Controllers
{
    public class PostsController : ApiController
    {
        // GET api/posts
        /// <summary>
        /// Returns all news in the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SqlRow> Get()
        {
            //refresh DB before GET request
            UpdateTable.Update();
            List<SqlRow> news = new List<SqlRow>();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection cn = new SqlConnection(connectionString);
            string command = "select * from News";
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                news.Add(new SqlRow
                {
                    Title = (string)dr["Title"],
                    Date = dr["Date"].ToString(),
                    Content = (string)dr["Content"],
                }
                    );

            }
            cn.Close();
            return news;
        }

        // GET /api/posts?from=&to
        /// <summary>
        /// Returns news between "from" and "to" timeframes
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<SqlRow> Get(DateTime from, DateTime to)
        {
            //refresh DB before GET request
            UpdateTable.Update();

            List<SqlRow> news = new List<SqlRow>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection cn = new SqlConnection(connectionString);
            string command = string.Format(@"SELECT * FROM News WHERE [Date] BETWEEN '{0}' AND '{1}' ", from.ToString(CultureInfo.InvariantCulture), to.ToString(CultureInfo.InvariantCulture));
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                news.Add(new SqlRow
                {
                    Title = (string)dr["Title"],
                    Date = dr["Date"].ToString(),
                    Content = (string)dr["Content"],
                }
                    );

            }
            cn.Close();
            return news;

        }

    }
}
