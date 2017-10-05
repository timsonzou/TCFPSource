using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace TCFP.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "E0001")]
        [Display(Name = "EmailAddress", ResourceType =typeof(Resources.Label))]
        [EmailAddress(ErrorMessageResourceName = "E0001", ErrorMessageResourceType = typeof(Resources.Message))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(ResourceType = typeof(Resources.Label), Name = "Password")]
        public string Password { get; set; }
    }
}