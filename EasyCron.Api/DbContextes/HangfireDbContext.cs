using Bayantu.Evos.Services.CronJob.Api.Entities.Hangfire;
using Microsoft.EntityFrameworkCore;

namespace EasyCron.Api.DbContextes
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext(DbContextOptions<HangfireDbContext> option)
            : base(option)
        {

        }

        public DbSet<JobParameter> JobParameters { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerData> ServerDatas { get; set; }
        public DbSet<SqlHash> SqlHashs { get; set; }
        public DbSet<SqlJob> SqlJobs { get; set; }
        public DbSet<SqlState> SqlStates { get; set; }
    }
}
