using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IdentitySample.Models;

namespace Funeral_Policy.Models
{
    public class Claim
    {
        [Key]
        public int claimId { get; set; }
        [Required]
        public string MembershipNumber { get; set; }
        [DisplayName("Title")]
        public string Tittle { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }

        public string CreatorClaim { get; set; }
        [Required]
        public string NID { get; set; }
        public string Race { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string DeceasedTittle { get; set; }
        [Required]
        public string DeceasedName { get; set; }
        [Required]
        public string DeceasedSurname { get; set; }

        public int memberTypeId { get; set; }
         [Required]
        public string DeceasedNID { get; set; }
        [Required]
        public string DeceasedAddress { get; set; }
        public string ClaimantAddress { get; set; }
        public string Status { get; set; }
        public int MemberAplicationId { get; set; }
        [Required]
        public string Relationship{ get; set; }


       
        [DisplayName("Claim Date"), DataType(DataType.Date)]
        public DateTime? ClaimDate { get; set; }

        public int qouteId { get; set; }
        public int funeralBookingId { get; set; }
        [DataType(DataType.Currency)]

        public decimal payoutDue { get; set; }
        public decimal FuneralCost { get; set; }
        //[DataType(DataType.Date)]


        //[ DisplayName("Deathe Date")]
        //public DateTime DeathDate { get; set; }


        public string DeathCertificateNo { get; set; }
        public string DeathCause { get; set; }

       
        public byte[] ClaimantCertifiedID { get; set; }

        public byte[] BankStatement { get; set; }

        public byte[] DeathCertificate { get; set; }

        
        public byte[] DeceasedCertifiedID { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        public decimal GetClaimPayout()
        {
            var z = (from e in db.quotes
                     where e.quoteId == qouteId
                     select e.Funeralpayout).FirstOrDefault();
            return (decimal)z;
        }
        public decimal GetFuneralCost()
        {
            var f = (from e in db.FuneralBookings
                     where e.funeralBookingId == funeralBookingId
                     select e.TotalCost).FirstOrDefault();
            return f;
        }
        public double CalcTotalPayout()
           
        {


            // return (GetFuneralCost() - GetFuneralCost() );
            return (double)(GetClaimPayout() - GetFuneralCost());

            
        }
        
       
    }
}