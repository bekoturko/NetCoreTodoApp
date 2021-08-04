using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreTodoApp.Model;

namespace NetCoreTodoApp.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Todo> ToDos { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Todo>(entity =>
			{
				entity.Property(e => e.TodoId)
				.UseIdentityColumn();

				entity.Property(e => e.UserId)
				.IsRequired();
			});

			base.OnModelCreating(builder);
		}
	}
}