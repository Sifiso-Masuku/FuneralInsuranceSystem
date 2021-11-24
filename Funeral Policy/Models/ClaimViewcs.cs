using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Funeral_Policy.Models
{
    public class ClaimViewcs
    {
[Key]
        public int claimId { get; set; }
        
        [DisplayName("Membership Number")]
        public string MembershipNumber { get; set; }
        [DisplayName("Title")]
        public string Tittle { get; set; }
        public string creator { get; set; }
        public string Name { get; set; }
        
        public string Surname { get; set; }

        [DisplayName("Passport/ID Number")]
        public string NID { get; set; }
      
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DisplayName("Contact Number")]
        public string Phone { get; set; }
        public string DeceasedTittle { get; set; }
        
        [DisplayName("Name")]
        public string DeceasedName { get; set; }
        
        [DisplayName("Surname")]
        public string DeceasedSurname { get; set; }


       
        [DisplayName("Passport/ID Number")]
        public string DeceasedNID { get; set; }
        
        [DisplayName("Home Address")]
        public string DeceasedAddress { get; set; }
        [DisplayName("Home Address")]
        public string ClaimantAddress { get; set; }
       
        
        public string Relationship { get; set; }





        

       
        [DisplayName("Claim Date"), DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; }




        //[DisplayName("Deathe Date")]
        //[DataType(DataType.Date)]

        //public DateTime? DeathDate { get; set; }

        [DisplayName("Death Certificate Number")]
        public string DeathCertificateNo { get; set; }
        [DisplayName("Death Cause")]
        public string DeathCause { get; set; }

        
    }
}