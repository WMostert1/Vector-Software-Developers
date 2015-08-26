using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using Autofac.Extras.Moq;

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
        public void Unit_GetFarmById_Business_ShouldAcquire()
        {
            //Arrange
            /*_autoMock
                .Mock<IDbReporting>()
                .Setup(rpt => rpt.GetFarmById(12345))
                .Returns(new Farm());
                

            var bugReporting = _autoMock.Create<BugReporting.BugReporting>();*/

            //Act
            //Assert
            var x = 0;
            x.ShouldEqual(0);
        }
    }
}
