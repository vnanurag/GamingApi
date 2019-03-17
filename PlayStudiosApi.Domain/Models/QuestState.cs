using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayStudiosApi.Domain.Models
{
    public class QuestState
    {
        public int TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
    }
}