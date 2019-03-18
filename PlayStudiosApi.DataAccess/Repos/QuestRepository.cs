using PlayStudiosApi.DataAccess.DBContext;
using PlayStudiosApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.Repos
{
    public class QuestRepository : IQuestRepository
    {
        public List<Quest> GetQuestInfo()
        {
            QuestDbContext questDbContext = new QuestDbContext();
            return questDbContext.Quests.ToList();
        }

        public QuestState GetQuestState(string playerId)
        {
            QuestDbContext questDbContext = new QuestDbContext();

            var result = questDbContext
                            .Quests
                            .Where(x => x.PlayerId == playerId)
                            .Select(x => new QuestState
                            {
                                TotalQuestPercentCompleted = x.TotalQuestPercentCompleted,
                                LastMilestoneIndexCompleted = x.LastMilestoneIndexCompleted
                            })
                            .FirstOrDefault();

            return result;
        }
    }
}
