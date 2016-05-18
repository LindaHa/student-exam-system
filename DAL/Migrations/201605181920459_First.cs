namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        Text = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        PointsForCorrectAnswer = c.Int(nullable: false),
                        Explanation = c.String(),
                        TypeOfQuestion = c.String(nullable: false),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestPatterns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        NumberOfQuestions = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Time = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        StudentGroupId = c.Int(nullable: false),
                        TeacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .ForeignKey("dbo.StudentGroups", t => t.StudentGroupId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId)
                .Index(t => t.AreaId)
                .Index(t => t.StudentGroupId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.StudentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Solutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        TestPatternId = c.Int(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.TestPatterns", t => t.TestPatternId)
                .Index(t => t.StudentId)
                .Index(t => t.TestPatternId);
            
            CreateTable(
                "dbo.SolutionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answer_Id = c.Int(nullable: false),
                        Solution_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.Answer_Id)
                .ForeignKey("dbo.Solutions", t => t.Solution_Id)
                .Index(t => t.Answer_Id)
                .Index(t => t.Solution_Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(),
                        TeacherId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId)
                .Index(t => t.StudentId)
                .Index(t => t.TeacherId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StudentStudentGroups",
                c => new
                    {
                        Student_Id = c.Int(nullable: false),
                        StudentGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.StudentGroup_Id })
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.StudentGroups", t => t.StudentGroup_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.StudentGroup_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.AspNetUsers", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TestPatterns", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.TestPatterns", "StudentGroupId", "dbo.StudentGroups");
            DropForeignKey("dbo.StudentStudentGroups", "StudentGroup_Id", "dbo.StudentGroups");
            DropForeignKey("dbo.StudentStudentGroups", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Solutions", "TestPatternId", "dbo.TestPatterns");
            DropForeignKey("dbo.Solutions", "StudentId", "dbo.Students");
            DropForeignKey("dbo.SolutionAnswers", "Solution_Id", "dbo.Solutions");
            DropForeignKey("dbo.SolutionAnswers", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.TestPatterns", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Questions", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.StudentStudentGroups", new[] { "StudentGroup_Id" });
            DropIndex("dbo.StudentStudentGroups", new[] { "Student_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "TeacherId" });
            DropIndex("dbo.AspNetUsers", new[] { "StudentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SolutionAnswers", new[] { "Solution_Id" });
            DropIndex("dbo.SolutionAnswers", new[] { "Answer_Id" });
            DropIndex("dbo.Solutions", new[] { "TestPatternId" });
            DropIndex("dbo.Solutions", new[] { "StudentId" });
            DropIndex("dbo.TestPatterns", new[] { "TeacherId" });
            DropIndex("dbo.TestPatterns", new[] { "StudentGroupId" });
            DropIndex("dbo.TestPatterns", new[] { "AreaId" });
            DropIndex("dbo.Questions", new[] { "AreaId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.StudentStudentGroups");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Teachers");
            DropTable("dbo.SolutionAnswers");
            DropTable("dbo.Solutions");
            DropTable("dbo.Students");
            DropTable("dbo.StudentGroups");
            DropTable("dbo.TestPatterns");
            DropTable("dbo.Areas");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
