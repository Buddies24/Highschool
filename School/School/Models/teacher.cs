using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Personal information

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Display(Name = "Middle Name")]
        public string MidName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "ID Number")]
        [Required]
        [MaxLength(13)]
        [MinLength(13)]
        [Range(13, Int64.MaxValue, ErrorMessage = "ID Number should not contain charecters and must be 13 digits")]
        public string IdNum { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [Range(10, Int64.MaxValue, ErrorMessage = "Phone number should not contain charecters and must be 10 digits")]
        [Display(Name = "Phone Number")]
        public string phone { get; set; }


        [Display(Name = "Grade Id")]
        public int gradeId { get; set; }

        [Display(Name = "Subject Id")]
        public int SubjectId { get; set; }



        #endregion
    }
}