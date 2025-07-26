using Repositories.EFCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            // Configuration for the database connection builder

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            // DBContextOptionsBuilder to configure the context

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlite(configuration.GetConnectionString("SqlConnection"),
                 prj =>  prj.MigrationsAssembly("WebAPI"));


            return new RepositoryContext(builder.Options);


        }


    }
    
}
