using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class Stream
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Stream Name")]
        public string StreamName { get; set; }
        /// <summary>
        /// separate id's by pipe
        /// </summary>

        [Display(Name = "Stream Subjects")] 
        public string StreamSubjects { get; set; }
        /// <summary>
        /// used if student was in grade 9 only
        /// separate id's by pipe
        /// </summary>
        [Display(Name="Pre Subjects")]////
        public string PreCompulsorySubjects { get; set; }

    }
}