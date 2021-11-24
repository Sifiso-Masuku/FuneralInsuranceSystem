using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models
{
    public class FuneralPlan
    {
        [Key]
        public int FuneralPlanId { get; set; }
        [DisplayName("Funeral Plan Name")]
        public string FuneralPlanName { get; set; }
        
        [ DisplayName("Payout")]
        public int funeralCoverPayoutId { get; set; }
        public virtual FuneralCoverPayout FuneralCoverPayout { get; set; }
        [DisplayName("Funeral Description")]
        public string FuneralPlanDescription { get; set; }
        public virtual List<Quote> Quotes { get; set; }
    }
}