using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolAdminAPIconsuming.Models;

namespace SchoolAdminAPIconsuming.Controllers
{
    public class AdminController : Controller
    {
        HttpClient client;                      //Declaring global object of HttpClient class 

        public AdminController() 
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);        // loading the client handler into client object
        }


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
            List<Student> emplist = new List<Student>();                                //creating list of Emp to store empdetails

            string url = "https://localhost:44355/api/Admin/GetAllStudents";             //store the url of the concerned api fxn

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;       //Getting the data from the api result in formatted manner

                var obj = JsonConvert.DeserializeObject<List<Student>>(jsondata);  // we installed Newtonsoft.Json from tools to use this converter that gives us data in obj form from json 

                if (obj != null)
                {
                    emplist = obj;           //obj was local object that stored the data of emp , so we shifted it to global object emplist
                }
            }
            return View(emplist);
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






















    }
}
