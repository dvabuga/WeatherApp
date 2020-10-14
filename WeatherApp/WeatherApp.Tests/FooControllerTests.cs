using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp.Tests
{

    [TestFixture]
    public class FooControllerTests
    {
        private FooController _fooController;

        [SetUp]
        public void Setup()
        {
            _fooController = new FooController();
        }


        [Test]
        public void Test1Async()
        {
            var r =  _fooController.Index();
            var v = r as OkObjectResult;
            var res = v.Value;
        

            Assert.AreEqual("1", res);
        }
    }
}