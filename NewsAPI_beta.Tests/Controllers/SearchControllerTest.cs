using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsAPI_beta.Controllers;

namespace NewsAPI_beta.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            SearchController controller = new SearchController();

            // Act
            IEnumerable<SqlRow> result = controller.Get("в");

            // Assert
            Assert.IsNotNull(result);
        }
        
        
    }
}
