using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayStudiosApi.DataAccess.DBContext;
using PlayStudiosApi.DataAccess.Models;
using PlayStudiosApi.DataAccess.Repos;

namespace PlayStudiosApi.DataAccess.Tests.Repos
{
    [TestClass]
    public class QuestRepositoryTests
    {
        private QuestRepository repo;
        private Mock<QuestDbContext> db = new Mock<QuestDbContext>();
        private Mock<DbSet<Quest>> quests = new Mock<DbSet<Quest>>();
        private List<Quest> questsList = new List<Quest>();

        [TestInitialize]
        public void TestInitialize()
        {
            repo = new QuestRepository(db.Object);

            // Setting up mock DB data
            var quest1 = new Quest
            {
                Id = 1,
                PlayerId = "Player1",
                PlayerLevel = 2,
                ChipsAwarded = 20,
                QuestPointsEarned = 200,
                TotalQuestPercentCompleted = 30,
                LastMilestoneIndexCompleted = 1
            };

            var quest2 = new Quest
            {
                Id = 2,
                PlayerId = "Player2",
                PlayerLevel = 3,
                ChipsAwarded = 30,
                QuestPointsEarned = 300,
                TotalQuestPercentCompleted = 50,
                LastMilestoneIndexCompleted = 2
            };

            questsList.Add(quest1);
            questsList.Add(quest2);
            var data = questsList.AsQueryable();
            
            quests.As<IQueryable<Quest>>().Setup(m => m.Provider).Returns(data.Provider);
            quests.As<IQueryable<Quest>>().Setup(m => m.Expression).Returns(data.Expression);
            quests.As<IQueryable<Quest>>().Setup(m => m.ElementType).Returns(data.ElementType);
            quests.As<IQueryable<Quest>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            db.Setup(x => x.Quests).Returns(quests.Object);
        }

        [TestMethod]
        public void GetQuestState_Success_Returns_QuestState()
        {
            // Arrange
            var playerId = "Player1";

            // Act
            var result = repo.GetQuestState(playerId);

            // Assert
            Assert.AreEqual(30, result.TotalQuestPercentCompleted);
            Assert.AreEqual(1, result.LastMilestoneIndexCompleted);
        }

        [TestMethod]
        public void GetQuestState_Success_Returns_Null_If_PlayerNotFound()
        {
            // Arrange
            var playerId = "PlayerDoesNotExist";

            // Act
            var result = repo.GetQuestState(playerId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetQuestState_Failure_Throws_Exception()
        {
            try
            {
                // Arrange
                var playerId = "PlayerException";

                db
                .Setup(x => x.Quests)
                .Throws(new Exception("Error"));

                // Act
                var result = repo.GetQuestState(playerId);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.AreEqual("Error", ex.Message);
            }
        }

        [TestMethod]
        public void AddOrUpdateQuestState_Success_UpdatesDb_Returns_QuestFromDb()
        {
            // Arrange
            var quest1 = new Quest
            {
                Id = 1,
                PlayerId = "Player1",
                PlayerLevel = 2,
                ChipsAwarded = 20,
                QuestPointsEarned = 200,
                TotalQuestPercentCompleted = 30,
                LastMilestoneIndexCompleted = 1
            };

            var dbRequest = new Quest
            {
                Id = 1,
                PlayerId = "Player1",
                PlayerLevel = 5,
                ChipsAwarded = 70,
                QuestPointsEarned = 800,
                TotalQuestPercentCompleted = 80,
                LastMilestoneIndexCompleted = 5
            };

            // Act
            var result = repo.AddOrUpdateQuestState(dbRequest);

            // Assert
            Assert.AreEqual(quest1.PlayerId, result.PlayerId);
            Assert.AreEqual(dbRequest.PlayerLevel, result.PlayerLevel);
            Assert.AreEqual(dbRequest.ChipsAwarded, result.ChipsAwarded);
            Assert.AreEqual(dbRequest.QuestPointsEarned, result.QuestPointsEarned);
            Assert.AreEqual(dbRequest.TotalQuestPercentCompleted, result.TotalQuestPercentCompleted);
            Assert.AreEqual(dbRequest.LastMilestoneIndexCompleted, result.LastMilestoneIndexCompleted);
        }

        [TestMethod]
        public void AddOrUpdateQuestState_Failure_MilestoneAlreadyCompleted_Throws_Exception()
        {
            try
            {
                // Arrange
                var dbRequest = new Quest
                {
                    Id = 1,
                    PlayerId = "Player1",
                    PlayerLevel = 4,
                    ChipsAwarded = 40,
                    QuestPointsEarned = 400,
                    TotalQuestPercentCompleted = 60,
                    LastMilestoneIndexCompleted = 1
                };
                questsList.Add(dbRequest);

                db.Setup(x => x.Quests).Returns(quests.Object);
                db.Setup(x => x.SaveChanges());

                // Act
                var result = repo.AddOrUpdateQuestState(dbRequest);
            }
            catch (Exception ex)
            {
                var output = "This milestone has been completed. You can't complete a milestone more than once";

                // Assert
                Assert.AreEqual(output, ex.Message);
            }
        }

        [TestMethod]
        public void AddOrUpdateQuestState_Failure_Throws_Exception()
        {
            try
            {
                // Arrange
                var dbRequest = new Quest
                {
                    Id = 6,
                    PlayerId = "PlayerError",
                    PlayerLevel = 4,
                    ChipsAwarded = 40,
                    QuestPointsEarned = 400,
                    TotalQuestPercentCompleted = 60,
                    LastMilestoneIndexCompleted = 1
                };

                db
                .Setup(x => x.Quests)
                .Throws(new Exception("Error"));

                // Act
                var result = repo.AddOrUpdateQuestState(dbRequest);
            }
            catch (Exception ex)
            {
                
                // Assert
                Assert.AreEqual("Error", ex.Message);
            }
        }
    }
}
