namespace SchoolAdminAPIconsuming.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }  // For storing the book's image
    }
}
