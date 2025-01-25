using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Constants
{
    public enum NewsStatus
    {
        [EnumMember(Value = "Active")]
        Active,

        [EnumMember(Value = "Disabled")]
        Disabled

    }
}
