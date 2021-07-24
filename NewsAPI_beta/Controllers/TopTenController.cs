﻿using NewsAPI_beta.Models;
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


            cn.Close();
            return topUsed;

        }

    }



    


}
