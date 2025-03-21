﻿namespace Core.Entities.Identity
{
    public class Employee : ApplicationUser
    {
        //1 Employee - 1 Department
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        //1 Employee - 1 User
        public virtual ApplicationUser User { get; set; }

    }
}