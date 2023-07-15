using System.ComponentModel.DataAnnotations;

namespace admission_task.Dtos
{
    public class ModelDto
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Model { get; set; }
        public string? Photo { get; set; }
    }
    public class CreatedModelDto
    {
        public string? ID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Model { get; set; }
        public AppFileDto? img { get; set; }

        //  public IFormFile? Photo { get; set; }
    }
}
