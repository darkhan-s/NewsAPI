using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace NewsAPI_beta.Controllers
{
    public class SearchController : ApiController
    {

        // GET api/search
        public IEnumerable<SqlRow> Get(string text)
        {

            //refresh DB before GET request
            UpdateTable.Update();

            List<SqlRow> news = new List<SqlRow>();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection cn = new SqlConnection(connectionString);

            //return articles with {text} in body or title
            string command = string.Format(@"SELECT * FROM News WHERE [Title] LIKE N'%{0}%' OR [Content] LIKE N'%{1}%' ", text, text);
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
