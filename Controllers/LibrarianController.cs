using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAdminAPIconsuming.Data;
using SchoolAdminAPIconsuming.Models;
using Microsoft.EntityFrameworkCore;


namespace SchoolAdminAPIconsuming.Controllers
{
    public class LibrarianController : Controller

    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LibrarianController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //private string GetCurrentUserId()
        //{
        //    return HttpContext.Session.GetString("UserId");
        //}

        // GET: Librarian/AddBook
        public IActionResult AddBook()
        {
            return View();
        }

        // POST: Librarian/AddBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(Book book, IFormFile BookImage)
        {


            if (BookImage != null && BookImage.Length > 0)
            {

                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/books");


                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }


                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(BookImage.FileName);
                string filePath = Path.Combine(uploadFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await BookImage.CopyToAsync(fileStream);
                }

                book.ImagePath = "/images/books/" + uniqueFileName;
            }
            else
            {
                ModelState.AddModelError("", "Please upload a book image.");
                return View(book);
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookList));


            //return View(book);



        }

        // GET: Librarian/BookList
        public IActionResult BookList()
        {
            var books = _context.Books.ToList();
            return View(books);
        }



        // GET: IssueBook
        public async Task<IActionResult> IssueBook()
        {
            // Fetch books, teachers, and students for the dropdown lists
            ViewBag.Books = new SelectList(await _context.Books.ToListAsync(), "BookId", "Title");
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "UserId", "FirstName");
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "UserId", "StudentName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IssueBook(int bookId, string userId, string userType, DateTime issuedOn)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                ModelState.AddModelError("", "Invalid book selected.");
                return View();
            }

            Teacher teacher = null;
            Student student = null;

            // Check if the user is a teacher or student based on the selected userType
            if (userType == "Teacher")
            {
                teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
                if (teacher == null)
                {
                    ModelState.AddModelError("", "Invalid teacher selected.");
                    return View();
                }
            }
            else if (userType == "Student")
            {
                student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                if (student == null)
                {
                    ModelState.AddModelError("", "Invalid student selected.");
                    return View();
                }
            }

            // Create the BookIssuance record, ensuring the correct user is assigned
            var bookIssuance = new BookIssuance
            {
                BookId = bookId,
                UserId = userId,
                Teacher = teacher,  // Assign teacher if selected
                Student = student,  // Assign student if selected
                IssuedOn = issuedOn
            };

            // Add the issuance record to the database
            _context.BookIssuances.Add(bookIssuance);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IssuedBooksList));  // Redirect to the issued books list
        }




        // List of issued books
        public async Task<IActionResult> IssuedBooksList()
        {
            var issuedBooks = await _context.BookIssuances
                                             .Include(b => b.Book)
                                             .Include(b => b.Teacher)
                                             //.Include(b => b.Student)
                                             .ToListAsync();
            return View(issuedBooks);
        }

    }
}
