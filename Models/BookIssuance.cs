using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class BookIssuance
    {
        public int BookIssuanceId { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        // For issuing to Student or Teacher based on UserId
        public string UserId { get; set; }
        public Teacher Teacher { get; set; }  // Could be null if issued to student

        public int StudentId { get; set; }  // Could be null if issued to teacher
        public Student Student { get; set; }

        [Required]
        public DateTime IssuedOn { get; set; }

        // Set the DueDate as 14 days from the issue date
        public DateTime DueDate
        {
            get
            {
                return IssuedOn.AddDays(14);  // Default due date is 14 days from issued date
            }
        }

        public DateTime? ReturnedOn { get; set; }

        [NotMapped]
        public decimal LateFee
        {
            get
            {
                if (ReturnedOn.HasValue && ReturnedOn.Value > DueDate)
                {
                    // Calculate late fee if the book is returned after the due date
                    var daysLate = (ReturnedOn.Value - DueDate).Days;
                    return daysLate * 10;  // 10 Rs for each day late
                }
                return 0;
            }
        }

    }
}
