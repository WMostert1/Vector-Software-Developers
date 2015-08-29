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
        public void Unit_GetScoutStopsByFarmId_Business_ShouldAcquire()
        {
            //Arrange
            _autoMock
                .Mock<IDbBugReporting>()
                .Setup(rpt => rpt.GetScoutStopsByFarmId(12345))
                .Returns(new List<ScoutStop>()
                {
                    new ScoutStop()
                    {
                        ScoutStopID = 1,
                        UserID = 1,
                        BlockID = 1,
                        NumberOfTrees = 10,
                        Latitude = 100L,
                        Longitude = 100L,
                        Date = DateTime.Now,
                        Block = new Block(){
                            BlockID = 1,
                            FarmID = 1,
                            BlockName = "Piesang"
                        },
                        ScoutBugs = new List<ScoutBug>()
                        {
                            new ScoutBug()
                            {
                                ScoutBugID = 1,
                                ScoutStopID = 1,
                                SpeciesID = 1,
                                NumberOfBugs = 30,
                                FieldPicture = new byte[]{1,2,3},
                                Comments = "This is a comment on bug collection 1",
                                Species = new Species()
                                {
                                    SpeciesID = 1,
                                    SpeciesName = "Yellow Edged Bug",
                                    Lifestage = 1,
                                    IdealPicture = new byte[]{3,2,1},
                                    IsPest = true
                                }
                            },
                            new ScoutBug()
                            {
                                ScoutBugID = 2,
                                ScoutStopID = 1,
                                SpeciesID = 1,
                                NumberOfBugs = 35,
                                FieldPicture = new byte[]{1,2,3},
                                Comments = "This is a comment on bug collection 2",
                                Species = new Species()
                                {
                                    SpeciesID = 1,
                                    SpeciesName = "Yellow Edged Bug",
                                    Lifestage = 1,
                                    IdealPicture = new byte[]{3,2,1},
                                    IsPest = true
                                }
                            }
                        }
                    }
                });
                

            var bugReporting = _autoMock.Create<BugReporting.BugReporting>();

            //Act
            var test = bugReporting.GetCapturedData(new GetCapturedDataRequest(){FarmId = 12345});
            //Assert
        }
    }
}
