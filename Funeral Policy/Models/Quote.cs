using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IdentitySample.Models;

namespace Funeral_Policy.Models
{
    public class Quote
    {
        [Key]
        public int quoteId { get; set; }
        [Required,DisplayName("First Name")]
        public string Name { get; set; }
        [Required, DisplayName("Surname")]
        public string Surname { get; set; }
        [Required]
        public string Gender { get; set; }
        public string MemberEmail { get; set; }
       public int MemberAplicationId { get; set; }
        [Required]
        public int Age { get; set; }
        [Required, DisplayName("cellphone")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "You must provide a phone number")]
        public string Phone { get; set; }
        [DisplayName("Funeral Plan")]
        public int FuneralPlanId { get; set; }
       public virtual FuneralPlan FuneralPlan { get; set; }
        //[Required,DisplayName("Payout")]
        //public int funeralCoverPayoutId { get; set; }
        //public virtual FuneralCoverPayout FuneralCoverPayout { get; set; }
        //public maritalStatus MaritalStatus { get; set; }
        [DisplayName("Quote Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Quote_Date { get; set; }
        [Required, DisplayName("Monthly Premium"), DataType(DataType.Currency)]
        public decimal PremiumCost { get; set; }
        public string Status { get; set; }
        [DisplayName("Cover Payout"), DataType(DataType.Currency)]
        public double Funeralpayout { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public ICollection<MemberApplication> memberApplications { get; set; }
        public ICollection<FuneralPlan> funeralPlans { get; set; }
        /// public string Email { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();
        public decimal GetFuneralPayout()
        {
            var w = (from e in db.funeralPlans
                     where e.FuneralPlanId == FuneralPlanId
                     select e.FuneralCoverPayout.PayoutAmount).FirstOrDefault();
            return w;
        }

      

        public string GetPlanName()
        {
            var p = (from m in db.funeralPlans
                     where m.FuneralPlanId == FuneralPlanId
                     select m.FuneralPlanName).FirstOrDefault();
            return p;
        }
        public string GetPlanDescription()
        {
            var z = (from v in db.funeralPlans
                     where v.FuneralPlanId == FuneralPlanId
                     select v.FuneralPlanDescription).FirstOrDefault();
            return z;
        }
        public double CalculatePrice()
        {
            // Work out base price.
            // Start with male customers.
            double TotalPremium;

            if (Gender == "Male")
            {
                if (Age >= 0 && Age <= 18)
                {
                    TotalPremium = 150.00;
                }
                else if (Age >= 19 && Age <= 24)
                {
                    TotalPremium = 180.00;
                }
                else if (Age >= 25 && (Age) <= 35)
                {
                    TotalPremium = 200.00;
                }
                else if ((Age) >= 35 && (Age) <= 45)
                {
                    TotalPremium = 250.00;
                }
                else if ((Age) >= 45 && (Age) <= 60)
                {
                    TotalPremium = 320.00;
                }
                else
                {
                    TotalPremium = 500.00;
                }
            }

            //Then look at female customers.
            else
            {
                if (Age >= 0 && Age <= 18)
                {
                    TotalPremium = 100.00;
                }
                else if (Age >= 19 && Age <= 24)
                {
                    TotalPremium = 165.00;
                }
                else if (Age >= 25 && Age <= 35)
                {
                    TotalPremium = 180.00;
                }
                else if (Age >= 35 && Age <= 45)
                {
                    TotalPremium = 225.00;
                }
                else if (Age >= 45 && Age <= 60)
                {
                    TotalPremium = 315.00;
                }
                else
                {
                    TotalPremium = 485.00;
                }
            }

            //    // Adjust Premium based on "Regional Health Index"
            //    switch (client.Country)
            //    {
            //        case "England":
            //            break;

            //        case "Wales":
            //            TotalPremium = TotalPremium - 100.00;
            //            break;

            //        case "Scotland":
            //            TotalPremium = TotalPremium + 200.00;
            //            break;

            //        case "Ireland":
            //            TotalPremium = TotalPremium + 50.00;
            //            break;

            //        case "Northern Ireland":
            //            TotalPremium = TotalPremium + 75.00;
            //            break;

            //        default:
            //            TotalPremium = TotalPremium + 100.00;
            //            break;
            //    }

            //    // If client has children increase premium by 50%
            //    if (client.Children == "Y")
            //    {
            //        TotalPremium = TotalPremium * 1.5;
            //    }

            //    // If client is a smoker increase premium by 300%
            //    if (client.Smoker == "Y")
            //    {
            //        TotalPremium = TotalPremium * 4.0;
            //    }

            //    // Adjust premium based on exercise.
            //    if (client.HoursOfExercise < 1)
            //    {
            //        TotalPremium = TotalPremium * 1.2;
            //    }
            //    else if (client.HoursOfExercise < 3)
            //    {
            //    }
            //    else if (client.HoursOfExercise < 6)
            //    {
            //        double Deduction = TotalPremium * 0.3;
            //        TotalPremium = TotalPremium - Deduction;
            //    }
            //    else if (client.HoursOfExercise < 9)
            //    {
            //        double Deduction = TotalPremium * 0.5;
            //        TotalPremium = TotalPremium - Deduction;
            //    }
            //    else if (client.HoursOfExercise >= 9)
            //    {
            //        TotalPremium = TotalPremium * 1.5;
            //    }

            // Check that the final premium is not less then R50, if it is make the premium R50
            if (TotalPremium < 50.00)
            {
                TotalPremium = 50.00;
            }
            return TotalPremium;
        }


    }

}
