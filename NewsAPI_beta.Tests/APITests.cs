using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewsAPI_beta.Tests
{
    [TestClass]
    public class APITests
    {
        [TestMethod]
        public void IsRowsCountCorrect()
        {
            Parser parser = new Parser();
            for (int i = 1; i < 4; i++)
            {
                parser.Parse("https://kapital.kz/tehnology?page=" + i.ToString());
            }

            Assert.AreEqual(30, parser.Rows.Count);
        }


    }
}
