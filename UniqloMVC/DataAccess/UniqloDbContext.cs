using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using UniqloMVC.Models;

namespace UniqloMVC.DataAccess
{
    public class UniqloDbContext:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Uniqlo;Trusted_Connection=True;TrustServerSertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        //public UniqloDbContext(DbContextOptions opt):base(opt)
        //{

        //}
    }
}
