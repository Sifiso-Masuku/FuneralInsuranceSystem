using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}