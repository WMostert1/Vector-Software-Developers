using System;
using System.Web.Http;
using System.Web.UI.WebControls;
using BugCentral.Controllers;
using DataAccess.Interface.DTOModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using DataAccess.Interface;
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
                Username = "michelle@gmail.com",
                Password = "321"
            };

            var autoMock = AutoMock.GetStrict();
            autoMock.Mock<IDbAuthentication>()
                .Setup(dbAuthentication => dbAuthentication.GetUserIdRoles(loginRequest))
                .Returns(new LoginResponse(){Id = default(int), Roles = null});

            var controller = autoMock.Create<AuthenticationController>();

            //Act
            var response = controller.Login(loginRequest);

            //Assert
            response.Id.ShouldEqual(0);
            response.Roles.ShouldBeNull();

        }
    }
}
