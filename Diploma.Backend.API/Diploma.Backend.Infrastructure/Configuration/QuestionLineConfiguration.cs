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
    public class QuestionLineConfiguration : IEntityTypeConfiguration<QuestionLine>
    {
        public void Configure(EntityTypeBuilder<QuestionLine> builder)
        {
            builder
                .ToTable(nameof(QuestionLine))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
        }
    }
}
