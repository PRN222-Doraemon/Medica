using Core.Entities;
using MedicaWeb_MVC.ViewModels.Classes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MedicaWeb_MVC.ViewModels.Orders
{
    public class OrderVM
    {
        [ValidateNever]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual IEnumerable<ClassVM> Classes { get; set; } = new List<ClassVM>();
    }
}
