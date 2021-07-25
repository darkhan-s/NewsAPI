using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;


namespace NewsAPI_beta
{
    /// <summary>
    /// Class to parse articles from fixed source and store them in SqlRows class
    /// </summary>
    public class Parser
    {
        public Parser()
        {
        }
        
        // counter for the articles, increment by 10 when the page is parsed
        private int shift = 0;

        public void Parse(string webpage)
        {

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(webpage);

            // complete page
            var newsHTML = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("main-news")).ToList();

            // section with articles
            var articlesList = newsHTML[0].Descendants("article")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("main-news__item")).ToList();


            //articles split into a list
            List<HtmlNode> textboxList = new List<HtmlNode>();
            foreach (HtmlNode item in articlesList)
            {
                var temp = item.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("main-news__txt")).ToList();
                textboxList.AddRange(temp);
            }

            // article header
            List<HtmlNode> dateHeaderList = new List<HtmlNode>();
            foreach (HtmlNode item in textboxList)
            {
                var temp = item.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("main-news__info information-article")).ToList();
                dateHeaderList.AddRange(temp);
            }

            // list with dates of the article 
            List<HtmlNode> dateList = new List<HtmlNode>();
            foreach (HtmlNode item in dateHeaderList)
            {
                var temp = item.Descendants("time")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("information-article__date")).ToList();

                dateList.AddRange(temp);
            }


            // list with titles of the articles
            List<HtmlNode> titleList = new List<HtmlNode>();
            foreach (HtmlNode item in textboxList)
            {

                var temp = item.Descendants("a")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("main-news__name")).ToList();
                titleList.AddRange(temp);
            }



            // list with body of the articles
            List<HtmlNode> contentList = new List<HtmlNode>();
            foreach (HtmlNode item in textboxList)
            {
                var temp = item.Descendants("p")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("main-news__anons")).ToList();

                contentList.AddRange(temp);
            }

            ConvertToRows(titleList, dateList, contentList);



        }

        /// <summary>
        /// Convert HTML nodes to sql rows class
        /// </summary>
        /// <param name="titleList"></param>
        /// <param name="dateList"></param>
        /// <param name="contentList"></param>
        private void ConvertToRows(List<HtmlNode> titleList, List<HtmlNode> dateList, List<HtmlNode> contentList)
        {


            for (int i = 0; i < titleList.Count; i++)
            {
                SqlRow row = new SqlRow();
                Rows.Add(row);

                // write parsed data to Rows class 
                Rows[shift + i].Title = titleList[i].InnerText;
                Rows[shift + i].Date = FixDateTime(dateList[i].InnerText);
                Rows[shift + i].Content = contentList[i].InnerText;
            }
            shift = shift + 10;
        }

        /// <summary>
        /// Fix datetime HTML formatting to SQL acceptable data
        /// </summary>
        /// <param name="rawDate"></param>
        /// <returns></returns>
        private string FixDateTime(string rawDate)
        {
            rawDate = rawDate.Replace(" &middot; ", "-");
            DateTime dateTime = DateTime.ParseExact(rawDate, "dd.MM.yyyy-HH:mm", CultureInfo.InvariantCulture);
            
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss:fff");
        }

        public List<SqlRow> Rows { get; set; } = new List<SqlRow>();
        public int Shift {get; set;}

    }
}
