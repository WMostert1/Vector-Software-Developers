using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using Autofac.Extras.Moq;
using BugBusiness.Interface.BugReporting.DTO;
using DataAccess.Interface;
using DataAccess.Models;

namespace BugBusiness.Tests
{
    [TestClass]
    public class BugReportingTest
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
        public void Unit_GetCapturedData_Business_ShouldNotAcquire()
        {
            //Arrange
            _autoMock
                .Mock<IDbBugReporting>()
                .Setup(rpt => rpt.GetScoutStopsByFarmId(12345))
                .Returns( new List<ScoutStop>());

            _autoMock
               .Mock<IDbBugReporting>()
               .Setup(rpt => rpt.GetTreatmentsByFarmId(12345))
               .Returns( new List<Treatment>());
                
            var bugReporting = _autoMock.Create<BugReporting.BugReporting>();

            //Act
            var test = bugReporting.GetCapturedData(new GetCapturedDataRequest(){FarmId = 12345});

            //Assert
            test.ShouldBeNull();
        }
    }
}
