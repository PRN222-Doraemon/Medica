using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Constants
{
    public enum ContactStatus
    {
        [EnumMember(Value = "Unknown")]
        Unknown,

        [EnumMember(Value = "Booked")]
        Contacted
    }
}
