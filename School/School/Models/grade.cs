using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class Grade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Grade")]
        public GradeEnum GradeNum { get; set; }

        [Required]
        [Display(Name ="Grade Group")]
        public GradesSeparatorEnum GradeLetter { get; set; }

        [Display(Name = "Grade Name")]
        public string GradeName { get; set; }

        [Display(Name = "Classroom")]
        public int ClassroomId { get; set; }

        [Display(Name = "Number of Students")]
        public int NumOfStudentsInClass { get; set; }

        [Display(Name ="Maximum Students Allowed")]
        public int MaxNumOfStudsInClass { get; set; }

        [Display(Name ="Teacher Id")]
        public int teacherId { get; set; }
    }
}