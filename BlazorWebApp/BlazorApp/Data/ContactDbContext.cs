using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class ContactDbContext(DbContextOptions<ContactDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("tblBlazorContacts");
            entity.HasKey(e => e.ContactId);
            entity.Property(e => e.ContactId).ValueGeneratedOnAdd();

            entity.Property(e => e.FullName)
                .HasColumnName("Full Name")
                .IsRequired();

            entity.Property(e => e.Address).HasColumnName("Address");
            entity.Property(e => e.City).HasColumnName("City");
            entity.Property(e => e.State).HasColumnName("State");
            entity.Property(e => e.Zip).HasColumnName("Zip");

            entity.Property(e => e.Email)
                .HasColumnName("Email")
                .IsRequired();
        });
    }
}
