using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public enum AppliedFor { Manager = 1, MarketingOfficer, OfficeAssistant, Others }
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
    public class Candidate:EntityBase
    {
       
        [Required, StringLength(50)]
        public string CandidateName { get; set; }
        [Required, DataType(DataType.Date)]
        public System.DateTime BirthDate { get; set; }
        [Required, EnumDataType(typeof(AppliedFor))]
        public AppliedFor AppliedFor { get; set; }
        [Required, Column(TypeName = "money"), DataType(DataType.Currency)]
        public decimal ExpectedSalary { get; set; }
        [Display(Name ="Ready to work at night")]
        public bool Conditions { get; set; }
        [Required, StringLength(30)]
        public string Picture { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
    }
    public class Qualification:EntityBase
    {
        
        [Required, StringLength(50)]
        public string Degree { get; set; }
        [Required]
        public int PassingYear { get; set; }
        [Required, StringLength(50)]
        public string Institute { get; set; }
        [Required, StringLength(20)]
        public string Result { get; set; }
        [Required, ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
    }
    
}
