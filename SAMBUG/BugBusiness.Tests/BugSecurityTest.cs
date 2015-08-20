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
            _autoMock = AutoMock.GetLoose();
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
                    UserId = 1,
                    Roles = new List<Role>()
                    {
                        new Role(){Description = "Admin", Type = 1},
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
            loginResponse.User.Id.ShouldEqual(1);
            loginResponse.User.Roles[0].Type.ShouldEqual(1);
            loginResponse.User.Roles[1].Type.ShouldEqual(2);

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
            _autoMock
               .Mock<IDbAuthentication>()
               .Setup(dbAuthentication => dbAuthentication.InsertNewUser("TestEmail@TestHost.com", "321", "Farm1"))
               .Returns(true);

            var bugSecurity = _autoMock.Create<BugSecurity.BugSecurity>();
           
            //Act
            RegisterResponse registerResponse = bugSecurity.Register(new RegisterRequest()
            {
                Username = "TestEmail@TestHost.com",
                UsernameConfirmation = "TestEmail@TestHost.com",
                Password = "321",
                PasswordConfirmation = "321",
                FarmName = "Farm1"
            });

            //Assert - Expect No Service Exceptions
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(UserExistsException))]
        public void Unit_Register_Business_ShouldNotAddUser()
        {
            //Arrange
            _autoMock
               .Mock<IDbAuthentication>()
               .Setup(dbAuthentication => dbAuthentication.InsertNewUser("TestEmail@TestHost.com", "54321", "Farm1"))
               .Returns(false);

            var bugSecurity = _autoMock.Create<BugSecurity.BugSecurity>();

            //Act
            RegisterResponse registerResponse = bugSecurity.Register(new RegisterRequest()
            {
                Username = "TestEmail@TestHost.com",
                UsernameConfirmation = "TestEmail@TestHost.com",
                Password = "54321",
                PasswordConfirmation = "54321",
                FarmName = "Farm1"
            });

            //Assert - Expect UserExistsException
        }

        [TestMethod]
        public void Unit_Register_Business_ShouldInvalidateEmailAddresses()
        {
            //Arrange
            _autoMock
               .Mock<IDbAuthentication>().SetReturnsDefault(true);

            var bugSecurity = _autoMock.Create<BugSecurity.BugSecurity>();

            string[] invalidEmails =
            {
                "plainaddress",
                "#@%^%#$@#$@#.com",
                "@example.com",
                "email.example.com",
                "email@example@example.com",
                ".email@example.com"
            };

            var invalidEmailCount = 0;

            //Act
            foreach (var email in invalidEmails)
            {
                try
                {
                    bugSecurity.Register(new RegisterRequest()
                    {
                        Username = email,
                        UsernameConfirmation = email,
                        Password = "54321",
                        PasswordConfirmation = "54321",
                        FarmName = "Farm1"
                    });
                }
                catch (InvalidInputException)
                {
                    ++invalidEmailCount;
                }
            }
           
            //Assert
            invalidEmailCount.ShouldEqual(invalidEmails.Length);
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_Register_Business_ShouldInvalidateConfirmations()
        {
            //Arrange
            _autoMock
               .Mock<IDbAuthentication>()
               .Setup(dbAuthentication => dbAuthentication.InsertNewUser("TestEmail@TestHost.com", "54321", "Farm1"))
               .Returns(true);

            var bugSecurity = _autoMock.Create<BugSecurity.BugSecurity>();

            //Act
            RegisterResponse registerResponse = bugSecurity.Register(new RegisterRequest()
            {
                Username = "TestEmail@TestHost.com",
                UsernameConfirmation = "different@differentHost.com",
                Password = "54321",
                PasswordConfirmation = "54321",
                FarmName = "Farm1"
            });

            //Assert - Expect InvalidInputException
        }
    }
}
