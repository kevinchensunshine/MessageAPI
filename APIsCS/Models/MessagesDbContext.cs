using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIsCS.Models;

public partial class MessagesDbContext : DbContext
{
    public MessagesDbContext()
    {
    }

    public MessagesDbContext(DbContextOptions<MessagesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Message> Messages { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MessagesDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
