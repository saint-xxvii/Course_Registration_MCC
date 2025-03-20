using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sample_1.Data;
using sample_1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample_1.Pages
{
    public class EnrollModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly EmailService _emailService;

        public EnrollModel(ApplicationDbContext db, EmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        [BindProperty]
        public string RegNo { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public int SelectedCourseId { get; set; } // Store the selected CourseId

        public IList<Course_Details> Courses { get; set; } // List of courses to display

        public void OnGet()
        {
            // Retrieve year and semester from claims
            var yearClaim = User.Claims.FirstOrDefault(c => c.Type == "Year")?.Value;
            var semesterClaim = User.Claims.FirstOrDefault(c => c.Type == "Semester")?.Value;

            if (int.TryParse(yearClaim, out int year) && int.TryParse(semesterClaim, out int semester))
            {
                // Query relevant courses based on year and semester
                Courses = _db.Course_Details
                             .Where(c => c.Year == year && c.Semester == semester)
                             .ToList();
            }
            else
            {
                Courses = new List<Course_Details>(); // No courses if claims are invalid
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Use the registration number entered by the user
            var registerNo = RegNo;

            // Check if the selected CourseId is valid
            if (SelectedCourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid course selection.";
                return Page();
            }

            // Create a new registration entry
            var registeredCourse = new Registered_Course
            {
                RegisterNo = registerNo,
                CourseId = SelectedCourseId,
                EnrollmentDate = DateTime.Now // Set the current date and time
            };

            // Add the registered course to the database
            _db.Registered_Course.Add(registeredCourse);
            await _db.SaveChangesAsync();

            // Send Confirmation Email
            var course = _db.Course_Details.FirstOrDefault(c => c.CourseId == SelectedCourseId);
            var subject = "Course Registration Confirmation";
            var body = $@"
                <h2>Course Enrollment Confirmation</h2>
                <p>Dear Student,</p>
                <p>You have successfully enrolled in the course: <strong>{course?.CourseName}</strong>.</p>
                <p>We appreciate your interest in learning and wish you success!</p>
                <p>Regards,<br>College Admin</p>
            ";

            await _emailService.SendEmailAsync(Email, subject, body);

            TempData["SuccessMessage"] = "You have successfully enrolled in the course.";
            return RedirectToPage("/Courses"); // Redirect to the courses page or another page
        }
    }
}