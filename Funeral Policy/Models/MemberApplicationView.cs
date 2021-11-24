using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Funeral_Policy.Models.Enum;
using IdentitySample.Models;

namespace Funeral_Policy.Models
{
    public class MemberApplicationView
    {
        [Key]
        public int MemberAplicationId { get; set; }
        [DisplayName("Title")]
        public Tittle Tittle { get; set; }
        //[Required]
        [DisplayName("Name(s)")]
        public string Name { get; set; }
        [ DisplayName("Surname")]
        public string Surname { get; set; }
        [DisplayName("Gender")]
        public Gender Gender { get; set; }
        [ DisplayName("SA National ID")]
        public string NID { get; set; }
        public Race Race { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(pattern: @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        [ DisplayName("cellphone")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "You must provide a phone number")]
        public string Phone { get; set; }

       // [Required]
        [DisplayName("Home Address")]
        public string HomeAddress { get; set; }
        [DisplayName("City")]
        public string city { get; set; }
       // [Required]
        public Province Province { get; set; }
        [Required]
        [DisplayName("Earnings per month (before tax)"), DataType(DataType.Currency)]
        public decimal MonthlyEarnings { get; set; }
        [ DisplayName("Occupation")]
        
        public Occupation Occupation { get; set; }
        [DisplayName("Education")]
        public Education Education { get; set; }

        public string Status { get; set; }
        [DisplayName("Membership No")]
        public string MembershipNumber { get; set; }
        public decimal PremiumCost { get; set; }
        public string MemberEmail { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();

        public string generateMemberNumber(string id)
        {
            string year = Convert.ToString(System.DateTime.Now.Year);
            string result = year.Substring(0, 1) + year.Substring(2) + id.Substring(8);

            return result;
        }

        public decimal GetPremiumcost()
        {
            var premium = (from p in db.MemberApplications
                           where p.MemberAplicationId == MemberAplicationId
                           select p.PremiumCost).FirstOrDefault();
            return premium;
        }


    }
}