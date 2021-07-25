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
    public class PostsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            PostsController controller = new PostsController();

            // Act
            IEnumerable<SqlRow> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(30, result.Count());
        }

        [TestMethod]
        public void GetbyTime()
        {
            // Arrange
            PostsController controller = new PostsController();

            // Act
            var result= controller.Get(DateTime.Today.AddDays(-7), DateTime.Today);

            // Assert (should be always more than 1 news a week)
            Assert.AreNotEqual(0, result.Count());
        }
        
    }
}
