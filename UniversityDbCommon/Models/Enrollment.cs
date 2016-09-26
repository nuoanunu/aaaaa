using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityDbCommon.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public double Quiz1 { get; set; }
        public double Quiz2 { get; set; }
        public double Quiz3 { get; set; }
        public double Midterm { get; set; }
        public double Project { get; set; }
        public double Final { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
