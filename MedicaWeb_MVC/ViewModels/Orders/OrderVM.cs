using Core.Entities;
using MedicaWeb_MVC.ViewModels.Classes;

namespace MedicaWeb_MVC.ViewModels.Orders
{
    public class OrderVM
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
        public int PaymentIntentId { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual IEnumerable<MyLearningVM> Classes { get; set; } = new List<MyLearningVM>();
    }
}
