using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Users
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please enter your last name")]
        public string UserFirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your first name")]
        public string UserLastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter a email")]
        public string UserEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter a username")]
        public string UserUserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter a password")]
        public string UserPassword { get; set; } = string.Empty;
    }
}
