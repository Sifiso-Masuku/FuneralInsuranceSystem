using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class FuneralType
    {
        [Key]
        public int funeralTypeId { get; set; }
        [DisplayName("Funeral Type Name")]
        public string funeralTypeName { get; set; }
        
        [DisplayName("Funeral Cost"), DataType(DataType.Currency)]
        public decimal funeralCost { get; set; }
        public string description { get; set; }
    }
}