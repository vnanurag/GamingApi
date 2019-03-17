namespace PlayStudiosApi.Domain.Models
{
    public class QuestProgress
    {
        public int QuestPointsEarned { get; set; }
        public int TotalQuestPercentCompleted { get; set; }
        public MilestoneInfo MilestonesCompleted { get; set; }
    }
}