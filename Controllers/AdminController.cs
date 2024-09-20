using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchoolAdminAPIconsuming.Data;
using SchoolAdminAPIconsuming.Models;
using System.Net.Mail;
using System.Net;

namespace SchoolAdminAPIconsuming.Controllers
{
    public class AdminController : Controller
    {

        HttpClient client;                      //Declaring global object of HttpClient class 

        private readonly ApplicationDbContext db;


        public AdminController(ApplicationDbContext db) 
        {
            this.db = db;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);        // loading the client handler into client object
        }

        // student part

        // GET: /Admin/AddStudent
        public IActionResult AddStudent()
        {
            return View();
        }

        // POST: /Admin/AddStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            string url = "https://localhost:44355/api/Admin/AddStudent";
            var content = new StringContent(JsonConvert.SerializeObject(student), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return StatusCode((int)response.StatusCode);
        }


        public IActionResult Index()
        {
            List<Student> studlist = new List<Student>();                               

            string url = "https://localhost:44355/api/Admin/GetAllStudents";             //store the url of the concerned api fxn

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;       //Getting the data from the api result in formatted manner

                var obj = JsonConvert.DeserializeObject<List<Student>>(jsondata);  // we installed Newtonsoft.Json from tools to use this converter that gives us data in obj form from json 

                if (obj != null)
                {
                    studlist = obj;           //obj was local object that stored the data of emp , so we shifted it to global object emplist
                }
            }
            return View(studlist);
        }

        public async Task<IActionResult> DeleteStudent(int id)
        {
            string url = $"https://localhost:44355/api/Admin/DeleteStudent/{id}";

            HttpResponseMessage response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return StatusCode((int)response.StatusCode);
        }

        public async Task<IActionResult> EditStudent(int id)
        {
            string url = $"https://localhost:44355/api/Admin/GetStudentById/{id}";
        
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(jsondata);

                if (student != null)
                {
                    return View(student);
                }
            }

            return NotFound();
        }

        // POST: Admin/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            string url = $"https://localhost:44355/api/Admin/UpdateStudent/{student.StudentId}";
            var content = new StringContent(JsonConvert.SerializeObject(student), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return StatusCode((int)response.StatusCode);
        }


        //Teacher Part

        public IActionResult AddTeacher()
        {
            return View();
        }

        // POST: /Admin/AddTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return View(teacher);
            }

            string url = "https://localhost:44355/api/Admin/AddTeacher";
            var content = new StringContent(JsonConvert.SerializeObject(teacher), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return StatusCode((int)response.StatusCode);
        }

        // GET: /Admin/EditTeacher/5
        public async Task<IActionResult> EditTeacher(int id)
        {
            string url = $"https://localhost:44355/api/Admin/GetTeacherByID/{id}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var teacher = JsonConvert.DeserializeObject<Teacher>(jsondata);

                if (teacher != null)
                {
                    return View(teacher);
                }
            }

            return NotFound();
        }

        // POST: /Admin/EditTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return View(teacher);
            }

            string url = $"https://localhost:44355/api/Admin/UpdateTeacher/{teacher.TeacherId}";
            var content = new StringContent(JsonConvert.SerializeObject(teacher), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return StatusCode((int)response.StatusCode);
        }

        // GET: /Admin/DeleteTeacher/5
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            string url = $"https://localhost:44355/api/Admin/DeleteTeacher/{id}";

            HttpResponseMessage response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return StatusCode((int)response.StatusCode);
        }

        // GET: /Admin/TeacherList
        public async Task<IActionResult> TeacherList()
        {
            List<Teacher> teacherList = new List<Teacher>();

            string url = "https://localhost:44355/api/Admin/GetAllTeachers"; // You'll need to add this endpoint in your API

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<List<Teacher>>(jsondata);

                if (obj != null)
                {
                    teacherList = obj;
                }
            }

            return View(teacherList);
        }


        // Online Applications



        public IActionResult OnlineApplication()
        {
            return View();
        }

        // POST: /Admin/OnlineApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnlineApplication(OnlineApplication application)
        {
            if (!ModelState.IsValid)
            {
                return View(application);
            }

            db.OnlineApplications.Add(application);
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "You will receive an email shortly for further process.";

            return RedirectToAction("SignIn","Account");
        }


        public async Task<IActionResult> ViewApplications()
        {
            var applications = await db.OnlineApplications.ToListAsync();
            return View(applications);
        }

        // POST: /Admin/ApproveApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveApplication(int id)
        {
            var application = await db.OnlineApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Send approval email
            await SendEmail(
                application.Parent_Email,
                "Application Approved",
                "Your application has been approved. We will contact you soon for a scheduled interview.");

            db.OnlineApplications.Remove(application);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ViewApplications));
        }

        // POST: /Admin/RejectApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectApplication(int id)
        {
            var application = await db.OnlineApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Send rejection email
            await SendEmail(
                application.Parent_Email,
                "Application Rejected",
                "We regret to inform you that your application has been rejected.");

            db.OnlineApplications.Remove(application);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ViewApplications));
        }

        private async Task SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("forboringwork@gmail.com", "kfhestoyrzxrpxyc"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("forboringwork@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }


        public async Task<IActionResult> ViewLeaveRequests()
        {
            var leaveRequests = await db.LeaveRequests.ToListAsync();
            return View(leaveRequests);
        }

        // POST: /Admin/ApproveLeaveRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            leaveRequest.Status = "Approved";
            db.LeaveRequests.Update(leaveRequest);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Leave request approved successfully.";
            return RedirectToAction(nameof(ViewLeaveRequests));
        }

        // POST: /Admin/RejectLeaveRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            leaveRequest.Status = "Rejected";
            db.LeaveRequests.Update(leaveRequest);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Leave request rejected successfully.";
            return RedirectToAction(nameof(ViewLeaveRequests));
        }




        public IActionResult Create()
        {
            return View();
        }

        // POST: Librarian/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Librarian librarian)
        {
            db.Librarians.Add(librarian);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Create));  // Redirect to the list of librarians

            //return View(librarian);
        }








    }




}








