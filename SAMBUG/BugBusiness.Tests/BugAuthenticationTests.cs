using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using BugBusiness.Interface.BugAuthentication;
using BugCentral.HelperClass;
using BugBusiness.Interface.BugAuthentication.DTO;
using Should;
using BugBusiness.Interface.BugSecurity;
using DataAccess.Interface;
using BugBusiness.Interface.BugAuthentication.Exceptions;


namespace BugBusiness.Tests
{
    [TestClass]
    public class UnitTest1
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
        public void Unit_Pass_Recover_Account()
        {
          /*  var bugAuthentication = _autoMock.Create<BugAuthentication.BugAuthentication>();
       
            RecoverAccountResult recoverAccountResult = bugAuthentication.RecoverAccount(new RecoverAccountRequest()
            {
                From = "kaleabtessera@gmail.com",
                FromPassword = "27ATEHBruKal1129",
                EmailTo = "kaleabtessera@gmail.com",
                Link = "http://localhost:53249/Home/ChangePassword"

            });

            recoverAccountResult.ShouldNotBeNull();*/
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(FailedEmailSendException))]
        public void Unit_Fail_Recover_Account()
        {
            var bugAuthentication = _autoMock.Create<BugAuthentication.BugAuthentication>();
 
            RecoverAccountResult recoverAccountResult = bugAuthentication.RecoverAccount(new RecoverAccountRequest()
            {
                From = "kaleabdsfsfsdftessera@gmail.com",
                FromPassword = "27ATEasdasdHBruKal1129",
                EmailTo = "kaleabtessera@gmail.com",
                Link = "http://localhost:53249/Home/ChangePassword"

            });

            
        }

        [TestMethod]
        public void Unit_Pass_Change_Password()
        {
            /*
            var bugAuthentication = _autoMock.Create<BugAuthentication.BugAuthentication>();
            //Arrange
            _autoMock
                .Mock<IDbBugSecurity>()
                .Setup(_bugSecurity => _bugSecurity.ChangeUserPassword("kaleabtessera@gmail.com", "newPassFromTest"))
                .Returns(true);


            ChangePasswordResult changePasswordResult = bugAuthentication.ChangePassword(new ChangePasswordRequest()
            {
                Email = "kaleabtessera@gmail.com",
                Password = "newPassFromTest"

            });

            changePasswordResult.ShouldNotBeNull();
             */
        }

        [TestMethod]
        public void Unit_Fail_Change_Password()
        {
            /*
            var bugAuthentication = _autoMock.Create<BugAuthentication.BugAuthentication>();
            //Arrange
            _autoMock
                .Mock<IDbBugSecurity>()
                .Setup(_bugSecurity => _bugSecurity.ChangeUserPassword("kaleabtessera@gmail.com", "newPassFromTest"))
                .Returns(false);


            ChangePasswordResult changePasswordResult = bugAuthentication.ChangePassword(new ChangePasswordRequest()
            {
                Email = "kaleabtessera@gmail.com",
                Password = "newPassFromTest"

            });

            changePasswordResult.ShouldBeNull();
              */
        }

    }
}
