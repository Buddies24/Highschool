using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class StudentGradeSubjectHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        [Display(Name = "Grade Subject Id")]
        public int GradeSubectId { get; set; }

        [Display(Name = "Student Grade")]
        public GradeEnum StudentGrade { get; set; }

        [Display(Name = "Subject Grade")]
        public GradeEnum SubjectGrade { get; set; }

        public SubjectStatusEnum SubjectStatus { get; set; }

        public int StudentMark { get; set; }

    }

}