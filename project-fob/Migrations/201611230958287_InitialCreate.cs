namespace project_fob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Meeting_Id = c.Int(),
                        User_Id = c.Int(),
                        Fob_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .ForeignKey("dbo.Fob", t => t.Fob_Id)
                .Index(t => t.Meeting_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Fob_Id);
            
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeetingId = c.String(maxLength: 9, unicode: false),
                        Active = c.Boolean(nullable: false),
                        RoomPassword = c.String(),
                        HostPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.MeetingId, t.Active }, unique: true, name: "IX_FirstAndSecond");
            
            CreateTable(
                "dbo.Host",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Meeting_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Meeting_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 9, unicode: false),
                        Lastcheckin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserId, unique: true);
            
            CreateTable(
                "dbo.Stats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Attendeescount = c.Int(nullable: false),
                        Fobcount = c.Int(nullable: false),
                        TopicStartTime = c.DateTime(nullable: false),
                        TopicStopTime = c.DateTime(nullable: false),
                        Meeting_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id)
                .Index(t => t.Meeting_Id);
            
            CreateTable(
                "dbo.Fob",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttendeeCount = c.Int(nullable: false),
                        FobCount = c.Int(nullable: false),
                        TopicStartTime = c.DateTime(nullable: false),
                        Meeting_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id)
                .Index(t => t.Meeting_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fob", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.Attendee", "Fob_Id", "dbo.Fob");
            DropForeignKey("dbo.Attendee", "User_Id", "dbo.User");
            DropForeignKey("dbo.Stats", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.Host", "User_Id", "dbo.User");
            DropForeignKey("dbo.Host", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.Attendee", "Meeting_Id", "dbo.Meeting");
            DropIndex("dbo.Fob", new[] { "Meeting_Id" });
            DropIndex("dbo.Stats", new[] { "Meeting_Id" });
            DropIndex("dbo.User", new[] { "UserId" });
            DropIndex("dbo.Host", new[] { "User_Id" });
            DropIndex("dbo.Host", new[] { "Meeting_Id" });
            DropIndex("dbo.Meeting", "IX_FirstAndSecond");
            DropIndex("dbo.Attendee", new[] { "Fob_Id" });
            DropIndex("dbo.Attendee", new[] { "User_Id" });
            DropIndex("dbo.Attendee", new[] { "Meeting_Id" });
            DropTable("dbo.Fob");
            DropTable("dbo.Stats");
            DropTable("dbo.User");
            DropTable("dbo.Host");
            DropTable("dbo.Meeting");
            DropTable("dbo.Attendee");
        }
    }
}
