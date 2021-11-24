using IdentitySample.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funeral_Policy.Models
{
    public class MemberApplication
    {
        [Key]
        public int MemberAplicationId { get; set; }
        [Required]
        [DisplayName("Title")]
        public string Tittle { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string NID { get; set; }
        public string Race { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string HomeAddress { get; set; }
        public string city { get; set; }
        [Required]
        public string Province { get; set; }
        [Required, DisplayName("Monthly Earnings"), DataType(DataType.Currency)]
        public decimal MonthlyEarnings { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public string Education { get; set; }
       
        public byte[] CertifiedID { get; set; }

        public byte[] BankStatement { get; set; }
      
        public byte[] PaySlip { get; set; }
        
        public byte[] ProofOfAddress { get; set; }
        
        public int quoteId { get; set; }
        public decimal PremiumCost { get; set; }
        public string FuneralPlanName { get; set; }
       public decimal Payout { get; set; }
        public string Status { get; set; }
        [DisplayName("Membership No")]
        public string MembershipNumber { get; set; }
        public double BalanceDue { get; set; }
       public int memberTypeId { get; set; }
        // public string creator { get; set; }
        public ICollection<ApplicationDocuments> applicationDocuments { get; set; }
        public ICollection<Quote> quotes { get; set; }
        public ICollection<FuneralCoverPayout> funeralCoverPayouts { get; set; }
        public ICollection<FuneralPlan> funeralPlans { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();
        public decimal GetPremiumCost()

        {
            var p = (from e in db.quotes
                     where e.quoteId == quoteId
                     select e.PremiumCost).FirstOrDefault();
            return p;
        }
        public int GetId()
        {
            var id = (from i in db.MemberApplications
                      where i.MemberAplicationId == MemberAplicationId

                      select i.MemberAplicationId

                    ).FirstOrDefault();

            return id;

        }
        public string GetFuneralName()

        {
            var k = (from e in db.quotes
                     where e.quoteId == quoteId
                     select e.FuneralPlan.FuneralPlanName).FirstOrDefault();
            return k;
        }

        //public decimal GetPayout()

        //{
        //    var p = (from e in db.quotes
        //             where e.quoteId == quoteId
        //             select e.FuneralCoverPayout.PayoutAmount).FirstOrDefault();
        //    return p;
        //}
    }

}