using Core.Entities;

public class FilterButtonVM
{
    public ClassroomStatus? ClassroomStatus { get; set; }
    public int? CourseId { get; set; }
    public string Controller { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}