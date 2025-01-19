using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sample_1.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sample_1.Pages
{
    public class CoursesModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CoursesModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Course_Details> Courses { get; set; } = new List<Course_Details>();

        private (int Year, int Semester)? GetLoggedInUserYearAndSemester(string registerNo)
        {
            var student = _db.Student_Details
                .FirstOrDefault(s => s.RegisterNo.Trim() == registerNo.Trim());

            if (student != null)
            {
                return (student.Year, student.Semester);
            }

            return null;
        }

        public IList<Course_Details> RelevantCourses { get; set; } = new List<Course_Details>();

        public void OnGet()
        {
            var yearClaim = User.Claims.FirstOrDefault(c => c.Type == "Year")?.Value;
            var semesterClaim = User.Claims.FirstOrDefault(c => c.Type == "Semester")?.Value;
            Console.WriteLine($"Year: {yearClaim}, Semester: {semesterClaim}");

            if (int.TryParse(yearClaim, out int year) && int.TryParse(semesterClaim, out int semester))
            {
                Console.WriteLine($"Parsed Year: {year}, Parsed Semester: {semester}");
                // Query relevant courses based on year and semester
                RelevantCourses = _db.Course_Details
                                    .Where(c => c.Year == year && c.Semester == semester)
                                    .ToList();
                Console.WriteLine($"Courses Found: {RelevantCourses.Count}");
            }
            else
            {
                Console.WriteLine("Failed to find it");
            }
        }
        
    }
}
