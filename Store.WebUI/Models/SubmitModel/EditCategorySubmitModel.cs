using System.ComponentModel.DataAnnotations;

namespace Store.WebUI.Models.SubmitModel
{
    public class EditCategorySubmitModel
    {
        [Required(ErrorMessage = "Cannot be null")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Cannot be null")]

        public string? Description { get; set; }
        [Required(ErrorMessage = "Cannot be null")]

        public string? Icon { get; set; }
        [Required(ErrorMessage = "Cannot be null")]

        public string? Slug { get; set; }
        [Required(ErrorMessage = "Cannot be null")]

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
        public DateTimeOffset? EditAt { get; set; }
    }
}
