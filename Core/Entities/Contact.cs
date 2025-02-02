using Core.Entities.Owned;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Contact : BaseEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public DateTime LastContacted { get; set; }
        public bool IsSubscribed { get; set; }
        public ContactStatus Status { get; set; }
    }

    public enum ContactStatus
    {
        [EnumMember(Value = "Unknown")]
        Unknown,

        [EnumMember(Value = "Booked")]
        Contacted
    }
}
