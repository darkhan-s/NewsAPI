using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsAPI_beta.Controllers;

namespace NewsAPI_beta.Tests.Controllers
{
    [TestClass]
    public class TopTenControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            TopTenController controller = new TopTenController();

            // Act
            IEnumerable<Models.WordsCounterModel> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count());
        }
        
        
    }
}
