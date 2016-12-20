using System;
using System.Configuration;
using Ansira;
using Ansira.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnsiraSDKTests
{
    [TestClass]
    public class AccessTokenTest
    {
        private string _clientId, _clientSecret;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            this._clientId = ConfigurationManager.AppSettings["Test.Client.Id"];
            this._clientSecret = ConfigurationManager.AppSettings["Test.Client.Secret"];

            Assert.IsNotNull(this._clientId);
            Assert.IsNotNull(this._clientSecret);
        }

        [TestMethod()]
        public void GetAccessToken()
        {
            ApiClient target = new ApiClient(_clientId, _clientSecret, true);

            string accessToken = target.GetAccessToken();
            Assert.IsNotNull(accessToken, "GetAccessToken returns token");
            Assert.IsInstanceOfType(target.AccessTokenExpires, DateTime.Now.GetType(), "Token expiry is set");
            Assert.IsTrue(target.AccessTokenExpires > DateTime.Now, "Token expiry is valid");
        }
    }
}
