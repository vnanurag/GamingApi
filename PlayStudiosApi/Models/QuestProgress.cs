using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayStudiosApi.Models
{
    public class QuestProgress
    {
        public int QuestPointsEarned { get; set; }
        public int TotalQuestPercentCompleted { get; set; }
        public MilestoneInfo[] MilestonesCompleted { get; set; }
    }
}