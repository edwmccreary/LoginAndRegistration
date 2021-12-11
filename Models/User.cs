using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginAndRegistration.Models
{
    public class User
    {
        [Key]
        public int userId {get;set;}
        [Required]
        [MinLength(2,ErrorMessage = "First name must be greated than 1 character in length")]
        public string firstname {get;set;}
        [Required]
        [MinLength(2,ErrorMessage = "Last name must be greated than 1 character in length")]
        public string lastname {get;set;}

        [Required]
        [EmailAddress]
        public string email {get;set;}

        [Required]
        [MinLength(8,ErrorMessage = "Password must be atleast 8 characters in length")]
        [DataType(DataType.Password)]
        public string password {get;set;}

        public DateTime createdAt {get;set;} = DateTime.Now;
        public DateTime updatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirm {get;set;}

    }
}