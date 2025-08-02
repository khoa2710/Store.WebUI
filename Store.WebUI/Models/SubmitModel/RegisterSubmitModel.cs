using System.ComponentModel.DataAnnotations;

namespace Store.WebUI.Models.SubmitModel
{
    public class RegisterSubmitModel
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? FullName { get; set; }
        //==
        [Required]
        public string? AvataUrl { get; set; }
    }
}
