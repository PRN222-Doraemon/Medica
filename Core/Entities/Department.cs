﻿using Core.Entities.Identity;

namespace Core.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}