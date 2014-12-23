using System.Data.Entity;
using AngularJSAuthentication.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AngularJSAuthentication.API.Data
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {
     
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

}