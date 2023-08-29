namespace Final.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandidateName = c.String(nullable: false, maxLength: 50),
                        BirthDate = c.DateTime(nullable: false),
                        AppliedFor = c.Int(nullable: false),
                        ExpectedSalary = c.Decimal(nullable: false, storeType: "money"),
                        Conditions = c.Boolean(nullable: false),
                        Picture = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Degree = c.String(nullable: false, maxLength: 50),
                        PassingYear = c.Int(nullable: false),
                        Institute = c.String(nullable: false, maxLength: 50),
                        Result = c.String(nullable: false, maxLength: 20),
                        CandidateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.CandidateId, cascadeDelete: true)
                .Index(t => t.CandidateId);
            CreateStoredProcedure("dbo.DeleteCandidate",
                p => new { id = p.Int() },
                "DELETE FROM dbo.Candidates WHERE id=@id");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Qualifications", "CandidateId", "dbo.Candidates");
            DropIndex("dbo.Qualifications", new[] { "CandidateId" });
            DropTable("dbo.Qualifications");
            DropTable("dbo.Candidates");
            DropStoredProcedure("dbo.DeleteCandidate");
        }
    }
}
