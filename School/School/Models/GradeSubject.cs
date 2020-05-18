using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class GradeSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required]
        [Display(Name = "Minimum Pass Mark")]
        public int MinPassMark { get; set; }

        [Required]
        [Display(Name = "Grade")]
        public GradeEnum GradeNumber { get; set; }

        [Display(Name = "Teacher")]
        public int teacherId { get; set; }

        [Display(Name = "Required At Streams (list)")]
        public string ListOfStreamsIdRequiredAt { get; set; }
    }
}