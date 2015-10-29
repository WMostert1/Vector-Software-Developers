using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using DataAccess.Interface;
using Should;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;
using System.Collections.Generic;
using DataAccess.Models;

namespace BugBusiness.Tests
{
    [TestClass]
    public class FarmManagementTest
    {
        private AutoMock _autoMock;

        [TestInitialize]
        public void Setup()
        {
            
            AutoMapper.Mapper.CreateMap<Block, BlockFarmManDto>();
            _autoMock = AutoMock.GetStrict();
        }

        [TestCleanup]
        public void TearDown()
        {
            _autoMock.Dispose();
        }

        ////////////////
        /// AddBlock ///
        ////////////////

        [TestMethod]
        public void Unit_AddBlock_Business_ShouldAddBlock()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.InsertNewBlock(1, "Test1"))
                .Returns(true);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            AddBlockResponse addblockResult = farmManagement.AddBlock(new AddBlockRequest()
                {
                    FarmID = 1,
                    BlockName = "Test1"
                });

            //Assert
            addblockResult.ShouldNotBeNull();
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(BlockExistsException))]
        public void Unit_AddBlock_Business_ShouldThrowBlockExistsException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.InsertNewBlock(1, "TestBlock"))
                .Returns(false);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.AddBlock(new AddBlockRequest()
            {
                FarmID = 1,
                BlockName = "TestBlock"
            });

            //Assert - Expect BlockExistsException
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_AddBlock_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.InsertNewBlock(1, ""))
                .Returns(false);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.AddBlock(new AddBlockRequest()
            {
                FarmID = 1,
                BlockName = ""
            });

            //Assert - Expect InvalidInputException
        }

   

        ///////////////////////
        /// UpdateBlockByID ///
        ///////////////////////
 
        [TestMethod]
        public void Unit_UpdateBlockByID_Business_ShouldUpdateBlock()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.UpdateBlock(2, "UpdateTest"))
                .Returns(1);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            UpdateBlockByIDResponse updateblockbyidResult = farmManagement.UpdateBlockByID(new UpdateBlockByIDRequest()
                {
                    BlockID=2,
                    BlockName="UpdateTest"
                });
            //Assert
            updateblockbyidResult.ShouldNotBeNull();
        }

        [TestMethod]
        //[ExpectedExceptionAttribute(typeof(CouldNotUpdateException))]
        public void Unit_UpdateBlockByID_Business_ShouldThrowCouldNotUpdateException()
        {
            /*
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.UpdateBlock(Int64.MaxValue, "UpdateTest"))
                .Returns(0);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.UpdateBlockByID(new UpdateBlockByIDRequest()
            {
                BlockID = Int64.MaxValue,
                BlockName = "UpdateTest"
            });
            //Assert - Expect CouldNotUpdateException
              */
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_UpdateBlockByID_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.UpdateBlock(0, ""))
                .Returns(0);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.UpdateBlockByID(new UpdateBlockByIDRequest()
            {
                BlockID = 0,
                BlockName = ""
            });
            //Assert - Expect InvalidInputException
        }

        ///////////////////////
        /// DeleteBlockByID ///
        ///////////////////////

        [TestMethod]
        public void Unit_DeleteBlockByID_Business_ShouldDeleteBlock()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.DeleteBlock(2))
                .Returns(true);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            DeleteBlockByIDResponse deleteblockbyidResult = farmManagement.DeleteBlockByID(2);
            //Assert
            deleteblockbyidResult.ShouldNotBeNull();
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(CouldNotDeleteBlockException))]
        public void Unit_DeleteBlockByID_Business_ShouldThrowCouldNotDeleteException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.DeleteBlock(Int64.MaxValue))
                .Returns(false);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.DeleteBlockByID(Int64.MaxValue);
            //Assert - Expect CouldNotDeleteBlockException
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_DeleteBlockByID_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.DeleteBlock(0))
                .Returns(false);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.DeleteBlockByID(0);
            //Assert - Expect InvalidInputException
        }

        ////////////////////
        /// AddTreatment ///
        ////////////////////

        [TestMethod]
        public void Unit_AddTreatment_Business_ShouldAddTreatment()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.InsertNewTreatment(1, DateTime.Today, "Testing Treatment Addition"))
                .Returns(true);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            AddTreatmentResponse addTreatmentResult = farmManagement.AddTreatment(new AddTreatmentRequest()
            {
                BlockID = 1,
                TreatmentDate = DateTime.Today.ToString(),
                TreatmentComments="Testing Treatment Addition"
            });

            //Assert
            addTreatmentResult.ShouldNotBeNull();
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_AddTreatment_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.InsertNewTreatment(0, DateTime.Today,""))
                .Returns(false);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.AddTreatment(new AddTreatmentRequest()
            {
                BlockID = 0,
                TreatmentDate=DateTime.Today.ToString(),
                TreatmentComments = ""
            });

            //Assert - Expect InvalidInputException
        }
    }
}
