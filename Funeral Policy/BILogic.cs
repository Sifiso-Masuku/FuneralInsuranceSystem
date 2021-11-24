using IdentitySample.Models;
using Funeral_Policy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Funeral_Policy.Models
{
    public class BILogic
    {
        private readonly static ApplicationDbContext db = new ApplicationDbContext();

        public static decimal GetPayouAmount(int? qouteId)
        {
            var payout = db.quotes.Find(qouteId);
            return (decimal)payout.Funeralpayout;
        }
        //public static decimal GetPayouAmount(int? claimId)
        //{
        //    var payout = db.Claims.Find(claimId);
        //    return (decimal)payout.CalcTotalPayout();
        //}
        public static void ApplicationStatus(int? id, string statuse)
        {
            var dbRecord = db.Claims.Find(id);
            dbRecord.Status = statuse;
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
           // EmailSender.ResidenceApplication(dbRecord);
        }

        //public static bool ChechRoomNumber(string roomNumber)
        //{
        //    var rooms = db.Rooms.ToList();
        //    foreach (var item in rooms)
        //    {
        //        return roomNumber != item.RoomNumber;
        //    }
        //    return false;
        //}

        public static bool CheckDOB(DateTime DOB)
        {
            var years = DateTime.Now.Year - DOB.Year;
            return years > 10;
        }
    }
}