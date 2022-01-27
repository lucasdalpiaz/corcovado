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
        //homologacao =9
        //producao=1
        public static int servidor = 1;
        public DbSet<MessageFile> messageFiles { get; set; }
        public DbSet<Log> logs { get; set; }
        public DbSet<Barco> barcos { get; set; }
        public DbSet<EAIS> eais { get; set; }

        public DbSet<LogWeb> logWebs { get; set; }

        public DbSet<LogNaoEncontrado> logNaoEncontrados { get; set; }

        public DbSet<LogNaoEncontradoProcessado> logNaoEncontradoProcessados { get; set; }
        public DbSet<LogPorcentagem> logPorcentagens { get; set; }
        public DbSet<CalculoGeo> calculoGeos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (servidor == 9)
            {
                optionsBuilder.UseNpgsql("host=localhost;Port=5432;username=postgres;password=dalpiaz;database=homologacao");
            }
            else if (servidor == 1)
            {
                optionsBuilder.UseNpgsql("host=localhost;Port=5432;username=postgres;password=dalpiaz;database=db_dpsync");
            }
            else
            {
                optionsBuilder.UseNpgsql("host=localhost;Port=5432;username=postgres;password=dalpiaz;database=XXXXXXXX");
            }
            

        }

      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<MessageFile>().HasKey(k => new {k.Id, k.Mobile, k.DataConvertida });
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
            modelBuilder.Entity<MessageFile>().Property(p => p.Tipo).HasColumnName("tipo");
            modelBuilder.Entity<MessageFile>().Property(p => p.DataPos).HasColumnName("data_pos");
            modelBuilder.Entity<MessageFile>().Property(p => p.DataCriacao).HasColumnName("data_criacao");
            modelBuilder.Entity<MessageFile>().Property(p => p.Velocity).HasColumnName("velocity");
            modelBuilder.Entity<MessageFile>().Property(p => p.Course).HasColumnName("course");
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



            modelBuilder.Entity<Barco>().HasKey(k => new { k.nome_eais, k.nome_global });
            modelBuilder.Entity<Barco>().Property(p => p.nome_eais).HasColumnName("nome_eais");
            modelBuilder.Entity<Barco>().Property(p => p.nome_global).HasColumnName("nome_global");
            modelBuilder.Entity<Barco>().Property(p => p.esn_global).HasColumnName("esn_global");
            modelBuilder.Entity<Barco>().Property(p => p.id_eais).HasColumnName("id_eais");
            modelBuilder.Entity<Barco>().Property(p => p.pasta_arquivos_xml).HasColumnName("pasta_arquivos_xml");
            modelBuilder.Entity<Barco>().Property(p => p.pasta_arquivos_txt).HasColumnName("pasta_arquivos_txt");
            modelBuilder.Entity<Barco>().ToTable("tb_barco");


            modelBuilder.Entity<LogWeb>().HasKey(k => new { k.dataCriacao });
            modelBuilder.Entity<LogWeb>().Property(p => p.dataCriacao).HasColumnName("data_criacao");
            modelBuilder.Entity<LogWeb>().Property(p => p.aviso).HasColumnName("aviso");
            modelBuilder.Entity<LogWeb>().ToTable("tb_log_web");

            modelBuilder.Entity<LogNaoEncontrado>().HasKey(k => new { k.Id ,k.IdAnterior});
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.QtdFalha).HasColumnName("qtd_falha");
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.mobile).HasColumnName("mobile");
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.IdAnterior).HasColumnName("id_anterior");
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.DataPos).HasColumnName("data_pos");
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.DataPosAnterior).HasColumnName("data_pos_anterior");
            modelBuilder.Entity<LogNaoEncontrado>().Property(p => p.DataCriacao).HasColumnName("data_criacao");
            modelBuilder.Entity<LogNaoEncontrado>().ToTable("tb_log_nao_encontrado");


            modelBuilder.Entity<LogNaoEncontradoProcessado>().HasKey(k => new { k.mobile ,k.DataPos });
            modelBuilder.Entity<LogNaoEncontradoProcessado>().Property(p => p.mobile).HasColumnName("mobile");
            modelBuilder.Entity<LogNaoEncontradoProcessado>().Property(p => p.DataPos).HasColumnName("data_pos");
            modelBuilder.Entity<LogNaoEncontradoProcessado>().ToTable("tb_log_nao_encontrado_processado");


            modelBuilder.Entity<LogPorcentagem>().HasKey(k => new { k.DataCriacao });
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.DataCriacao).HasColumnName("data_criacao");
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.DataAlteracao).HasColumnName("data_alteracao");
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.Porcentagem).HasColumnName("porcentagem");
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.Realizado).HasColumnName("realizado");
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.Esperado).HasColumnName("esperado");
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.QtdMobile).HasColumnName("qtd_mobile");
            modelBuilder.Entity<LogPorcentagem>().Property(p => p.Posicoes).HasColumnName("posicoes");
            modelBuilder.Entity<LogPorcentagem>().ToTable("tb_log_porcentagem");

            modelBuilder.Entity<CalculoGeo>().HasKey(k => new { k.id, k.data_a, k.data_b });
            modelBuilder.Entity<CalculoGeo>().Property(p => p._a).HasColumnName("_a");
            modelBuilder.Entity<CalculoGeo>().Property(p => p._b).HasColumnName("_b");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.lat_a).HasColumnName("lat_a");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.lat_b).HasColumnName("lat_b");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.lon_a).HasColumnName("lon_a");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.lon_b).HasColumnName("lon_b");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.data_a).HasColumnName("data_a");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.data_b).HasColumnName("data_b");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.fm_decimal).HasColumnName("fm_decimal");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.fm_grau).HasColumnName("fm_grau");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.fm_minuto).HasColumnName("fm_minuto");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.fm_segundo).HasColumnName("fm_segundo");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.df_segundo).HasColumnName("df_segundo");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.dl_segundo).HasColumnName("dl_segundo");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.NA).HasColumnName("na");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.MA).HasColumnName("ma");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.NB).HasColumnName("nb");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.X1).HasColumnName("x1");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.Y1).HasColumnName("y1");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.DE1).HasColumnName("de1");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.az_decimal).HasColumnName("az_decimal");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.az_grau).HasColumnName("az_grau");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.az_minuto).HasColumnName("az_minuto");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.az_segundo).HasColumnName("az_segundo");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.B).HasColumnName("b");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.C).HasColumnName("c");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.D).HasColumnName("d");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.E).HasColumnName("e");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.K1).HasColumnName("k1");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.K2).HasColumnName("k2");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.K3).HasColumnName("k3");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.K4).HasColumnName("k4");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.K5).HasColumnName("k5");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.Y).HasColumnName("y");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.C1).HasColumnName("c1");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.C2).HasColumnName("c2");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.A1B).HasColumnName("a1b");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.W).HasColumnName("w");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.X).HasColumnName("x");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.distancia_m).HasColumnName("distancia_m");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.contra_azimute_decimal).HasColumnName("contra_azimute_decimal");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.contra_azimute_grau).HasColumnName("contra_azimute_grau");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.contra_azimute_minuto).HasColumnName("contra_azimute_minuto");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.contra_azimute_segundo).HasColumnName("contra_azimute_segundo");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.azimute_b_decimal).HasColumnName("azimute_b_decimal");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.azimute_b_grau).HasColumnName("azimute_b_grau");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.azimute_b_minuto).HasColumnName("azimute_b_minuto");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.azimute_b_segundo).HasColumnName("azimute_b_segundo");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.rumo_gds).HasColumnName("rumo_gds");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.quadrante).HasColumnName("quadrante");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.dif_hora).HasColumnName("dif_hora");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.velocidade_km).HasColumnName("velocidade_km");
            modelBuilder.Entity<CalculoGeo>().Property(p => p.velocidade_knots).HasColumnName("velocidade_knots");
            modelBuilder.Entity<CalculoGeo>().ToTable("tb_calculo_geo");

        }

    }
}