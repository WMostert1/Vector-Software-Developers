using System;
using System.Web.Http;
using System.Web.UI.WebControls;
using BugCentral.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using Should;

namespace BugCentral.Tests
{
    [TestClass]
    public class AuthenticationControllerTest
    {
        [TestMethod]
        public void CentralLoginTest_ShouldNotAuthenticate()
        {
            //Arrange
            var loginRequest = new LoginRequest()
            {
                Username = "email1",
                Password = "123"
            };

       /*     IDbAuthentication dbAuth = new DbAuthentication();

            var x = dbAuth.GetUserByCredentials(loginRequest.Username, loginRequest.Password);

       */

            /*var autoMock = AutoMock.GetStrict();
            autoMock
                .Mock<IDbAuthentication>()
                .Setup(dbAuthentication => dbAuthentication.GetUserIdRoles(loginRequest))
                .Returns(new LoginResponse(){Id = default(int), Roles = null});

            var controller = autoMock.Create<AuthenticationController>();

            //Act
            var response = controller.Login(loginRequest);

            //Assert
            response.Id.ShouldEqual(0);
            response.Roles.ShouldBeNull();*/

        }
    }
}
