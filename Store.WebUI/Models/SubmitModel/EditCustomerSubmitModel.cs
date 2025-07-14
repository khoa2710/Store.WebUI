using System.ComponentModel.DataAnnotations;

namespace Store.WebUI.Models.SubmitModel
{
    public class EditCustomerSubmitModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        public DateTimeOffset? EditAt { get; set; }
    }
}
