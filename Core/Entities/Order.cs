using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
        public int PaymentIntentId { get; set; }
        public decimal TotalPrice { get; set; }
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
