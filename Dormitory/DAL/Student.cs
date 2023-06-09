﻿using System;
using System.Collections.Generic;

namespace Dormitory.DAL
{
    public partial class Student
    {
        public Student()
        {
            Applications = new HashSet<Application>();
            RoomStudents = new HashSet<RoomStudent>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<RoomStudent> RoomStudents { get; set; }
    }
}
