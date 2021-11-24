using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Funeral_Policy.Models.Enum
{
    public enum Occupation
    {
        Manager=1,
        Professional=2,
        Technician=3,
        Clerk=4,
        [Description("Sales an Services")]SalesandServices=5,
        [Description("Skilled agriculture")]SkilledAgriculture=6,
        [Description("Craft and related trade")]CraftAndRelatedTrade=7,
        [Description("Plant and machine operator")]PlantAndMachineOperator=8,
        Elementary=9,
        [Description("Armed forces")]ArmedForces=10,
        Pensioner=11,
        Unemployed=12
    }
}