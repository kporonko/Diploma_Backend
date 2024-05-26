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
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder
                .ToTable(nameof(Subscription))
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder
                .Property(t => t.Name)
                .HasColumnType("varchar(255)")
                .HasColumnName(nameof(Subscription.Name))
                .IsRequired(false);
            builder
                .Property(t => t.IsActive)
                .HasColumnType("bit")
                .HasColumnName(nameof(Subscription.IsActive))
                .IsRequired();
            builder
                .Property(t => t.DateChangeStatus)
                .HasColumnType("datetime")
                .HasColumnName(nameof(Subscription.DateChangeStatus))
                .IsRequired();
            builder
                .Property(t => t.DateCreate)
                .HasColumnType("datetime")
                .HasColumnName(nameof(Subscription.DateCreate))
                .IsRequired();
            builder
                .Property(t => t.SubscriptionId)
                .HasColumnType("varchar(255)")
                .HasColumnName(nameof(Subscription.SubscriptionId))
                .IsRequired(false);
        }
    }
}
