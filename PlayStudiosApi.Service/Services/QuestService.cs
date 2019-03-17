using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Service.Configuration;
using PlayStudiosApi.Service.Models;
using PlayStudiosApi.Service.Services.Interfaces;
using System;

namespace PlayStudiosApi.Service.Services
{
    public class QuestService : IQuestService
    {
        private readonly ILogger _logger;
        private readonly QuestConfiguration _configuration;
        private int RateFromBet;
        private int LevelBonusRate;
        private int TargetQuestPoints;
        private int MilestonesPerQuest;
        private int MilestonesReward;
        private int MilestoneLimit;

        public QuestService(
            ILogger logger, 
            QuestConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            RateFromBet = _configuration.RateFromBet;
            LevelBonusRate = _configuration.LevelBonusRate;
            TargetQuestPoints = _configuration.TargetQuestPoints;
            MilestonesPerQuest = _configuration.MilestonesPerQuest;
            MilestonesReward = _configuration.MilestonesReward;
            MilestoneLimit = _configuration.MilestoneLimit;
        }

        public QuestState GetQuestState(string playerId)
        {
            try
            {
                return new QuestState();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public QuestProgress GetQuestProgress(PlayerInfo playerInfo)
        {
            _logger
                .Info($"Getting Quest Progress for player {JsonConvert.SerializeObject(playerInfo)}");

            try
            {
                var questPointsEarned = (playerInfo.ChipAmountBet * RateFromBet) + (playerInfo.PlayerLevel * LevelBonusRate);
                var questPercentCompleted = (questPointsEarned / TargetQuestPoints) * 100;

                var mileStonePercent = 100 / MilestonesPerQuest;
                var mileStoneIndex = questPercentCompleted / mileStonePercent;
                var chipsAwarded = mileStoneIndex * MilestonesReward;

                var result = new QuestProgress
                {
                    QuestPointsEarned = questPointsEarned,
                    TotalQuestPercentCompleted = questPercentCompleted,
                    MilestonesCompleted = new MilestoneInfo
                    {
                        MilestoneIndex = mileStoneIndex,
                        ChipsAwarded = chipsAwarded
                    }
                };

                _logger
                    .Info($"Quest Progress for the player {playerInfo.PlayerId} retrieved successfully " +
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
