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

                //using (var db = new QuestDbContext())
                //{
                //    var result2 = db
                //                   .Quests
                //                   .Where(x => x.PlayerId == playerId)
                //                   .Select(x => new QuestState
                //                   {
                //                       TotalQuestPercentCompleted = x.TotalQuestPercentCompleted,
                //                       LastMilestoneIndexCompleted = x.LastMilestoneIndexCompleted
                //                   })
                //                   .FirstOrDefault();

                //    if (result == null)
                //    {
                //        return null;
                //    }

                //    return result;
                //}                
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
                //using (var db = new QuestDbContext())
                //{
                //    var questFromDb = db
                //                        .Quests
                //                        .Where(x => x.PlayerId == quest.PlayerId)
                //                        .FirstOrDefault();

                //    if (questFromDb == null)
                //    {
                //        // Adding a quest for the first time
                //        db.Quests.Add(quest);
                //        db.SaveChanges();

                //        var response = GetUpdatedQuest(quest.PlayerId);
                //        return response;
                //    }
                //    else
                //    {
                //        // Check if the milestone has already been achieved
                //        var milestoneAchieved = IsMilestoneCompleted(quest.PlayerId, quest.LastMilestoneIndexCompleted);
                //        if (milestoneAchieved)
                //        {
                //            throw new Exception("This milestone has been completed. You can't complete a milestone more than once");
                //        }

                //        // Update the quest in DB with new information
                //        questFromDb.PlayerLevel = quest.PlayerLevel;
                //        questFromDb.QuestPointsEarned = quest.QuestPointsEarned;
                //        questFromDb.TotalQuestPercentCompleted = quest.TotalQuestPercentCompleted;
                //        questFromDb.LastMilestoneIndexCompleted = quest.LastMilestoneIndexCompleted;
                //        questFromDb.ChipsAwarded = quest.ChipsAwarded;

                //        db.SaveChanges();

                //        var response = GetUpdatedQuest(quest.PlayerId);
                //        return response;
                //    }
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private Quest GetUpdatedQuest(string playerId)
        {
            try
            {
                using (var db = new QuestDbContext())
                {
                    return db
                            .Quests
                            .Where(x => x.PlayerId == playerId)
                            .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private bool IsMilestoneCompleted(string playerId, int milestone)
        {
            try
            {
                using (var db = new QuestDbContext())
                {
                    return db
                             .Quests
                             .Any(x => x.PlayerId == playerId 
                                    && x.LastMilestoneIndexCompleted >= milestone);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
