using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    public abstract class BaseTest
    {
        protected readonly ApplicationDbContext _dbContext;

        protected BaseTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
        }
    }
}
