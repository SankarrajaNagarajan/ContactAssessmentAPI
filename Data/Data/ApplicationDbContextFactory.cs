using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ContactApi.Data.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-O3FT923;Database=ContactDb;User Id=sa;Password=Test@123;TrustServerCertificate=true;MultipleActiveResultSets=true"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
