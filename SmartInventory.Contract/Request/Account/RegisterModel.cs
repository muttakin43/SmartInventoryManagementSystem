using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request.Account
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }= string.Empty;
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }= string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; }= string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
