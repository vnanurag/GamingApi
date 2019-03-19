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
    }
}
