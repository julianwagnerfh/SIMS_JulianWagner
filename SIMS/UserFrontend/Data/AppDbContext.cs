using Microsoft.EntityFrameworkCore;
using UserFrontend.Models;

namespace UserFrontend.Data
{
	public class AppDbContext : DbContext
	{
        public DbSet<User> Users { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{

		}
    }
}
