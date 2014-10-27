using System;
using System.ComponentModel.DataAnnotations;

namespace AngularJSAuthentication.API.Entities
{
    public class RefreshToken
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; }

        [Required]
        public DateTime ExpiresUtc { get; set; }
    }
}