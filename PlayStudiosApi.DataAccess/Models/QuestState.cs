using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.Models
{
    public class QuestState
    {
        public double TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
    }
}
