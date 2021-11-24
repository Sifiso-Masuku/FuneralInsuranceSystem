using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funeral_Policy.Models
{
    public class Coffin
    {
        [Key]
        public int coffinId { get; set; }
        [DisplayName("Coffin Name")]
        public string coffinName { get; set; }
        [DisplayName("Coffin Type")]
        public string coffinType { get; set; }
        [Required, DisplayName("Coffin Price"), DataType(DataType.Currency)]
        public decimal coffinPrice { get; set; }
        public string ImageType { get; set; }
        public byte[] Image { get; set; }
    }
}