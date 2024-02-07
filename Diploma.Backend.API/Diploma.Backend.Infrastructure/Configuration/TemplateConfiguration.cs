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
    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder
                .ToTable(nameof(Template))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder
                .Property(t => t.TemplateCode)
                .IsRequired()
                .HasColumnName("TemplateCode")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.DefaultParams)
                .IsRequired()
                .HasColumnName("DefaultParams")
                .HasColumnType("varchar(max)");
            builder
                .Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(max)");
        }
    }
}
