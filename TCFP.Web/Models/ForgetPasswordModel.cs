using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCFP.Web.Models
{
    public class ForgetPasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "E0001")]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.Label))]
        [EmailAddress(ErrorMessageResourceName = "E0001", ErrorMessageResourceType = typeof(Resources.Message))]
        public string Email { get; set; }
    }
}