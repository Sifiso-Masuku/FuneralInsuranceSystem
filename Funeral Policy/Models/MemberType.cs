using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Funeral_Policy.Models
{
    public class MemberType
    {
        [Key]
        public int memberTypeId { get; set; }
        public string TypeName { get; set; }
        [DisplayName("Member payout"), DataType(DataType.Currency)]
        public decimal payout { get; set; }
        public string Name { get; set; }
        //[Required]
        public string Surname { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date fo Birth is required"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateOfBirth { get; set; }
        public ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}