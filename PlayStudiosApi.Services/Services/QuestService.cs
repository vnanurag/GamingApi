using Newtonsoft.Json;
using PlayStudiosApi.DataAccess.Models;
using PlayStudiosApi.DataAccess.Repos;
using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Services.Models;
using PlayStudiosApi.Services.Services.Interfaces;
using Serilog;
using System;
using System.Configuration;
using QuestState = PlayStudiosApi.Domain.Models.QuestState;

namespace PlayStudiosApi.Services.Services
{
    public class QuestService : IQuestService
    {
        private readonly ILogger _logger;
        private readonly IQuestRepository _questRepository;
        private int RateFromBet;
        private int LevelBonusRate;
        private int TargetQuestPoints;
        private int MilestonesPerQuest;
        private int MilestonesReward;

        public QuestService(
            ILogger logger,
            IQuestRepository questRepository)
        {
            _logger = logger;
            _questRepository = questRepository;

            // Setting Quest Config values
            RateFromBet = Convert.ToInt32(ConfigurationManager.AppSettings["RateFromBet"]);
            LevelBonusRate = Convert.ToInt32(ConfigurationManager.AppSettings["LevelBonusRate"]);
            TargetQuestPoints = Convert.ToInt32(ConfigurationManager.AppSettings["TargetQuestPoints"]);
            MilestonesPerQuest = Convert.ToInt32(ConfigurationManager.AppSettings["MilestonesPerQuest"]);
            MilestonesReward = Convert.ToInt32(ConfigurationManager.AppSettings["MilestonesReward"]);
        }

        /// <summary>
        /// Gets the Quest state for a give player id.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public QuestState GetQuestState(string playerId)
        {
            try
            {
                var questState = _questRepository
                                        .GetQuestState(playerId);

                if (questState == null)
                {
                    return null;
                }

                var result = new QuestState
                {
                    TotalQuestPercentCompleted = Convert.ToInt32(questState.TotalQuestPercentCompleted),
                    LastMilestoneIndexCompleted = questState.LastMilestoneIndexCompleted
                };

                _logger
                    .Information($"Quest state for the player {playerId} retrieved successfully " +
                                    $"with result {JsonConvert.SerializeObject(result)}");

                return result;
            }
            catch (Exception ex)
            {
                _logger
                    .Error($"Getting Quest State for player {playerId} failed with the following message {ex.Message}");

                throw;
            }
        }

        /// <summary>
        /// Adds/Updates the player information to the DB 
        /// and gets the updated quest progress of the player
        /// </summary>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        public QuestProgress GetQuestProgress(PlayerInfo playerInfo)
        {
            _logger
                .Information($"Getting Quest Progress for player {JsonConvert.SerializeObject(playerInfo)}");

            try
            {
                var questPointsEarned = (playerInfo.ChipAmountBet * RateFromBet) + (playerInfo.PlayerLevel * LevelBonusRate);
                var questPercentCompleted = ((double)questPointsEarned / TargetQuestPoints) * 100;
                var mileStonePercent = 100 / MilestonesPerQuest;
                var mileStoneIndex = Math.Floor(questPercentCompleted / mileStonePercent);
                var chipsAwarded = mileStoneIndex * MilestonesReward;

                // Creating the request to the DB
                var quest = new Quest
                {
                    PlayerId = playerInfo.PlayerId,
                    PlayerLevel = playerInfo.PlayerLevel,
                    QuestPointsEarned = questPointsEarned,
                    TotalQuestPercentCompleted = questPercentCompleted,
                    LastMilestoneIndexCompleted = Convert.ToInt32(mileStoneIndex),
                    ChipsAwarded = Convert.ToInt32(chipsAwarded)
                };

                var updatedQuest = _questRepository
                                        .AddOrUpdateQuestState(quest);

                // Updated quest progress is returned
                var result = new QuestProgress
                {
                    QuestPointsEarned = updatedQuest.QuestPointsEarned,
                    TotalQuestPercentCompleted = Convert.ToInt32(updatedQuest.TotalQuestPercentCompleted),
                    MilestonesCompleted = new MilestoneInfo
                    {
                        MilestoneIndex = Convert.ToInt32(updatedQuest.LastMilestoneIndexCompleted),
                        ChipsAwarded = Convert.ToInt32(updatedQuest.ChipsAwarded)
                    }
                };

                _logger
                    .Information($"Quest Progress for the player {playerInfo.PlayerId} retrieved successfully " +
                                    $"with result {JsonConvert.SerializeObject(result)}");

                return result;
            }
            catch (Exception ex)
            {
                _logger
                    .Error($"Getting Quest Progress for player {playerInfo.PlayerId} failed with the following message {ex.Message}");

                throw;
            }
        }
    }
}
