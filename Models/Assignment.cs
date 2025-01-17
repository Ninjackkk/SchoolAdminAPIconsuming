﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string AssignmentName { get; set; }

        [Required]
        public DateTime AssignmentDate { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public string AssignmentFile { get; set; }  // Path or URL to the file

        [Required]
        public string GivenBy { get; set; }  // Teacher's name

        public string StdName { get; set; }  // Student's name

    }
}
