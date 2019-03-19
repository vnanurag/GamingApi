using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayStudiosApi.Controllers;
using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Services.Models;
using PlayStudiosApi.Services.Services;
using PlayStudiosApi.Services.Services.Interfaces;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace PlayStudiosApi.Tests.Controllers
{
    [TestClass]
    public class QuestControllerTest
    {
        private QuestController controller;
        private Mock<IQuestService> questService = new Mock<IQuestService>();
        private Mock<ILogger> logger = new Mock<ILogger>();

        [TestInitialize]
        public void TestInitialize()
        {
            controller = new QuestController(questService.Object, logger.Object);
        }

        [TestMethod]
        public void GetQuestProgress_Success_Returns_QuestProgress()
        {
            // Arrange
            var playerInfo = new PlayerInfo
            {
                PlayerId = "Anurag",
                PlayerLevel = 3,
                ChipAmountBet = 100
            };

            var output = new QuestProgress
            {
                QuestPointsEarned = 10600,
                TotalQuestPercentCompleted = 21,
                MilestonesCompleted = new MilestoneInfo
                {
                    MilestoneIndex = 2,
                    ChipsAwarded = 4000
                }
            };

            questService
                .Setup(x => x.GetQuestProgress(playerInfo))
                .Returns(output);

            // Act
            IHttpActionResult result = controller.GetQuestProgress(playerInfo);
            var contentResult = result as OkNegotiatedContentResult<QuestProgress>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<QuestProgress>));
            Assert.AreEqual(output.QuestPointsEarned, contentResult.Content.QuestPointsEarned);
            Assert.AreEqual(output.TotalQuestPercentCompleted, contentResult.Content.TotalQuestPercentCompleted);
            Assert.AreEqual(output.MilestonesCompleted.MilestoneIndex, contentResult.Content.MilestonesCompleted.MilestoneIndex);
            Assert.AreEqual(output.MilestonesCompleted.ChipsAwarded, contentResult.Content.MilestonesCompleted.ChipsAwarded);
        }

        [TestMethod]
        public void GetQuestProgress_Failure_BlankPlayerId_Returns_BadRequest()
        {
            // Arrange
            var playerInfo = new PlayerInfo
            {
                PlayerId = "",
                PlayerLevel = 2,
                ChipAmountBet = 100
            };

            logger
                .Setup(x => x.Error("Bad Request"));

            var output = "Please provide valid player information to see the quest progress.";

            // Act
            IHttpActionResult result = controller.GetQuestProgress(playerInfo); 
            var contentResult = result as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(output, contentResult.Message);
        }

        [TestMethod]
        public void GetQuestProgress_Failure_NullPlayerId_Returns_BadRequest()
        {
            // Arrange
            var playerInfo = new PlayerInfo
            {
                PlayerId = null,
                PlayerLevel = 2,
                ChipAmountBet = 100
            };

            logger
                .Setup(x => x.Error("Bad Request"));

            var output = "Please provide valid player information to see the quest progress.";

            // Act
            IHttpActionResult result = controller.GetQuestProgress(playerInfo);
            var contentResult = result as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(output, contentResult.Message);
        }

        [TestMethod]
        public void GetQuestProgress_Failure_Returns_Exception()
        {
            // Arrange
            var playerInfo = new PlayerInfo
            {
                PlayerId = "Anurag",
                PlayerLevel = 2,
                ChipAmountBet = 100
            };

            questService
                .Setup(x => x.GetQuestProgress(playerInfo))
                .Throws(new Exception("Error"));

            logger
                .Setup(x => x.Error("Internal Server Error"));

            // Act
            IHttpActionResult result = controller.GetQuestProgress(playerInfo);
            var contentResult = result as ExceptionResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ExceptionResult));
            Assert.AreEqual("Error", contentResult.Exception.Message);
        }

        [TestMethod]
        public void GetQuestState_Success_Returns_QuestState()
        {
            // Arrange
            var playerId = "Anurag";

            var output = new QuestState
            {
                TotalQuestPercentCompleted = 20,
                LastMilestoneIndexCompleted = 2
            };

            questService
                .Setup(x => x.GetQuestState(playerId))
                .Returns(output);

            // Act
            IHttpActionResult result = controller.GetQuestState(playerId);
            var contentResult = result as OkNegotiatedContentResult<QuestState>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<QuestState>));
            Assert.AreEqual(output.TotalQuestPercentCompleted, contentResult.Content.TotalQuestPercentCompleted);
            Assert.AreEqual(output.LastMilestoneIndexCompleted, contentResult.Content.LastMilestoneIndexCompleted);
        }

        [TestMethod]
        public void GetQuestState_Success_Returns_NotFound_If_PlayerDoesNotExist()
        {
            // Arrange
            var playerId = "DoesNotExist";

            questService
                .Setup(x => x.GetQuestState(playerId))
                .Returns((QuestState)null);

            // Act
            IHttpActionResult result = controller.GetQuestState(playerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetQuestState_Failure_InvalidPlayerId_Returns_BadRequest()
        {
            // Arrange
            var playerId = "";

            logger
                .Setup(x => x.Error("Bad Request"));

            var output = "Player Id is not valid.";

            // Act
            IHttpActionResult result = controller.GetQuestState(playerId);
            var contentResult = result as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(output, contentResult.Message);
        }

        [TestMethod]
        public void GetQuestState_Failure_Returns_Exception()
        {
            // Arrange
            var playerId = "Anurag";

            questService
                .Setup(x => x.GetQuestState(playerId))
                .Throws(new Exception("Error"));

            logger
                .Setup(x => x.Error("Internal Server Error"));

            // Act
            IHttpActionResult result = controller.GetQuestState(playerId);
            var contentResult = result as ExceptionResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ExceptionResult));
            Assert.AreEqual("Error", contentResult.Exception.Message);
        }
    }
}
