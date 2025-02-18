using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantCopy.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The username can only contain letters and numbers.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and include at least one lowercase letter, one uppercase letter, and one digit.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Name is mandatory")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets for Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This is not a valid email address")]
        [EmailAddress(ErrorMessage = "This is not a valid email address")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "This is not a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact is mandatory")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string Phone {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}