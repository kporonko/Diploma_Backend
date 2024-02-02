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
    public class OptionTranslationConfiguration : IEntityTypeConfiguration<OptionTranslation>
    {
        public void Configure(EntityTypeBuilder<OptionTranslation> builder)
        {
            builder
                .ToTable(nameof(OptionTranslation))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder
                .Property(t => t.Language)
                .IsRequired()
                .HasColumnName("Language")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.OptionLine)
                .IsRequired()
                .HasColumnName("OptionLine")
                .HasColumnType("varchar(max)");
        }
    }
}
