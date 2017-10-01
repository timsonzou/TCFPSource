using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace TCFP.Web.Models
{
    public class NewPassword
    {
        [DataType(DataType.Password)]
        [StringLength(16,ErrorMessageResourceType =typeof(Resources.Message),ErrorMessageResourceName = "E0005",MinimumLength =8)]
        [Required]
        [Display(ResourceType =typeof(Resources.Label),Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "E0005", MinimumLength = 8)]
        [Required]
        [Compare(otherProperty: "Password", ErrorMessageResourceType =typeof(Resources.Message), ErrorMessageResourceName ="E0007")]
        [Display(ResourceType = typeof(Resources.Label), Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}