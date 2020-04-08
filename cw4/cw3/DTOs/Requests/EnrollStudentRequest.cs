using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "Podaj poprawne Imie")]
        [MaxLength(10)]
        public string FirstName { get; set; }

       // [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

       // [Required]
        public string BirthDate { get; set; }


       // [Required]
        public string Studies { get; set; }
    }
}
