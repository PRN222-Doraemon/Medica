using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal TotalPrice { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public virtual IEnumerable<Classroom> Classes { get; set; } = new List<Classroom>();
    }
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Paid")]
        Paid
    }
}
