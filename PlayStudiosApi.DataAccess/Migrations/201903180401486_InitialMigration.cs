namespace PlayStudiosApi.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.String(),
                        PlayerLevel = c.Int(nullable: false),
                        ChipsAwarded = c.Int(nullable: false),
                        QuestPointsEarned = c.Int(nullable: false),
                        TotalQuestPercentCompleted = c.Double(nullable: false),
                        LastMilestoneIndexCompleted = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Quests");
        }
    }
}
