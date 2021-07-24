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

            Parser parser = new Parser();

            // To load 30 news
            for (int i = 1; i < 4; i++)
            {
                parser.Parse("https://kapital.kz/tehnology?page=" + i.ToString());
            }

            Connector connector = new Connector("DARKHAN\\SQLEXPRESS", "TestDB");

            connector.Publish(parser.Rows);


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
            cn.Close();
            return news;

        }

    }
}
