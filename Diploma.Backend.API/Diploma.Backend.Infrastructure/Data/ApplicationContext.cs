using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Targeting> Targetings { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<OptionTranslation> OptionTranslations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryInTargeting> CountriesInTargetings { get; set; }
        public DbSet<SurveyInUnit> SurveysInUnits { get; set; }
        public DbSet<SurveyUnit> SurveyUnits { get; set; }
        public DbSet<UnitSettings> UnitSettings { get; set; }
        public DbSet<UnitAppearance> UnitAppearances { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<QuestionLine> QuestionLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TargetingConfiguration());
            modelBuilder.ApplyConfiguration(new SurveyConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionOptionConfiguration());
            modelBuilder.ApplyConfiguration(new OptionTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CountryInTargetingConfiguration());
            modelBuilder.ApplyConfiguration(new SurveyInUnitConfiguration());
            modelBuilder.ApplyConfiguration(new SurveyUnitConfiguration());
            modelBuilder.ApplyConfiguration(new UnitSettingsConfiguration());
            modelBuilder.ApplyConfiguration(new UnitAppearanceConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionLineConfiguration());

            modelBuilder.Entity<User>()
                .HasKey(k => k.Id);


            modelBuilder.Entity<Question>()
                .Property(q => q.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (QuestionType)Enum.Parse(typeof(QuestionType), v)
                );
            
            modelBuilder.Entity<Survey>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<Survey>()
                .HasOne(u => u.User)
                .WithMany(s => s.Surveys)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Survey>()
                .HasOne(s => s.Targeting)
                .WithMany(t => t.Surveys)
                .HasForeignKey(s => s.TargetingId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);


            modelBuilder.Entity<Targeting>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Question>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<Question>()
                .HasOne(s => s.Survey)
                .WithMany(q => q.Questions)
                .HasForeignKey(s => s.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionLine>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Question>()
                .HasOne(s => s.QuestionLine)
                .WithOne(q => q.Question)
                .HasForeignKey<QuestionLine>(s => s.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionOption>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<QuestionOption>()
                .HasOne(q => q.Question)
                .WithMany(o => o.QuestionOptions)
                .HasForeignKey(q => q.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OptionTranslation>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<OptionTranslation>()
                .HasOne(o => o.QuestionOption)
                .WithMany(t => t.OptionTranslations)
                .HasForeignKey(o => o.QuestionOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Country>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<CountryInTargeting>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<CountryInTargeting>()
                .HasOne(t => t.Country)
                .WithMany(c => c.CountryInTargetings)
                .HasForeignKey(t => t.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CountryInTargeting>()
                .HasOne(t => t.Targeting)
                .WithMany(c => c.CountryInTargetings)
                .HasForeignKey(t => t.TargetingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyInUnit>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<SurveyInUnit>()
                .HasOne(s => s.Survey)
                .WithMany(su => su.SurveyInUnits)
                .HasForeignKey(s => s.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SurveyInUnit>()
                .HasOne(s => s.SurveyUnit)
                .WithMany(su => su.SurveyInUnits)
                .HasForeignKey(s => s.SurveyUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyUnit>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<SurveyUnit>()
                .HasOne(s => s.UnitSettings)
                .WithOne(su => su.SurveyUnit)
                .HasForeignKey<UnitSettings>(s => s.SurveyUnitId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SurveyUnit>()
                .HasOne(s => s.UnitAppearance)
                .WithMany(su => su.SurveyUnits)
                .HasForeignKey(s => s.AppearanceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UnitSettings>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<UnitAppearance>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<UnitAppearance>()
                .HasOne(u => u.Template)
                .WithMany(a => a.UnitAppearances)
                .HasForeignKey(u => u.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Template>()
                .HasKey(k => k.Id);


            // Add user relationships to all needed entities

            modelBuilder.Entity<Targeting>()
                .HasOne(u => u.User)
                .WithMany(s => s.Targetings)
                .HasForeignKey(u => u.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SurveyUnit>()
                .HasOne(u => u.User)
                .WithMany(s => s.SurveyUnits)
                .HasForeignKey(u => u.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UnitAppearance>()
                .HasOne(u => u.User)
                .WithMany(s => s.UnitAppearances)
                .HasForeignKey(u => u.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UnitSettings>()
                .HasOne(u => u.User)
                .WithMany(s => s.UnitSettings)
                .HasForeignKey(u => u.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
        }
    }
}
