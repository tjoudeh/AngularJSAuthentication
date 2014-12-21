using System;
using System.ComponentModel.DataAnnotations;
using AngularJSAuthentication.Data.Attributes;
using AngularJSAuthentication.Data.Infrastructure;

namespace AngularJSAuthentication.Data.Entities
{
    [CollectionName("refreshTokens")]
    public class RefreshToken : IEntity<string>
    {
        [Key]
        public string Id { get; set; }
       
        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; }
        
        public DateTime IssuedUtc { get; set; }
        
        public DateTime ExpiresUtc { get; set; }
        
        [Required]
        public string ProtectedTicket { get; set; }
    }
}