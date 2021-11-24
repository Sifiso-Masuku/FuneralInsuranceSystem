using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IdentitySample.Models;

namespace Funeral_Policy.Models
{
    public class AssignStaff
    {
        [Key]
        public int AssignId { get; set; }

        public int StaffId { get; set; }
        public virtual Staff Staff { get; set; }
        [DisplayName("order Number")]
        public int funeralBookingId { get; set; }
        public string Member { get; set; }
        public string Coffin { get; set; }
        [DisplayName("Funeral Type")]
        public string FuneralType { get; set; }
        [DisplayName("Funeral Date")]
        public DateTime FuneraralDate { get; set; }
        [DisplayName("Funeral Address")]
        public string adress { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();
        public string user()
        {
            var u = (from s in db.FuneralBookings
                     where s.funeralBookingId == funeralBookingId
                     select s.creator).FirstOrDefault();
            return u;
        }
        public string Coffins()
        {
            var u = (from s in db.FuneralBookings
                     where s.funeralBookingId == funeralBookingId
                     select s.CoffinName).FirstOrDefault();
            return u;
        }
        public string funeralTypes()
        {
            var u = (from s in db.FuneralBookings
                     where s.funeralBookingId == funeralBookingId
                     select s.FuneralName).FirstOrDefault();
            return u;
        }
        public string FuneralAddress()
        {
            var j = (from s in db.FuneralBookings
                     where s.funeralBookingId == funeralBookingId
                     select s.address).FirstOrDefault();
            return j;
        }

        public DateTime Date()
        {
            var d = (from s in db.FuneralBookings
                     where s.funeralBookingId == funeralBookingId
                     select s.FuneralDate).FirstOrDefault();
            return d;
        }
        public string FuneralName()
        {
            var z = (from s in db.FuneralBookings
                     where s.funeralBookingId == funeralBookingId
                     select s.FuneralName).FirstOrDefault();
            return z;
        }
    }
}
