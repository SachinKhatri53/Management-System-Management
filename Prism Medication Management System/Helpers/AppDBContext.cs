using Microsoft.EntityFrameworkCore;
using Prism_Medication_Management_System.Models;

namespace Prism_Medication_Management_System.Helpers
{
    public class AppDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=DESKTOP-5VTVLL0\\SQLEXPRESS;Database=medication_management_system;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<User> user { get; set; }
        public DbSet<Medication> medication { get; set; }
        public DbSet<Dosage> dosage { get; set; }
        public DbSet<Schedule> schedule { get; set; }
        public DbSet<MedicineNotification> notification { get; set; }
       /* public DbSet<Report> report { get; set; }*/
        
    }
}
