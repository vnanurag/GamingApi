using PlayStudiosApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.Repos
{
    public interface IQuestRepository
    {
        QuestState GetQuestState(string playerId);
        Quest AddOrUpdateQuestState(Quest quest);
    }
}
