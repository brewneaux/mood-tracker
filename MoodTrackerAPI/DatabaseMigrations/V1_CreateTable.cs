using FluentMigrator;
using FluentMigrator.SqlServer;

namespace MoodTrackerAPI.DatabaseMigrations
{
    [Migration(20200601100000)]
    public class Migration20200601100000 : Migration
    {
        public override void Down()
        {
            Delete.Table("gad7");
            Delete.Table("phq9");
        }
 
        public override void Up()
        {
            Create.Table("gad7")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
                .WithColumn("response_date").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("anxious_nervous_on_edge").AsInt16()
                .WithColumn("cant_stop_worrying").AsInt16()
                .WithColumn("worrying_about_too_many_things").AsInt16()
                .WithColumn("trouble_relaxing").AsInt16()
                .WithColumn("restless").AsInt16()
                .WithColumn("annoyed").AsInt16()
                .WithColumn("afraid").AsInt16()
                .WithColumn("score").AsInt16()
                .WithColumn("severity").AsAnsiString(50);
            Create.Table("phq9").WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
                .WithColumn("response_date").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("little_interest_or_pleasure").AsInt16()
                .WithColumn("down_depressed_hopeless").AsInt16()
                .WithColumn("trouble_sleeping").AsInt16()
                .WithColumn("feeling_tired").AsInt16()
                .WithColumn("appetite").AsInt16()
                .WithColumn("self_loathing").AsInt16()
                .WithColumn("trouble_concentrating").AsInt16()
                .WithColumn("slow_or_fast").AsInt16()
                .WithColumn("self_harm").AsInt16()
                .WithColumn("score").AsInt16()
                .WithColumn("severity").AsAnsiString(50);
        }
    }
}