using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class MemberStatus
    {
        [Key]
        public int memberStatusId { get; set; }
        public string MemberActive { get; set; }
    }
}