using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admission_task.Models
{
    public class ModelCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? ID { get; set; }
        [Required]  
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? Model { get; set; }
        public string? Photo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
