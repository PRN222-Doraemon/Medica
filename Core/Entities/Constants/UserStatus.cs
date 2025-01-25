using System.Runtime.Serialization;

namespace Core.Entities.Constants
{
    public enum UserStatus
    {
        [EnumMember(Value = "Enabled")]
        Enabled,

        [EnumMember(Value = "Disabled")]
        Disabled
    }
}
