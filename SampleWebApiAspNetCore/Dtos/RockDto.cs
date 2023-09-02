namespace SampleWebApiAspNetCore.Dtos
{
    public class RockDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int Hardness { get; set; }
        public string? Luster { get; set; }
        public DateTime Created { get; set; }
    }
}
