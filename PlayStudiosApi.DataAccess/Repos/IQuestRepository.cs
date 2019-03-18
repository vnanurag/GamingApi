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
        List<Quest> GetQuestInfo();
        QuestState GetQuestState(string playerId);
    }
}
