using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Http;
using NewsParserBeta;

namespace NewsAPI_beta.Controllers
{
    public class PostsController : ApiController
    {
        // GET api/posts
        public IEnumerable<SqlRow> Get()
        {
            List<SqlRow> news = new List<SqlRow>();
            SqlConnection cn = new SqlConnection("Server = DARKHAN\\SQLEXPRESS; Database = TestDB; Integrated Security = True;");
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
            return news;
        }

        // GET /api/posts?from=&to
        public IEnumerable<SqlRow> Get(DateTime from, DateTime to)
        {
            List<SqlRow> news = new List<SqlRow>();
            SqlConnection cn = new SqlConnection("Server = DARKHAN\\SQLEXPRESS; Database = TestDB; Integrated Security = True;");
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
            return news;

        }

    }
}
