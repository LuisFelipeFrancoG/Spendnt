using Microsoft.EntityFrameworkCore;

namespace Spendn_t.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext>options):base(options)
        {
                
        }

    }
}
