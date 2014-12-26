﻿using System.ComponentModel.DataAnnotations;
using AngularJSAuthentication.Data.Attributes;
using AngularJSAuthentication.Data.Infrastructure;
using AngularJSAuthentication.Data.Models;

namespace AngularJSAuthentication.Data.Entities
{
    [CollectionName("clients")]
    public class Client : IEntity<string>
    {
        [Key] 
        public string Id { get; set; }
       
        [Required]
        public string Secret { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public ApplicationTypes ApplicationType { get; set; }
        
        public bool Active { get; set; }
        
        public int RefreshTokenLifeTime { get; set; }
        
        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
    }
}