using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchoolAdminAPIconsuming.Data;
using SchoolAdminAPIconsuming.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SchoolAdminAPIconsuming.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext db;
        private readonly HttpClient _client;

        public StudentController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            _client = new HttpClient(clientHandler);
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> ViewAttendance()
        {

            var studentId = HttpContext.Session.GetString("StudentId");

            string url = $"https://localhost:44355/api/Student/GetAttendance?studentId={studentId}";

            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var attendances = JsonConvert.DeserializeObject<List<Attendance>>(jsonResponse);
                return View(attendances);
            }

            return View(new List<Attendance>());
        }


        public async Task<IActionResult> ViewAssignment()
        {
            // Retrieve the STD from the session
            var studentStdName = HttpContext.Session.GetString("studentstd");

            if (string.IsNullOrEmpty(studentStdName))
            {
                // Redirect to SignIn if STD is missing
                return RedirectToAction("SignIn", "Account");
            }

            // Construct the URL to call the API endpoint
            string url = $"https://localhost:44355/api/Student/GetAssignmentByStd?stdName={studentStdName}";

            // Call the API to get the assignments
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Read the JSON response and deserialize it to a list of assignments
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var assignments = JsonConvert.DeserializeObject<List<Assignment>>(jsonResponse);

                // Ensure the file URLs are correct
                foreach (var assignment in assignments)
                {
                    if (!string.IsNullOrEmpty(assignment.AssignmentFile))
                    {
                        assignment.AssignmentFile = Url.Action("DownloadFile", new { filePath = Path.GetFileName(assignment.AssignmentFile) });
                    }
                }

                // Pass the assignments to the view
                return View(assignments);
            }

            // If the response is not successful, return an empty list or handle as needed
            return View(new List<Assignment>());
        }

        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(); // Or handle as needed
            }

            var fileName = Path.GetFileName(filePath);
            var fileFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

            if (!System.IO.File.Exists(fileFullPath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(fileFullPath);
            return File(fileBytes, "application/octet-stream", fileName);
        }


        // assignment submission remaining



        public async Task<IActionResult> SubmitAssignment(AssignmentResponseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, model.SolutionFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.SolutionFile.CopyToAsync(stream);
            }

            var assignmentResponse = new
            {
                model.AssignmentName,
                model.AssignmentDate,
                model.Deadline,
                SubmittedOn = DateTime.Now,
                model.GivenBy,
                model.StdName,
                model.StudentId,
                SolutionFile = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", model.SolutionFile.FileName),
                Score = 0
            };

            var jsonContent = JsonConvert.SerializeObject(assignmentResponse);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://localhost:44355/api/Student/SubmitAssignment", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["IsSubmitted"] = true; // Set a flag for successful submission
                return RedirectToAction("ViewAssignment");
            }

            ModelState.AddModelError(string.Empty, "Error submitting assignment.");
            return View(model);
        }



        public async Task<IActionResult> ViewTimetable()
        {
            var studentStdName = HttpContext.Session.GetString("studentstd");

            if (string.IsNullOrEmpty(studentStdName))
            {
                return RedirectToAction("SignIn", "Account");
            }

            // Fetch timetables from the API
            string url = $"https://localhost:44355/api/Student/GetTimetableByStd?stdName={studentStdName}";

            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var timetables = JsonConvert.DeserializeObject<List<Timetable>>(jsonResponse);

                // Ensure the file paths are correct
                foreach (var timetable in timetables)
                {
                    if (!string.IsNullOrEmpty(timetable.TimetableFile))
                    {
                        timetable.TimetableFile = Url.Content("~/uploads/" + Path.GetFileName(timetable.TimetableFile));
                    }
                }

                return View(timetables);
            }

            return View(new List<Timetable>());
        }


        public async Task<IActionResult> ViewFees()
        {
            var studentId = HttpContext.Session.GetString("StudentId");

            if (string.IsNullOrEmpty(studentId))
            {
                return RedirectToAction("SignIn", "Account");
            }

            // Fetch the student record from the database
            var student = await db.Students.FirstOrDefaultAsync(s => s.StudentId.ToString() == studentId);

            if (student == null)
            {
                ModelState.AddModelError("", "Student not found.");
                return View();
            }

            // Fetch the STD record based on the student's standard
            var std = await db.STDs.FirstOrDefaultAsync(s => s.StdName == student.STD);

            if (std == null)
            {
                ModelState.AddModelError("", "Standard not found.");
                return View();
            }

            // Create a view model to hold the fee information
            var feesViewModel = new FeesViewModel
            {
                StudentName=student.StudentName,
                StdName = std.StdName,
                AnnualFees = std.AnnualFees,
                FeesStatus = student.FeesStatus
            };

            return View(feesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PayFees()
        {
            var studentId = HttpContext.Session.GetString("StudentId");
            var student = await db.Students.FindAsync(int.Parse(studentId));

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var std = await db.STDs.FirstOrDefaultAsync(s => s.StdName == student.STD);
            if (std == null)
            {
                return NotFound("Standard not found.");
            }

            double amount = std.AnnualFees ?? 0;

            // Create a Razorpay order
            var keyId = "rzp_test_Kl7588Yie2yJTV"; // Your Razorpay Key ID
            var keySecret = "6dN9Nqs7M6HPFMlL45AhaTgp"; // Your Razorpay Key Secret

            using (var client = new HttpClient())
            {
                var orderData = new
                {
                    amount = amount * 100, // Amount in paise
                    currency = "INR",
                    receipt = Guid.NewGuid().ToString(),
                    payment_capture = 1 // Auto capture
                };

                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{keyId}:{keySecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var response = await client.PostAsJsonAsync("https://api.razorpay.com/v1/orders", orderData);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var orderResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    string orderId = (string)orderResponse.id;

                    return Json(new { orderId, keyId });
                }
            }

            return BadRequest("Failed to create order");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeesStatus([FromBody] PaymentInfo paymentInfo)
        {
            var studentId = HttpContext.Session.GetString("StudentId");
            var student = await db.Students.FindAsync(int.Parse(studentId));

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Update FeesStatus to "Paid"
            student.FeesStatus = "Paid";
            await db.SaveChangesAsync();

            return Json(new { success = true });
        }

        public class PaymentInfo
        {
            public string PaymentId { get; set; }
        }




    }

}

