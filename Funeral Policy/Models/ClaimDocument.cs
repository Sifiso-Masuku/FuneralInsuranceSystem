using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class ClaimDocument
    {
        [Key]
        public int ClaimDocumentId { get; set; }
        [Display(Name = "Claim Date"), DataType(DataType.Date)]

        public DateTime DateCreated { get; set; }
        public string creator { get; set; }
        [Required, DisplayName("claimant Certified ID/Passport ")]
        public byte[] ClaimantCertifiedID { get; set; }
        [Required, DisplayName("claimant Bank Statement ")]
        public byte[] BankStatement { get; set; }
        [Required, DisplayName("Death Certificate ")]
        public byte[] DeathCertificate { get; set; }

        [Required, DisplayName(" Deceased Certified ID/Passport ")]
        public byte[] DeceasedCertifiedID { get; set; }
    }
}