using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;


namespace Data
{
    public class ProjektContext : DbContext
    {
        
        public DbSet<Questions> Questions => Set<Questions>();
        public DbSet<Answers> Answers => Set<Answers>();
        public DbSet<Category> Category => Set<Category>();
        public DbSet<User> User => Set<User>();

        public ProjektContext(DbContextOptions<ProjektContext> options)
            : base(options)
        {
            // Den her er tom. Men ": base(options)" sikre at constructor
            // på DbContext super-klassen bliver kaldt.
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Et eksempel på at man selv kan styre hvad en tabel skal hedde.
            modelBuilder.Entity<TodoTask>().ToTable("Tasks");
        }*/
    }
}
