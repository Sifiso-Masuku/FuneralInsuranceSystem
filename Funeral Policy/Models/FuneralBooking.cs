using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using IdentitySample.Models;
using Funeral_Policy.Models.CartModels;

namespace Funeral_Policy.Models
{
    public class FuneralBooking: IValidatableObject
    {
     [Key]
     public int funeralBookingId { get; set; }
        [DisplayName("Total Amount")]
        public decimal TotalCost { get; set; }
        [Display(Name = "Member Email")]
        public string creator { get; set; }

        [Display(Name = "Booking Date"), DataType(DataType.Date)]

        public DateTime DateCreated { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       
        [Required, DisplayName("Funeral Date")]
        public DateTime FuneralDate { get; set; }
        [DisplayName("Funeral Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime? Time { get; set; }
        public int funeralTypeId { get; set; }
        [DisplayName("Funeral Type")]
        public virtual FuneralType FuneralType { get; set; }
        [DisplayName("Funeral Address")]
        public string address { get; set; }
        [ DisplayName("Funeral Cost"), DataType(DataType.Currency)]
        public double FuneralPrice { get; set; }
        [DisplayName("Coffin Cost"), DataType(DataType.Currency)]
        public double CoffinPrice { get; set; }
        public string FuneralName { get; set; }
        public string CoffinName { get; set; }
        public int coffinId { get; set; }
        public string Status { get; set; }
        public bool isPaid { get; set; }
        public string Action { get; set; }
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
           


            if (FuneralDate < DateTime.Now.AddDays(5))
            {
                yield return new ValidationResult("Funeral date must be 5 days greater than current date");
            }
           
        }
        ApplicationDbContext db = new ApplicationDbContext();

        public decimal GetFuneralPrice()
        {
            var j = (from e in db.FuneralTypes
                     where e.funeralTypeId == funeralTypeId
                     select e.funeralCost).FirstOrDefault();
            return  j;
        }

        public string GetFuneralName()
        {
            var p = (from e in db.FuneralTypes
                     where e.funeralTypeId== funeralTypeId
                     select e.funeralTypeName).FirstOrDefault();
            return p;
        }
        public decimal GetCoffinPrice()
        {
            var rr = (from r in db.Coffins
                      where r.coffinId == coffinId
                      select r.coffinPrice).FirstOrDefault();
            return rr;
        }
        public string GetCoffinName()
        {
            var rr = (from r in db.Coffins
                      where r.coffinId== coffinId
                      select r.coffinName).FirstOrDefault();
            return rr;
        }
        public decimal CalcTotalCost()
        {
            return (GetCoffinPrice() +GetFuneralPrice() + (TotalCost));
        }
        public  ICollection<Cart> Carts { get; set; }
    }
}