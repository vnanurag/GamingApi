using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Services.Models;

namespace PlayStudiosApi.Services.Services.Interfaces
{
    public interface IQuestService
    {
        QuestState GetQuestState(string playerId);
        QuestProgress GetQuestProgress(PlayerInfo playerInfo);
    }
}
