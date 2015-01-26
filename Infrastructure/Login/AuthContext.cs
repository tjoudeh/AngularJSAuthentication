using System.Data.Entity;
using Core.DomainModel.AuthEntities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Infrastructure.Login
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