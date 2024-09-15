using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For session handling
using SchoolAdminAPIconsuming.Data;
using SchoolAdminAPIconsuming.Models;
using System.Linq;
using System.Net.Mail;

namespace SchoolAdminAPIconsuming.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;

        public AccountController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignIn signIn,bool loginAsParent)
        {
            if (ModelState.IsValid)
            {
                object? data = null; 

                if (signIn.Role == "Admin")
                {
                    if (signIn.UserId == "admin" && signIn.Password == "admin")
                    {
                        HttpContext.Session.SetString("UserId", signIn.UserId);
                        HttpContext.Session.SetString("Role", signIn.Role);

                        return RedirectToAction("AddStudent", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Admin credentials.");
                    }
                }
                else if (signIn.Role == "Teacher")
                {
                    data = db.Teachers
                             .Where(t => t.UserId == signIn.UserId && t.Password == signIn.Password)
                             .FirstOrDefault();

                    if (data != null)
                    {
                        HttpContext.Session.SetString("UserId", signIn.UserId);

                        HttpContext.Session.SetString("Role", signIn.Role);
                        return RedirectToAction("CreateAssignment", "Teach");
                    }
                }
                else if (signIn.Role == "Student")
                {
                    data = db.Students
                             .Where(s => s.UserId == signIn.UserId && s.Password == signIn.Password)
                             .FirstOrDefault();

                    if (data != null && loginAsParent)
                    {
                        var student = data as Student;
                        if (student != null)
                        {
                            var otp = GenerateOtp();
                            SendOtpEmail(student.Parent_Email, otp);

                            TempData["Otp"] = otp;
                            TempData["StudentUserId"] = student.UserId;
                            TempData["StudentPassword"] = student.Password;
                            return RedirectToAction("OTP");
                        }
                    }
                }
                else if (signIn.Role == "Librarian")
                {
                    data = db.Librarians
                             .Where(l => l.UserId == signIn.UserId && l.Password == signIn.Password)
                             .FirstOrDefault();

                    if (data != null)
                    {
                        HttpContext.Session.SetString("UserId", signIn.UserId);
                        HttpContext.Session.SetString("Role", signIn.Role);
                        return RedirectToAction("AddStudent", "Admin");
                    }
                }
                else if (signIn.Role == "Accountant")
                {
                    data = db.Accountants
                             .Where(a => a.UserId == signIn.UserId && a.Password == signIn.Password)
                             .FirstOrDefault();

                    if (data != null)
                    {
                        HttpContext.Session.SetString("UserId", signIn.UserId);
                        HttpContext.Session.SetString("Role", signIn.Role);
                        return RedirectToAction("AddStudent", "Admin");
                    }
                }
                else if (signIn.Role == "SystemAdmin")
                {
                    data = db.SystemAdmins
                             .Where(sa => sa.UserId == signIn.UserId && sa.Password == signIn.Password)
                             .FirstOrDefault();

                    if (data != null)
                    {
                        HttpContext.Session.SetString("UserId", signIn.UserId);
                        HttpContext.Session.SetString("Role", signIn.Role);
                        return RedirectToAction("AddStudent", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid role selected.");
                }

                if (data == null && signIn.Role != "Admin") 
                {
                    ModelState.AddModelError("", "Invalid credentials.");
                }
            }
            return View(signIn);
        }


        public IActionResult OTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OTP(string enteredOtp)
        {
            if (TempData["Otp"] != null && TempData["Otp"].ToString() == enteredOtp)
            {
                HttpContext.Session.SetString("UserId", TempData["StudentUserId"].ToString());
                HttpContext.Session.SetString("Role", "Student");

                return RedirectToAction("AddStudent","Admin");
            }
            else
            {
                ModelState.AddModelError("", "Invalid OTP.");
                return View();
            }
        }
        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private void SendOtpEmail(string email, string otp)
        {
            var smtpServer = "smtp.gmail.com"; 
            var smtpPort = 587; 
            var smtpUsername = "forboringwork@gmail.com"; 
            var smtpPassword = "kfhestoyrzxrpxyc"; 
            var fromEmail = "forboringwork@gmail.com"; 

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otp}",
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Role");

            TempData.Clear();

            return RedirectToAction("SignIn");
        }













    }
}
