using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Funeral_Policy.Models.Enum
{
    public enum Education
    {
        [Description("No Matric")]NoMatric=1,
        Matric=2,
        [Description("University,Technikon,Degree,Diploma")] University, Technikon, Degree, Diploma=3,
        [Description("Post-graduate(Honours, Masters etc)")] Postgraduate=4

    }
}