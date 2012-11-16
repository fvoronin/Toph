using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Toph.UI;

namespace Toph.Tests.UI
{
    [TestFixture]
    public class RouteTests
    {
        private RouteCollection _routes;

        [TestFixtureSetUp]
        public void Before_all_tests()
        {
            RouteConfig.RegisterRoutes(_routes = new RouteCollection());
        }

        [Test]
        public void HomeIndex()
        {
            Test("~/", "home", "index");
        }

        [Test]
        public void HomeAbout()
        {
            Test("~/about", "home", "about");
        }

        [Test]
        public void UserIndex()
        {
            Test("~/johndoe", "user", "index", new {username = "johndoe"});
            Test("~/johndoe", "user", "index", null, new {username = "johndoe"});
        }

        [Test]
        public void UserAdd()
        {
            Test("~/johndoe/add", "user", "add", new {username = "johndoe"});
        }

        [Test]
        public void CustomerIndex()
        {
            Test("~/johndoe/acme", "customer", "index", new {username = "johndoe", customer = "acme"});
        }

        [Test]
        public void AccountLogin()
        {
            Test("~/account/login", "account", "login");
        }

        [Test]
        public void AccountLogOff()
        {
            Test("~/account/logoff", "account", "logoff");
        }

        [Test]
        public void AccountRegister()
        {
            Test("~/account/register", "account", "register");
        }

        /************************************************************************************************************/
        /************************************************************************************************************/

        private void Test(string path, string controller, string action, object routeValues = null, object existingRouteValues = null)
        {
            var values = new RouteValueDictionary(routeValues);
            values["controller"] = controller;
            values["action"] = action;

            var existingRouteData = new RouteData();
            foreach (var routeValue in new RouteValueDictionary(existingRouteValues))
                existingRouteData.Values[routeValue.Key] = routeValue.Value;

            // First check to see if we can get the correct path from the passed route values
            var virtualPathData = _routes.GetVirtualPath(new RequestContext(StubHttpContext(""), existingRouteData), values);
            Assert.IsNotNull(virtualPathData, "VirtualPathData not found from route values");
            Assert.AreEqual(path, virtualPathData.VirtualPath);

            // Then see if the routing works in reverse (starting with the path)
            var routeData = _routes.GetRouteData(StubHttpContext(path));
            Assert.IsNotNull(routeData, "RouteData not found from path '{0}'", path);

            foreach (var key in values.Keys.Where(x => values[x] != null))
            {
                Assert.IsTrue(routeData.Values.ContainsKey(key), "Generated route doesn't contain '{0}'", key);
                Assert.AreEqual(values[key].ToString().ToLower(), routeData.Values[key].ToString().ToLower(), "Generated route value for '{0}' is wrong", key);
            }
        }

        private HttpContextBase StubHttpContext(string path)
        {
            var httpRequest = MockRepository.GenerateStub<HttpRequestBase>();
            httpRequest.Stub(x => x.ApplicationPath).Return("");
            httpRequest.Stub(x => x.AppRelativeCurrentExecutionFilePath).Return(path);

            var httpResponse = MockRepository.GenerateStub<HttpResponseBase>();
            httpResponse
                .Stub(x => x.ApplyAppPathModifier(Arg<string>.Is.Anything))
                .Return(null)
                .WhenCalled(x => x.ReturnValue = ((string)x.Arguments[0]).StartsWith("~") ? x.Arguments[0] : "~" + x.Arguments[0]);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.Stub(x => x.Request).Return(httpRequest);
            httpContext.Stub(x => x.Response).Return(httpResponse);

            return httpContext;
        }
    }
}
