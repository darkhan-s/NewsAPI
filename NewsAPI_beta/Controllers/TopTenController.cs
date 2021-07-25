using NewsAPI_beta.Models;
using System.Collections.Generic;
using System.Configuration;
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
            //refresh DB before GET request
            UpdateTable.Update();

            List<SqlRow> news = new List<SqlRow>();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection cn = new SqlConnection(connectionString);

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
                });

            }

            //concat all words into one string
            string allWords = "";

            foreach (var item in news)
            {
                allWords += item.Content;
                allWords += " ";
            }

            string[] words = allWords.Split();

            //select top 10 used words
            var topUsed = words.GroupBy(
                        word => word,
                        (key, counts) => new WordsCounterModel
                        {
                            Key = key,
                            Cnt = counts.Count()
                        }).OrderByDescending(x => x.Cnt).Take(10);


            cn.Close();
            return topUsed;

        }

    }



    


}
