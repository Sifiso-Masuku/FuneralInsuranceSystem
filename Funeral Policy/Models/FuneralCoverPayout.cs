using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class FuneralCoverPayout
    {
        [Key]
        public int funeralCoverPayoutId { get; set; }
        [DisplayName("Payout Amount"), DataType(DataType.Currency)]
        public decimal PayoutAmount { get; set; }
        public virtual List<Quote> Quotes { get; set; }
    }
}