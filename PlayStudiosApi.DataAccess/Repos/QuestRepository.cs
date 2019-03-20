using PlayStudiosApi.DataAccess.DBContext;
using PlayStudiosApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.Repos
{
    public class QuestRepository : IQuestRepository
    {
        private readonly QuestDbContext _db;

        public QuestRepository(QuestDbContext db)
        {
            _db = db;
        }

        public QuestState GetQuestState(string playerId)
        {
            try
            {
                var result = _db
                                .Quests
                                .Where(x => x.PlayerId == playerId)
                                .Select(x => new QuestState
                                {
                                    TotalQuestPercentCompleted = x.TotalQuestPercentCompleted,
                                    LastMilestoneIndexCompleted = x.LastMilestoneIndexCompleted
                                })
                                .FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

                return result;                
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Quest AddOrUpdateQuestState(Quest quest)
        {
            try
            {
                var questFromDb = _db
                                    .Quests
                                    .Where(x => x.PlayerId == quest.PlayerId)
                                    .FirstOrDefault();

                if (questFromDb == null)
                {
                    // Adding a quest for the first time
                    _db.Quests.Add(quest);
                    _db.SaveChanges();
                    var response = GetUpdatedQuest(quest.PlayerId);
                    return response;
                }
                else
                {
                    // Check if the milestone has already been achieved
                    var milestoneAchieved = IsMilestoneCompleted(quest.PlayerId, quest.LastMilestoneIndexCompleted);
                    if (milestoneAchieved)
                    {
                        throw new Exception("This milestone has been completed. You can't complete a milestone more than once");
                    }

                    // Update the quest in DB with new information
                    questFromDb.PlayerLevel = quest.PlayerLevel;
                    questFromDb.QuestPointsEarned = quest.QuestPointsEarned;
                    questFromDb.TotalQuestPercentCompleted = quest.TotalQuestPercentCompleted;
                    questFromDb.LastMilestoneIndexCompleted = quest.LastMilestoneIndexCompleted;
                    questFromDb.ChipsAwarded = quest.ChipsAwarded;

                    _db.SaveChanges();

                    var response = GetUpdatedQuest(quest.PlayerId);
                    return response;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the updated information after add or update.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual Quest GetUpdatedQuest(string playerId)
        {
            try
            {
                return _db
                        .Quests
                        .Where(x => x.PlayerId == playerId)
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Checks if the given milestone is already completed
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="milestone"></param>
        /// <returns></returns>
        public bool IsMilestoneCompleted(string playerId, int milestone)
        {
            try
            {
                return _db
                        .Quests
                        .Any(x => x.PlayerId == playerId
                            && x.LastMilestoneIndexCompleted >= milestone);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
