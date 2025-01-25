using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Constants
{
    public enum ClassroomMode
    {
        [EnumMember(Value = "Online")]
        Online,

        [EnumMember(Value = "Offline")]
        Offline,
    }
}
