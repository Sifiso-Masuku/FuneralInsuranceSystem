using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Funeral_Policy.Models.Enum;

namespace Funeral_Policy.Models
{
    public class Beneficiary
    {
        [Key]
        public int beneficiaryId { get; set; }
        [Required, DisplayName("Title")]
        public Tittle Tittle { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        [Required, DisplayName("ID Number")]
        public string NID { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date fo Birth is required"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public Relationship Relationship { get; set; }
    }
}