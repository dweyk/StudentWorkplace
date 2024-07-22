namespace StudentWorkplace.Data;

using Microsoft.EntityFrameworkCore.Design;

public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

		optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=mydatabase;Username=postgres;Password=postgres");

		return new ApplicationDbContext(optionsBuilder.Options);
	}
}