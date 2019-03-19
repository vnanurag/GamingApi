using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayStudiosApi.DataAccess.DBContext;
using PlayStudiosApi.DataAccess.Models;
using PlayStudiosApi.DataAccess.Repos;
using PlayStudiosApi.Services.Models;
using PlayStudiosApi.Services.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.Services.Tests.Services
{
    [TestClass]
    public class QuestServiceTests
    {
        private QuestService service;
        private Mock<IQuestRepository> repo = new Mock<IQuestRepository>();
        private Mock<ILogger> logger = new Mock<ILogger>();
        private Mock<IConfigurationSectionHandler> config = new Mock<IConfigurationSectionHandler>();
        private int RateFromBet;
        private int LevelBonusRate;
        private int TargetQuestPoints;
        private int MilestonesPerQuest;
        private int MilestonesReward;

        [TestInitialize]
        public void TestInitialize()
        {
            service = new QuestService(logger.Object, repo.Object);

            RateFromBet = Convert.ToInt32(ConfigurationManager.AppSettings["RateFromBet"]);
            LevelBonusRate = Convert.ToInt32(ConfigurationManager.AppSettings["LevelBonusRate"]);
            TargetQuestPoints = Convert.ToInt32(ConfigurationManager.AppSettings["TargetQuestPoints"]);
            MilestonesPerQuest = Convert.ToInt32(ConfigurationManager.AppSettings["MilestonesPerQuest"]);
            MilestonesReward = Convert.ToInt32(ConfigurationManager.AppSettings["MilestonesReward"]);
        }

        [TestMethod]
        public void GetQuestState_Success_Returns_QuestState()
        {
            // Arrange
            var playerId = "Player1";

            var quest1 = new Quest
            {
                Id = 1,
                PlayerId = "Player1",
                PlayerLevel = 3,
                ChipsAwarded = 200,
                QuestPointsEarned = 500,
                TotalQuestPercentCompleted = 50,
                LastMilestoneIndexCompleted = 2
            };

            repo
                .Setup(x => x.GetQuestState(playerId))
                .Returns(new QuestState
                {
                    TotalQuestPercentCompleted = quest1.TotalQuestPercentCompleted,
                    LastMilestoneIndexCompleted = quest1.LastMilestoneIndexCompleted
                });

            logger
                .Setup(x => x.Information("Logging quest state"));

            // Act
            var result = service.GetQuestState(playerId);

            // Assert
            Assert.AreEqual(quest1.TotalQuestPercentCompleted, result.TotalQuestPercentCompleted);
            Assert.AreEqual(quest1.LastMilestoneIndexCompleted, result.LastMilestoneIndexCompleted);
        }

        [TestMethod]
        public void GetQuestState_Success_Returns_Null()
        {
            // Arrange
            var playerId = "Player3";

            repo
                .Setup(x => x.GetQuestState(playerId))
                .Returns((QuestState)null);

            logger
                .Setup(x => x.Information("Logging quest state"));

            // Act
            var result = service.GetQuestState(playerId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetQuestState_Failure_Returns_Exception()
        {
            try
            {
                // Arrange
                var playerId = "Player4";

                repo
                    .Setup(x => x.GetQuestState(playerId))
                    .Throws(new Exception("Error"));

                logger
                    .Setup(x => x.Information("Logging quest state"));

                // Act
                var result = service.GetQuestState(playerId);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.AreEqual("Error", ex.Message);
            }
        }

        [TestMethod]
        public void GetQuestProgress_Success_Returns_QuestProgress()
        {
            // Arrange
            var playerInfo = new PlayerInfo
            {
                PlayerId = "NewPlayer",
                PlayerLevel = 2,
                ChipAmountBet = 30
            };

            var questDb = new Quest
            {
                PlayerId = playerInfo.PlayerId,
                PlayerLevel = playerInfo.PlayerLevel,
                QuestPointsEarned = 100,
                TotalQuestPercentCompleted = 25,
                LastMilestoneIndexCompleted = 2,
                ChipsAwarded = 30
            };

            repo
                .Setup(x => x.AddOrUpdateQuestState(It.IsAny<Quest>()))
                .Returns(questDb);

            logger
                .Setup(x => x.Information("Logging quest state"));

            // Act
            var result = service.GetQuestProgress(playerInfo);

            // Assert
            Assert.AreEqual(questDb.TotalQuestPercentCompleted, result.TotalQuestPercentCompleted);
            Assert.AreEqual(questDb.LastMilestoneIndexCompleted, result.MilestonesCompleted.MilestoneIndex);
            Assert.AreEqual(questDb.QuestPointsEarned, result.QuestPointsEarned);
        }
    }
}
