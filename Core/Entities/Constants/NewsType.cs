﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Constants
{
    public enum NewsType
    {
        [EnumMember(Value = "Blog")]
        Blog,

        [EnumMember(Value = "Program")]
        Program
    }
}
