using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        public string UserName { get; set; }

         [Required]
        public string Provider { get; set; }

         [Required]
         public string ExternalAccessToken { get; set; }

    }

    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
}