using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDbCommon.Models;

namespace UniversityDbCommon.DAL
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext() : base("UniversityDbContext")
        {
        }

        public UniversityDbContext(string connString) : base(connString)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
