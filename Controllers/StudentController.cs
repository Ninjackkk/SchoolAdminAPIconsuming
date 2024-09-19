using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolAdminAPIconsuming.Models;

namespace SchoolAdminAPIconsuming.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly HttpClient _client;

        public StudentController(IWebHostEnvironment webHostEnvironment)
        {
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



        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(AssignmentResponseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Save the file to the uploads folder
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

            // Prepare the assignment response for submission
            var assignmentResponse = new
            {
                model.AssignmentName,
                model.AssignmentDate,
                model.Deadline,
                SubmittedOn = DateTime.Now, // Set this in the API
                model.GivenBy,
                model.StdName,
                model.StudentId,
                AssignmentFile = model.SolutionFile.FileName // Store just the filename
            };

            // Serialize and send the response to the API
            var jsonContent = JsonConvert.SerializeObject(assignmentResponse);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Call the API to submit the assignment response
            var response = await _client.PostAsync("https://localhost:44355/api/Student/SubmitAssignment", content);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Assignment submitted successfully.";
                return View();
            }

            ViewBag.Message = "Error submitting assignment.";
            return View(model);
        }













    }

}

