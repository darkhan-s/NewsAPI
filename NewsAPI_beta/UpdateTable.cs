using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAPI_beta
{
    public static class UpdateTable
    {

        public static readonly string Source = "https://kapital.kz/tehnology?page=";

        public static void Update()
        {

            Parser parser = new Parser();

            // To load 30 news
            for (int i = 1; i < 4; i++)
            {
                parser.Parse(Source + i.ToString());
            }

            Connector connector = new Connector();
            connector.Publish(parser.Rows);

        }


    }
    
}