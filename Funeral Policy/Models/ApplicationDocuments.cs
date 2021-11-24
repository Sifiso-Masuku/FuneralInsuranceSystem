using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class ApplicationDocuments
    {
        [Key]
        public int applicationDocumentsId { get; set; }
        [DisplayName("Application Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ApplicationDate { get; set; }
        [Required,DisplayName("Certified ID/Passport ")]
        public byte[] CertifiedID { get; set; }

        [Required,DisplayName("3 Months Bank Statement")]
        public byte[] BankStatement { get; set; }
        [Required, DisplayName("Payslip")]
        public byte[] PaySlip { get; set; }

        [Required,DisplayName("Proof of Home Address ")]
        public byte[] ProofOfAddress { get; set; }
    }
}