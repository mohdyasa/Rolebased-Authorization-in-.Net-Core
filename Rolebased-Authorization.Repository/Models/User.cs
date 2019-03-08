using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rolebased_Authorization.Repository.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(15, ErrorMessage ="Max 15 characters are allowed.")]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }

    }
}
