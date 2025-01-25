using Core.Entities.Constants;
using MindSpace.Domain.Entities.Owned;

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
}
