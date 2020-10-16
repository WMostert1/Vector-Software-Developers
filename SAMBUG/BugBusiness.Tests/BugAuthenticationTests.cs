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
using BugBusiness.Interface.BugSecurity.DTO;


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

    }
}
