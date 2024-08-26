using Initial_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Initial_API.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContextEF(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<EmployeeModel> Employees { get; set; }
        public virtual DbSet<CardModel> Cards { get; set; }
        public virtual DbSet<EscalationModel> Escalations { get; set; }
        public virtual DbSet<NoteModel> Notes { get; set; }
        public virtual DbSet<CustomerModel> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnectionString"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF wil know which table will be affected by its primary key
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<EmployeeModel>()
                .ToTable("Employee", "dbo")
                .HasKey(e => e.IdEmployee);

            modelBuilder.Entity<CardModel>()
                .HasKey(e => e.IdCard);

            modelBuilder.Entity<NoteModel>()
                .HasKey(e => e.IdNote);

            modelBuilder.Entity<CustomerModel>()
                .HasKey(e => e.IdCustomer);

            modelBuilder.Entity<EscalationModel>()
                .HasKey(e => e.IdEscalation);
        }
    }
}
