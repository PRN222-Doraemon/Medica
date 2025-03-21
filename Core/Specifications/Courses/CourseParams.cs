﻿using Core.Entities;

namespace Core.Specifications.Courses
{
    public class CourseParams : PagingParams
    {
        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value?.ToLower() ?? string.Empty;
        }
        public int? CategoryID { get; set; }
        public CourseStatus? Status { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
