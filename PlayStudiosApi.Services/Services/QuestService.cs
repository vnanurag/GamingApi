using Newtonsoft.Json;
using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Services.Models;
using PlayStudiosApi.Services.Services.Interfaces;
using Serilog;
using System;
using System.Configuration;

namespace PlayStudiosApi.Services.Services
{
    public class QuestService : IQuestService
    {
        private readonly ILogger _logger;
        //private readonly QuestConfiguration _configuration;
        private int RateFromBet;
        private int LevelBonusRate;
        private int TargetQuestPoints;
        private int MilestonesPerQuest;
        private int MilestonesReward;
        private int MilestoneLimit;

        public QuestService(
            ILogger logger)
            //IConfigurationSectionFactory<QuestConfiguration> configuration)
        {
            _logger = logger;
            //_configuration = configuration.Load("QuestConfig");

            RateFromBet = Convert.ToInt32(ConfigurationManager.AppSettings["RateFromBet"]);
            LevelBonusRate = Convert.ToInt32(ConfigurationManager.AppSettings["LevelBonusRate"]);
            TargetQuestPoints = Convert.ToInt32(ConfigurationManager.AppSettings["TargetQuestPoints"]);
            MilestonesPerQuest = Convert.ToInt32(ConfigurationManager.AppSettings["MilestonesPerQuest"]);
            MilestonesReward = Convert.ToInt32(ConfigurationManager.AppSettings["MilestonesReward"]);
            MilestoneLimit = Convert.ToInt32(ConfigurationManager.AppSettings["MilestoneLimit"]);
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
