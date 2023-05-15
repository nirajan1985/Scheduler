using Microsoft.EntityFrameworkCore;
using SchedulerApi.Model;

namespace SchedulerApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {

        }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
