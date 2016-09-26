using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDbCommon.Models;

namespace UniversityDbCommon.DAL
{
    class UniversityDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<UniversityDbContext>
    {
        Random rnd = new Random();

        protected override void Seed(UniversityDbContext context)
        {
            var students = new List<Student>
            {
            new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01"),
                Email ="CarsonAlexander@gmail.com"},
            new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01"),
                Email ="MeredithAlonso@gmail.com"},
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01"),
                Email ="ArturoAnand@gmail.com"},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01"),
                Email ="GytisBarzdukas@gmail.com"},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01"),
                Email ="YanLi@gmail.com"},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01"),
                Email ="PeggyJustice@gmail.com"},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01"),
                Email ="LauraNorman@gmail.com"},
            new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01"),
                Email ="NinoOlivetto@gmail.com"}
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var courses = new List<Course>
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3,},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
            new Course{CourseID=1045,Title="Calculus",Credits=4,},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
            new Course{CourseID=2021,Title="Composition",Credits=3,},
            new Course{CourseID=2042,Title="Literature",Credits=4,}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
            new Enrollment{StudentID=1,CourseID=1050,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=1,CourseID=4022,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=1,CourseID=4041,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=2,CourseID=1045,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=2,CourseID=3141,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=2,CourseID=2021,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=3,CourseID=1050,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=4,CourseID=1050,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=4,CourseID=4022,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=5,CourseID=4041,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Quiz1=RndScr(),Quiz2=RndScr(),Quiz3=RndScr(),
                Midterm =RndScr(), Project=RndScr(),Final=RndScr()},
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();
        }

        private double RndScr()
        {
            return rnd.Next(0, 101) * 0.1;
        }
    }
}
