using System;
using System.ComponentModel.DataAnnotations;

namespace LoginAndRegistration.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "You must enter a valid email")]
        [EmailAddress]
        public string login_email {get;set;}

        [Required(ErrorMessage = "You must enter a valid password")]
        [DataType(DataType.Password)]
        public string login_password {get;set;}

    }
}