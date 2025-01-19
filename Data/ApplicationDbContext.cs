using Microsoft.EntityFrameworkCore;

namespace sample_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student_Details> Student_Details { get; set; }
        public DbSet<Course_Details> Course_Details { get; set; }
        public DbSet<Registered_Course> Registered_Course { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary key for Course_Details
            modelBuilder.Entity<Course_Details>()
                .HasKey(c => c.CourseId);

            // Define primary key for Registered_Course
            modelBuilder.Entity<Registered_Course>()
                .HasKey(r => r.Id);

            // Relation between Registered_Course and Student_Details
            modelBuilder.Entity<Registered_Course>()
                .HasOne(r => r.Student)
                .WithMany(s => s.RegisteredCourses)
                .HasForeignKey(r => r.RegisterNo)
                .HasPrincipalKey(s => s.RegisterNo)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior

            // Relation between Registered_Course and Course_Details
            modelBuilder.Entity<Registered_Course>()
                .HasOne(r => r.Course)
                .WithMany(c => c.RegisteredCourses)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior

            // Ensure RegisterNo is required
            modelBuilder.Entity<Registered_Course>()
                .Property(r => r.RegisterNo)
                .IsRequired();
        }
    }

    public class Course_Details
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Semester { get; set; }
        public string CourseDescription { get; set; }
        public int Year { get; set; }
        public int Availability { get; set; }

        // Navigation property
        public ICollection<Registered_Course> RegisteredCourses { get; set; }
    }

    public class Registered_Course
    {
        public int Id { get; set; }
        public required string RegisterNo { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        // Navigation properties
        public Student_Details Student { get; set; }
        public Course_Details Course { get; set; }
    }

    public class Student_Details
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegisterNo { get; set; }
        public string Dob { get; set; }
        public string Dept { get; set; }
        public string Section { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }

        // Navigation property
        public ICollection<Registered_Course> RegisteredCourses { get; set; }
    }
}