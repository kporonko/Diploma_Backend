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
    public class UnitSettingsConfiguration : IEntityTypeConfiguration<UnitSettings>
    {
        public void Configure(EntityTypeBuilder<UnitSettings> builder)
        {
            builder
                .ToTable(nameof(UnitSettings))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder
                .Property(t => t.OneSurveyTakePerDevice)
                .IsRequired()
                .HasColumnName("OneSurveyTakePerDevice")
                .HasColumnType("int");
            builder
                .Property(t => t.MaximumSurveysPerDevice)
                .IsRequired()
                .HasColumnName("MaximumSurveysPerDevice")
                .HasColumnType("int");
            builder
                .Property(t => t.HideAfterNoSurveys)
                .IsRequired()
                .HasColumnName("HideAfterNoSurveys")
                .HasColumnType("bit");
            builder
                .Property(t => t.MessageAfterNoSurveys)
                .IsRequired()
                .HasColumnName("MessageAfterNoSurveys")
                .HasColumnType("bit");
        }
    }
}
