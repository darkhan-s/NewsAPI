using NewsParserBeta;
using System.Collections.Generic;
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
            List<SqlRow> news = new List<SqlRow>();
            SqlConnection cn = new SqlConnection("Server = DARKHAN\\SQLEXPRESS; Database = TestDB; Integrated Security = True;");
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
            return news;

        }

    }
}
