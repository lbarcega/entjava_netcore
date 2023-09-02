using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class RockCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int Hardness { get; set; }
        public string? Luster { get; set; }
        public DateTime Created { get; set; }
    }
}
