using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace MedicaWeb_MVC.ViewModels.Classes
{
    public class ClassUpsertVM
    {
        public int Id { get; set; }
        public ClassroomMode Mode { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }
        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        public string? Description { get; set; }
        [Required(ErrorMessage = "Maximum participants is required.")]
        [Range(1, 50, ErrorMessage = "Max participants must be between 1 and 50.")]
        public int MaxParticipant { get; set; }
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Lecturer is required.")]
        public int LecturerId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (StartDate <= today)
            {
                yield return new ValidationResult("Start date must be greater than today.", new[] { nameof(StartDate) });
            }

            if (EndDate <= StartDate)
            {
                yield return new ValidationResult("End date must be greater than start date.", new[] { nameof(EndDate) });
            }
        }
    }
}
