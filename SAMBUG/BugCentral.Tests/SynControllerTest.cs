using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugBusiness.Interface.BugScouting;
using BugBusiness.Interface.BugScouting.DTO;
using BugBusiness.Interface.BugScouting.Exceptions;
using Autofac.Extras.Moq;
using BugCentral.Controllers;
using BugCentral.Controllers.DTO;
using Should;

namespace BugCentral.Tests
{
    [TestClass]
    public class SynControllerTest
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
        public void Unit_Syn_Should_Pass()
        {
            var SynchronizationController = _autoMock.Create<SynchronizationController>();
            SynchronizationController.ReadInJson("Test.json");
            
            SynResult persistScoutStopsResult = SynchronizationController.sync();

            persistScoutStopsResult.Passed.ShouldEqual(true);
        }
        [TestMethod]
        public void Unit_Syn_Should_Fail()
        {
            var SynchronizationController = _autoMock.Create<SynchronizationController>();
            SynResult persistScoutStopsResult = new SynResult();
            try
            {
                SynchronizationController.ReadInJson(@"C:\Usesfrs\Kafsflsfsdfse-ab\Ddsdsfsdfocuments\GitHub\Vector-Software-Developers\Test.json");
                persistScoutStopsResult = SynchronizationController.sync();
            }
            catch (Exception)
            {
                //Do nothing
            }

            persistScoutStopsResult.Passed.ShouldBeFalse();
        }
    }
}
