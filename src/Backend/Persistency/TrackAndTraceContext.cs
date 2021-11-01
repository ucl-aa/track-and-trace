using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistency
{
    public class TrackAndTraceContext : DbContext
    {
        public TrackAndTraceContext(DbContextOptions<TrackAndTraceContext> options)
            : base(options)
        {
        }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<DropOff> DropOffs { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<ZipCode> ZipCodes { get; set; }
    }
}