using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Should;
using Autofac.Extras.Moq;
using DataAccess.Interface;
using DataAccess.Interface.Domain;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;

namespace BugBusiness.Tests
{
    [TestClass]
    public class BugSecurityTest
    {
        private AutoMock _autoMock;

        [TestInitialize]
        public void Setup()
        {
            _autoMock = AutoMock.GetStrict();
        }

        [TestCleanup]
        public void TearDown()
        {
            _autoMock.Dispose();
        }

        [TestMethod]
        public void Unit_Login_Business_ShouldAuthenticate()
        {
            //Arrange
            _autoMock
                .Mock<IDbAuthentication>()
                .Setup(dbAuthentication => dbAuthentication.GetUserByCredentials("Test1", "321"))
                .Returns(new User()
                {
                    Id = 1,
                    Roles = new List<Role>()
                    {
                        new Role(){ Description = "Admin", Type = 1},
                        new Role(){Description = "Grower", Type = 2}
                    }
                });

            var bugSecurity = _autoMock.Create<BugSecurity.BugSecurity>();

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
            _autoMock
                .Mock<IDbAuthentication>()
                .Setup(dbAuthentication => dbAuthentication.GetUserByCredentials("Test2", "123"))
                .Returns((User)null);
            
            var bugSecurity = _autoMock.Create<BugSecurity.BugSecurity>();

            //Act
            bugSecurity.Login(new LoginRequest()
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
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Unit_Register_Business_ShouldNotAddUser()
        {
            //Arrange
            //Act
            //Assert
            throw new NotImplementedException();
        }
    }
}
