using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using DataAccess.Interface;
using Should;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;
using DataAccess.Interface.Domain;
using System.Collections.Generic;

namespace BugBusiness.Tests
{
    [TestClass]
    public class FarmManagementTest
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

        ///////////////////////////
        /// AddBlock Unit Tests ///
        ///////////////////////////

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
            AddBlockResult addblockResult = farmManagement.AddBlock(new AddBlockRequest()
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
        /// GetBlocksByFarm ///
        ///////////////////////

        [TestMethod]
        public void Unit_GetBlocksByFarm_Business_ShouldGetBlocks()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.GetBlocksByFarm(1))
                .Returns(new List<Block>());

            var farmManagement=_autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            GetBlocksByFarmResult getblocksbyfarmResult = farmManagement.GetBlocksByFarm(new GetBlocksByFarmRequest()
                {
                    FarmID = 1
                });
            //Assert
            getblocksbyfarmResult.Blocks.ShouldNotBeNull();
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(NoBlocksException))]
        public void Unit_GetBlocksByFarm_Business_ShouldThrowNoBlocksException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.GetBlocksByFarm(Int64.MaxValue))
                .Returns((List<Block>)null);

            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.GetBlocksByFarm(new GetBlocksByFarmRequest()
            {
                FarmID = Int64.MaxValue
            });
            //Assert - Expect InvalidInputException
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_GetBlocksByFarm_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.GetBlocksByFarm(0))
                .Returns((List<Block>)null);

            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.GetBlocksByFarm(new GetBlocksByFarmRequest()
            {
                FarmID = 0
            });
            //Assert - Expect InvalidInputException
        }

        ////////////////////
        /// GetBlockByID ///
        ////////////////////

        [TestMethod]
        public void Unit_GetBlockByID_Business_ShouldGetBlock()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.GetBlockByID(2))
                .Returns(new Block()
                {
                    BlockID=2,
                    BlockName="TestBlock"
                });
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            GetBlockByIDResult getblockbyidResult = farmManagement.GetBlockByID(new GetBlockByIDRequest()
                {
                    BlockID=2
                });

            //Assert
            getblockbyidResult.Block.BlockID.ShouldEqual(2);
            getblockbyidResult.Block.BlockName.ShouldEqual("TestBlock");
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(NoSuchBlockExistsException))]
        public void Unit_GetBlockByID_Business_ShouldThrowNoSuchBlockException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.GetBlockByID(Int64.MaxValue))
                .Returns((Block)null);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            GetBlockByIDResult getblockbyidResult = farmManagement.GetBlockByID(new GetBlockByIDRequest()
            {
                BlockID = Int64.MaxValue
            });

            //Assert - Expect NoSuchBlockExistsException
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_GetBlockByID_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.GetBlockByID(0))
                .Returns((Block)null);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            GetBlockByIDResult getblockbyidResult = farmManagement.GetBlockByID(new GetBlockByIDRequest()
            {
                BlockID = 0
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
                .Returns(true);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            UpdateBlockByIDResult updateblockbyidResult = farmManagement.UpdateBlockByID(new UpdateBlockByIDRequest()
                {
                    BlockID=2,
                    BlockName="UpdateTest"
                });
            //Assert
            updateblockbyidResult.ShouldNotBeNull();
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(CouldNotUpdateException))]
        public void Unit_UpdateBlockByID_Business_ShouldThrowCouldNotUpdateException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.UpdateBlock(Int64.MaxValue, "UpdateTest"))
                .Returns(false);
            var farmManagement = _autoMock.Create<FarmManagement.FarmManagement>();
            //Act
            farmManagement.UpdateBlockByID(new UpdateBlockByIDRequest()
            {
                BlockID = Int64.MaxValue,
                BlockName = "UpdateTest"
            });
            //Assert - Expect CouldNotUpdateException
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidInputException))]
        public void Unit_UpdateBlockByID_Business_ShouldThrowInvalidInputException()
        {
            //Arrange
            _autoMock
                .Mock<IDbFarmManagement>()
                .Setup(dbFarmManagement => dbFarmManagement.UpdateBlock(0, ""))
                .Returns(false);
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
            DeleteBlockByIDResult deleteblockbyidResult = farmManagement.DeleteBlockByID(new DeleteBlockByIDRequest()
                {
                    BlockID = 2
                });
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
            DeleteBlockByIDResult deleteblockbyidResult = farmManagement.DeleteBlockByID(new DeleteBlockByIDRequest()
            {
                BlockID = Int64.MaxValue
            });
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
            DeleteBlockByIDResult deleteblockbyidResult = farmManagement.DeleteBlockByID(new DeleteBlockByIDRequest()
            {
                BlockID = 0
            });
            //Assert - Expect InvalidInputException
        }
    }
}
