using System.Data.Entity;

namespace Infrastructure.Data
{
    public class SampleContext : DbContext
    {
        public SampleContext()
            : base("DefaultConnection")
        {

        }

        //public IDbSet<Test> Tests { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // use conventions when possible
        //}
    }
}
