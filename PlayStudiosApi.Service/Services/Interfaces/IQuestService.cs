using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Service.Models;

namespace PlayStudiosApi.Service.Services.Interfaces
{
    public interface IQuestService
    {
        QuestState GetQuestState(string playerId);
        QuestProgress GetQuestProgress(PlayerInfo playerInfo);
    }
}
