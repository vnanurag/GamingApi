using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayStudiosApi.Models
{
    public class PlayerProgress
    {
        public int TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
    }
}