using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Funeral_Policy.Models
{
    public class AdditionalMemberView
    {
        [Key]
        public int AddionalMemberId { get; set; }
        public int MemberID { get; set; }
        public int MemberTypeID { get; set; }
       // [Required]
        public string Name { get; set; }
        //[Required]
        public string Surname { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date fo Birth is required"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateOfBirth { get; set; }
        
    }
}