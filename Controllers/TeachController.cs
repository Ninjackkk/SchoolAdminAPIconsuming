using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAdminAPIconsuming.Data;
using SchoolAdminAPIconsuming.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectListItem

using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAdminAPIconsuming.Controllers
{
    public class TeachController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        public TeachController(ApplicationDbContext db)
        {
           this.db = db;
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        // GET: Teach/CreateAssignment
        public async Task<IActionResult> CreateAssignment()
        {
            var stdList = await db.STDs.ToListAsync();
            ViewBag.StdList = stdList;
            return View();
        }

        // POST: Teach/CreateAssignment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAssignment(AssignmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;

                if (viewModel.AssignmentFile != null && viewModel.AssignmentFile.Length > 0)
                {
                    var fileName = Path.GetFileName(viewModel.AssignmentFile.FileName);
                    filePath = Path.Combine(_uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.AssignmentFile.CopyToAsync(stream);
                    }

                    filePath = $"/uploads/{fileName}";
                }

                var assignment = new Assignment
                {
                    AssignmentName = viewModel.AssignmentName,
                    AssignmentDate = viewModel.AssignmentDate,
                    Deadline = viewModel.Deadline,
                    AssignmentFile = filePath,
                    GivenBy = viewModel.GivenBy,
                    StdName = viewModel.StdName // Adjusted to use StdName

                };

                db.Assignments.Add(assignment);
                await db.SaveChangesAsync();
                return RedirectToAction("CreateAssignment"); // Redirect to a suitable action after saving
            }

            // Re-populate dropdown if model state is invalid
            ViewBag.StdList = await db.STDs.ToListAsync();
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult LeaveRequest()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var teacherId = db.Teachers
                              .Where(t => t.UserId == userId)
                              .Select(t => t.TeacherId)
                              .FirstOrDefault();

            if (teacherId == 0)
            {
                return RedirectToAction("SignIn", "Account");
            }

            // Store the TeacherId in TempData for use in POST action
            TempData["TeacherId"] = teacherId;

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> LeaveRequest(LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                // Retrieve TeacherId from TempData
                int teacherId = int.Parse(TempData["TeacherId"].ToString());
                leaveRequest.TeacherId = teacherId;
                leaveRequest.Status = "Pending"; // Set status to Pending

                db.LeaveRequests.Add(leaveRequest);
                await db.SaveChangesAsync();

                TempData["SuccessMessage"] = "Applied for leave successfully";

                return RedirectToAction("ViewLeaveRequests");
                
            }

            // If model state is invalid, return the view with the current model
            return View(leaveRequest);
        }

        public async Task<IActionResult> ViewLeaveRequests()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var teacher = db.Teachers.FirstOrDefault(t => t.UserId == userId);

            if (teacher != null)
            {
                var leaveRequests = await db.LeaveRequests
                                             .Where(lr => lr.TeacherId == teacher.TeacherId)
                                             .ToListAsync();

                return View(leaveRequests);
            }

            return RedirectToAction("SignIn", "Account");
        }






        // GET: Teach/MarkAttendance
        public async Task<IActionResult> MarkAttendance()
        {
            // Get the logged-in teacher's UserId directly from session
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the teacher from the database based on UserId
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            // Fetch students in the teacher's STD
            var students = await db.Students.Where(s => s.STD == teacher.STD).ToListAsync();

            // Set the Date in ViewBag to pass to the view
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");

            return View(students); // Return the students to the view
        }

        // POST: Teach/MarkAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAttendance(List<int> presentStudentIds, string date)
        {
            DateTime attendanceDate = DateTime.Parse(date);

            // Get the logged-in teacher's UserId directly from session
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the teacher from the database based on UserId
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            // Fetch students in the teacher's STD
            var students = await db.Students.Where(s => s.STD == teacher.STD).ToListAsync();

            // Remove existing attendance records for the given date and STD
            var existingAttendances = await db.Attendances
                .Where(a => a.Date.Date == attendanceDate.Date && db.Students.Any(s => s.StudentId == a.StudentId && s.STD == teacher.STD))
                .ToListAsync();

            db.Attendances.RemoveRange(existingAttendances);

            // Record attendance for each student in the class
            foreach (var student in students)
            {
                var attendance = new Attendance
                {
                    StudentId = student.StudentId,
                    Date = attendanceDate,
                    IsPresent = presentStudentIds.Contains(student.StudentId) // Mark as present if in list
                };
                db.Add(attendance);
            }
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to attendance listing
        }

        public async Task<IActionResult> Index()
        {
            // Get the logged-in teacher's UserId directly from session
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the teacher from the database based on UserId
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            var attendanceList = await db.Attendances
                .Include(a => a.Student)
                .Where(a => a.Student.STD == teacher.STD) // Filter by teacher's STD
                .ToListAsync();

            return View(attendanceList); // Show attendance records for the teacher's STD
        }



        // GET: Teach/CreateTimetable
        public async Task<IActionResult> CreateTimetable()
        {
            var stdList = await db.STDs.ToListAsync();

            // Convert list of STD to list of SelectListItem
            ViewBag.StdList = new SelectList(stdList, "StdId", "StdName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTimetable(TimetableViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;

                if (viewModel.TimetableFile != null && viewModel.TimetableFile.Length > 0)
                {
                    var fileName = Path.GetFileName(viewModel.TimetableFile.FileName);
                    filePath = Path.Combine(_uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.TimetableFile.CopyToAsync(stream);
                    }

                    filePath = $"/timetables/{fileName}";
                }

                var timetable = new Timetable
                {
                    TimetableName = viewModel.TimetableName,
                    TimetableFile = filePath,
                    STD = viewModel.STD
                };

                db.Timetables.Add(timetable);
                await db.SaveChangesAsync();
                return RedirectToAction("CreateTimetable"); // Redirect to a suitable action after saving
            }

            // Re-populate dropdown if model state is invalid
            ViewBag.StdList = new SelectList(await db.STDs.ToListAsync(), "StdId", "StdName");
            return View(viewModel);
        }


    }
}


