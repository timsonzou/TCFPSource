using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace TCFP.Web.Models
{
    public class RegisterAccount
    {
        [Required(ErrorMessageResourceType =typeof(Resources.Message),ErrorMessageResourceName ="E0001")]
        [Display(Name ="EmailAddress", ResourceType =typeof(Resources.Label))]
        [EmailAddress(ErrorMessageResourceName = "E0001", ErrorMessageResourceType = typeof(Resources.Message))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "E0003")]
        [Display(Name = "Name")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "E0004")]
        public string Name { get; set; }
    }
}