using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PlayStudiosApi.Service.Configuration
{
    public class QuestConfiguration
    {
        public int RateFromBet { get; set; }
        public int LevelBonusRate { get; set; }
        public int TargetQuestPoints { get; set; }
        public int MilestonesPerQuest { get; set; }
        public int MilestonesReward { get; set; }
        public int MilestoneLimit { get; set; }
    }
}