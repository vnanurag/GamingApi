using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public int ChipsAwarded { get; set; }
        public int QuestPointsEarned { get; set; }
        public double TotalQuestPercentCompleted { get; set; } 
        public int LastMilestoneIndexCompleted { get; set; }
    }
}
