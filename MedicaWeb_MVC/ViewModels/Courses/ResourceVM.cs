﻿using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Courses
{
    public class ResourceVM
    {
        public int Id { get; set; }
        public int CourseChapterID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public ResourceType ResourceType { get; set; }
    }
}
