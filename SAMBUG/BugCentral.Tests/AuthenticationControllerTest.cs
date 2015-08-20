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
        public void Inter_Login_API_ShouldNotAuthenticate()
        {
            //Arrange
            //Act
            //Assert
            throw new NotImplementedException();
        }
    }
}
