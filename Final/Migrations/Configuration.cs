namespace Final.Migrations
{
    using Final.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Final.Models.CandidateDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Final.Models.CandidateDbContext context)
        {
            Candidate c = new Candidate { CandidateName = "Jahangir Alam", BirthDate = new DateTime(1999, 4, 1), AppliedFor = AppliedFor.Manager, ExpectedSalary = 10500.00M, Picture = "1.jpg", Conditions = true };
            c.Qualifications.Add(new Qualification { Degree = "HSC", PassingYear = 2015, Institute = "Hossenpur", Result = "4.31" });
            context.Candidates.Add(c);
            context.SaveChanges();
        }
    }
}
