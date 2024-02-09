using Diploma.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Configuration
{
    public class UnitAppearanceConfiguration : IEntityTypeConfiguration<UnitAppearance>
    {
        public void Configure(EntityTypeBuilder<UnitAppearance> builder)
        {
            builder
                .ToTable(nameof(UnitAppearance))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder
                .Property(t => t.Type)
                .IsRequired()
                .HasColumnName("Type")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.Params)
                .IsRequired()
                .HasColumnName("Params")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.State)
                .IsRequired()
                .HasColumnName("State")
                .HasColumnType("bit");
        }
    }
}
