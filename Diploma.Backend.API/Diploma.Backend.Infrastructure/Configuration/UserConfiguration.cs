using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diploma.Backend.Domain.Models;

namespace Diploma.Backend.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable(nameof(User))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder
                .Property(t => t.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.FirstName)
                .IsRequired()
                .HasColumnName("FirstName")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.LastName)
                .IsRequired()
                .HasColumnName("LastName")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.Password)
                .IsRequired()
                .HasColumnName("Password")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.Role)
                .IsRequired()
                .HasColumnName("Role")
                .HasColumnType("varchar(max)");
        }
    }
}
