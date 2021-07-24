using NewsParserBeta;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace NewsAPI_beta.Controllers
{
    public class TopTenController : ApiController
    {
        // GET api/topten
        public IEnumerable<WordsCounterModel> Get()
        {
            List<SqlRow> news = new List<SqlRow>();
            SqlConnection cn = new SqlConnection("Server = DARKHAN\\SQLEXPRESS; Database = TestDB; Integrated Security = True;");
            string command = string.Format(@"SELECT * FROM News");
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

            string allWords = "";

            foreach (var item in news)
            {
                allWords += item.Content;
                allWords += " ";
            }

            string[] words = allWords.Split();
            var topUsed = words.GroupBy(
                        word => word,
                        (key, counts) => new WordsCounterModel
                        {
                            Key = key,
                            Cnt = counts.Count()
                        }).OrderByDescending(x => x.Cnt).Take(10);

            
            return topUsed;

        }

    }

    public class WordsCounterModel
    {
        public string Key {get;set;}
        public int Cnt {get;set;}
    }


    


}
