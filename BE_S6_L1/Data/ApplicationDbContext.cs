using BE_S6_L1.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BE_S6_L1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Studente> Studenti { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Studente>().HasData(
                new Studente { Id = 1, Nome = "Mario", Cognome = "Rossi", DataDiNascita = new DateTime(1995, 5, 10), Email = "mario.rossi@example.com" },
                new Studente { Id = 2, Nome = "Laura", Cognome = "Bianchi", DataDiNascita = new DateTime(1998, 8, 15), Email = "laura.bianchi@example.com" },
                new Studente { Id = 3, Nome = "Giuseppe", Cognome = "Verdi", DataDiNascita = new DateTime(1997, 3, 21), Email = "giuseppe.verdi@example.com" }
            );
        }
    }
}