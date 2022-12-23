using System.ComponentModel.DataAnnotations;

namespace ASM2.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select role")]
        [Display(Name = "Role")]
        public string Role { get; set; }

    }
}
