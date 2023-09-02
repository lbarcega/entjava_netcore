
namespace SampleWebApiAspNetCore.Dtos
{
    public class RockUpdateDto
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int Hardness { get; set; }
        public string? Luster { get; set; }
        public DateTime Created { get; set; }
    }
}
