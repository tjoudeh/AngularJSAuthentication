using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AngularJSAuthentication.Data.Attributes;
using AngularJSAuthentication.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularJSAuthentication.Data.Entities
{
    [CollectionName("users")]
    [BsonIgnoreExtraElements(true)]
    public class User : IEntity<string>, IUser
    {
        [Key]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("userName")]
		public virtual string UserName { get; set; }
       
        [BsonElement("passwordHash")]
        public virtual string PasswordHash { get; set; }
        
        [BsonElement("securityStamp")]
		public virtual string SecurityStamp { get; set; }
        

        [BsonElement("roles")]
		public virtual List<string> Roles { get; private set; }
        

        [BsonElement("claims")]
		public virtual List<string> Claims { get; private set; }
       

        [BsonElement("logins")]
        public virtual List<UserLoginInfo> Logins { get; private set; }





        /// <summary>
        /// Gets the subsriptions.
        /// </summary>
        /// <value>The subscriptions the user has access to.</value>
        //[BsonElement("subscriptions")]
        //public virtual List<Subscription> Subscriptions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
        /// </summary>
        //public ApplicationUser()
        //{
        //    Claims = new List<string>();
        //    Roles = new List<string>();
        //    Logins = new List<UserLoginInfo>();
        //    Subscriptions = new List<Subscription>();
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        //public ApplicationUser(string userName) : this()
        //{
        //    UserName = userName;
        //}


    }
}
