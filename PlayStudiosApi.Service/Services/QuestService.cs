using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Service.Configuration;
using PlayStudiosApi.Service.Models;
using PlayStudiosApi.Service.Services.Interfaces;
using Serilog;
using System;

namespace PlayStudiosApi.Service.Services
{
    public class QuestService : IQuestService
    {
        private readonly ILogger _logger;
        private readonly QuestConfiguration _configuration;
        private int RateFromBet = 10;
        private int LevelBonusRate = 20;
        private int TargetQuestPoints = 1000;
        private int MilestonesPerQuest = 4;
        private int MilestonesReward = 250;
        private int MilestoneLimit =1;

        public QuestService(
            ILogger logger, 
            QuestConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            //RateFromBet = _configuration.RateFromBet;
            //LevelBonusRate = _configuration.LevelBonusRate;
            //TargetQuestPoints = _configuration.TargetQuestPoints;
            //MilestonesPerQuest = _configuration.MilestonesPerQuest;
            //MilestonesReward = _configuration.MilestonesReward;
            //MilestoneLimit = _configuration.MilestoneLimit;
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
            //_logger
            //    .Information($"Getting Quest Progress for player {JsonConvert.SerializeObject(playerInfo)}");

            try
            {
                var questPointsEarned = (playerInfo.ChipAmountBet * RateFromBet) + (playerInfo.PlayerLevel * LevelBonusRate);
                var questPercentCompleted = ((double)questPointsEarned / TargetQuestPoints) * 100;

                var mileStonePercent = 100 / MilestonesPerQuest;
                var mileStoneIndex = questPercentCompleted / mileStonePercent;
                var chipsAwarded = mileStoneIndex * MilestonesReward;

                var result = new QuestProgress
                {
                    QuestPointsEarned = questPointsEarned,
                    TotalQuestPercentCompleted = Convert.ToInt32(questPercentCompleted),
                    MilestonesCompleted = new MilestoneInfo
                    {
                        MilestoneIndex = Convert.ToInt32(mileStoneIndex),
                        ChipsAwarded = Convert.ToInt32(chipsAwarded)
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
