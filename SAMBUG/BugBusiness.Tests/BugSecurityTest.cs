using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Should;
using Autofac.Extras.Moq;
using BugBusiness.Interface.BugSecurity;
using DataAccess.Interface;
using Role = DataAccess.Interface.Domain.Role;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using DataAccess.Interface.Domain;

namespace BugBusiness.Tests
{
    [TestClass]
    public class BugSecurityTest
    {

        [TestMethod]
        public void Unit_Login_Business_ShouldAuthenticate()
        {
            //Arrange
            var autoMock = AutoMock.GetStrict();
            
            autoMock
                .Mock<IDbAuthentication>()
                .Setup(dbAuthentication => dbAuthentication.GetUserByCredentials("Test1", "321"))
                .Returns(new DataAccess.Interface.Domain.User()
                {
                    Id = 1,
                    Roles = new List<Role>()
                    {
                        new Role(){ Description = "Admin", Type = 1},
                        new Role(){Description = "Grower", Type = 2}
                    }
                });

            var bugSecurity = autoMock.Create<BugSecurity.BugSecurity>();

            //Act
            LoginResponse loginResponse = bugSecurity.Login(new LoginRequest()
            {
                Username = "Test1",
                Password = "321"
            });

            //Assert
            loginResponse.Id.ShouldEqual(1);
            loginResponse.Roles[0].Type.ShouldEqual(1);
            loginResponse.Roles[1].Type.ShouldEqual(2);

        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(NotRegisteredException))]
        public void Unit_Login_Business_ShouldNotAuthenticate()
        {
            //Arrange
            var autoMock = AutoMock.GetStrict();

            autoMock
                .Mock<IDbAuthentication>()
                .Setup(dbAuthentication => dbAuthentication.GetUserByCredentials("Test2", "123"))
                .Returns((DataAccess.Interface.Domain.User)null);
            
            var bugSecurity = autoMock.Create<BugSecurity.BugSecurity>();

            //Act
            LoginResponse loginResponse = bugSecurity.Login(new LoginRequest()
            {
                Username = "Test2",
                Password = "123"
            });

            //Assert - Expect NotRegisteredException
        }

        [TestMethod]
        public void Unit_Register_Business_ShouldAddUser()
        {
            //Arrange
            //Act
            //Assert
        }

        [TestMethod]
        public void Unit_Register_Business_ShouldNotAddUser()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
