using EasyCron.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyCron.Api.DbContextes
{
    public class CronJobDbContext : DbContext
    {
        public CronJobDbContext(DbContextOptions<CronJobDbContext> option)
            : base(option)
        {

        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobLog> JobLogs { get; set; }
    }
}
