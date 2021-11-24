using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        public int Registerapp { get; set; }
        [DisplayName("First Name")]
        public string Name { get; set; }
        [ DisplayName("Surname")]
        public string Surname { get; set; }

        //[Required]
        [DisplayName("National ID NO")]
        public string NID { get; set; }
        //[Required]
        [DisplayName("cellphone")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "You must provide a phone number")]
        public string Phone { get; set; }
        
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(pattern: @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        //[Required]
        [DisplayName("Home Address")]
        public string HomeAddress { get; set; }
        public string MembershipNumber { get; set; }
        public decimal PremiumCost { get; set; }
        public string Status { get; set; }
        public double BalanceDue { get; set; }
        public string FuneralPlan { get; set; }


    }
}