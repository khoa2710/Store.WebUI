namespace Store.WebUI.Models.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public string? Icon { get; set; }
        public string? Slug { get; set; }
        public DateTimeOffset? CreateAt { get; set; }
    }
}
