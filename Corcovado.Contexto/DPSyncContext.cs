using Corcovado.Modelos;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Corcovado.Contexto
{

    public class DPSyncContext : DbContext
    {
        public DbSet<MessageFile> messageFiles { get; set; }
        public DbSet<Log> logs { get; set; }

        public DbSet<EAIS> eais { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost;Port=5432;username=postgres;password=dalpiaz;database=db_dpsync");

        }

      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<MessageFile>().HasKey(k => new { k.Id, k.Esn });
            modelBuilder.Entity<MessageFile>().Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<MessageFile>().Property(p => p.InputXml).HasColumnName("input_xml");
            modelBuilder.Entity<MessageFile>().Property(p => p.Esn).HasColumnName("esn");
            modelBuilder.Entity<MessageFile>().Property(p => p.Unixtime).HasColumnName("unixtime");
            modelBuilder.Entity<MessageFile>().Property(p => p.Payload).HasColumnName("payload");
            modelBuilder.Entity<MessageFile>().Property(p => p.OutputXml).HasColumnName("output_xml");
            modelBuilder.Entity<MessageFile>().Property(p => p.OutputCsv).HasColumnName("output_csv");
            modelBuilder.Entity<MessageFile>().Property(p => p.Mobile).HasColumnName("mobile");
            modelBuilder.Entity<MessageFile>().Property(p => p.DataConvertida).HasColumnName("data_convertida");
            modelBuilder.Entity<MessageFile>().Property(p => p.Lat).HasColumnName("lat");
            modelBuilder.Entity<MessageFile>().Property(p => p.Lon).HasColumnName("lon");
            modelBuilder.Entity<MessageFile>().Property(p => p.Obs).HasColumnName("obs");
            modelBuilder.Entity<MessageFile>().ToTable("tb_dpsync");


            modelBuilder.Entity<Log>().HasKey(k => new { k.dataCriacao });
            modelBuilder.Entity<Log>().Property(p => p.dataCriacao).HasColumnName("data_criacao");
            modelBuilder.Entity<Log>().Property(p => p.aviso).HasColumnName("aviso");
            modelBuilder.Entity<Log>().Property(p => p.arquivo).HasColumnName("arquivo");
            modelBuilder.Entity<Log>().ToTable("tb_log");

            modelBuilder.Entity<EAIS>().HasKey(k => new { k.id,k.dt_pos_utc });
            modelBuilder.Entity<EAIS>().Property(p => p.vessel_name).HasColumnName("vessel_name");
            modelBuilder.Entity<EAIS>().Property(p => p.latitude).HasColumnName("latitude");
            modelBuilder.Entity<EAIS>().Property(p => p.longitude).HasColumnName("longitude");
            modelBuilder.Entity<EAIS>().Property(p => p.cog).HasColumnName("cog");
            modelBuilder.Entity<EAIS>().Property(p => p.sog).HasColumnName("sog");
            modelBuilder.Entity<EAIS>().Property(p => p.rot).HasColumnName("rot");
            modelBuilder.Entity<EAIS>().Property(p => p.heading).HasColumnName("heading");
            modelBuilder.Entity<EAIS>().Property(p => p.dt_pos_utc).HasColumnName("dt_pos_utc");
            modelBuilder.Entity<EAIS>().ToTable("tb_eais");



        }

    }
}