using Microsoft.EntityFrameworkCore;
using OlympicsAPI.Models;

namespace OlympicsAPI.Data
{
    //PACKAGES NEEDED: 
    // dotnet add package Microsoft.EntityFrameworkCore
    // dotnet add package Microsoft.EntityFrameworkCore.Relational
    // dotnet add package Microsoft.EntityFrameworkCore.SqlServer

    //Config object defines the connection settings etc

    //Data context = central entity framework / dapper class, contains all methods/classes
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;
        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        //dbset -> a blueprint for mapping a model onto a table in the database
        public virtual DbSet<TutorialUser> TutorialUsers { get; set; }
        public virtual DbSet<TutorialUserSalary> TutorialUserSalary { get; set; }
        public virtual DbSet<TutorialUserJobInfo> TutorialUserJobInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("TestConnection"), optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }


        //HasDefaultSchema uses Microsoft.EntityFrameworkCore.Relational
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //choosing the schema (container/organiser) for the model to be built and organised
            modelBuilder.HasDefaultSchema("TutorialAppSchema");


            //using an identifier 
            modelBuilder.Entity<TutorialUser>()
                .ToTable("Users", "TutorialAppSchema")
                .HasKey(u => u.UserId);
            modelBuilder.Entity<TutorialUserSalary>()
                .ToTable("Users", "TutorialAppSchema")
                .HasKey(u => u.UserId); ;
            modelBuilder.Entity<TutorialUserJobInfo>()
                .ToTable("Users", "TutorialAppSchema")
                .HasKey(u => u.UserId);

            //for revisiting see csharp 71
        }
    }

}