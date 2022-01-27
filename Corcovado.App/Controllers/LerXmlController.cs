using Corcovado.Contexto;
using Corcovado.Modelos;
using Corcovado.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;

namespace Corcovado.App.Controllers
{
    public class LerXmlController : ApiController
    {
     

        [HttpGet]
        public  async static Task<bool> LerXml()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    var authToken = Encoding.ASCII.GetBytes("cust_hisdesat_petrobras_gws:voSLH5Mf");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


                    var result = await client.GetStreamAsync("https://services.exactearth.com/gws/wfs?service=WFS&version=1.1.0&request=GetFeature&typeName=exactAIS:LVI&maxFeatures=50&outputFormat=gml3");


                    var serializer = new XmlSerializer(typeof(FeatureCollection));
                    FeatureCollection configuracao = new FeatureCollection();

                    using (var textReader = new StreamReader(result))
                    {
                        configuracao = (FeatureCollection)serializer.Deserialize(textReader);
                    }



                    using (var ctx = new DPSyncContext())
                    {

                        string _CAMINHO_PADRAO_TXT = "";
                        string _CAMINHO_PADRAO_XML = "";
                        if (DPSyncContext.servidor == 9)
                        {
                            _CAMINHO_PADRAO_TXT = @"c:\homologacao\dpsync_web_saida_txt";
                            _CAMINHO_PADRAO_XML = @"c:\homologacao\dpsync_web_saida_xml";
                        }
                        else if (DPSyncContext.servidor == 1)
                        {
                            _CAMINHO_PADRAO_TXT = @"c:\dpsync_web_saida_txt";
                            _CAMINHO_PADRAO_XML = @"c:\dpsync_web_saida_xml";
                        }
                        else
                        {
                            _CAMINHO_PADRAO_TXT = @"c:\other\dpsync_web_saida_txt";
                            _CAMINHO_PADRAO_XML = @"c:\other\dpsync_web_saida_xml";
                        }



                        List<EAIS> eaisList = new List<EAIS>();
                        List<CalculoGeo> calcGeoList = new List<CalculoGeo>();


                        IList<Barco> barcosDesativados = ctx.barcos
                                .Where(x => x.esn_global.ToUpper().Trim() == "D")
                                .OrderBy(c => c.nome_eais)
                                .ToList();

                        foreach (var item in configuracao.FeatureMembers.LVI)
                        {

                           

                            if (barcosDesativados.FirstOrDefault(x=>x.nome_eais == item.VesselName.ToUpper().Trim()) != null)
                            {
                                continue;
                            }


                          
                            EAIS eais = new EAIS
                            {
                                id = item.Id,
                                id_geral = -99 ,
                                vessel_name = item.VesselName,
                                longitude = item.Longitude,
                                latitude = item.Latitude,
                                sog = item.Sog,
                                cog = item.Cog,
                                rot = item.Rot,
                                heading = item.Heading,
                                dt_pos_utc = Convert.ToDateTime(item.DtPosUtc)
                            };

                            eaisList.Add(eais);

                            
                            if (ctx.messageFiles.FirstOrDefault(m=>m.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper() && m.DataConvertida == Convert.ToDateTime(item.DtPosUtc).ToString("yyyyMMdd_HHmmss")) == null)
                            {

                                MessageFile mInferior = ctx.messageFiles.OrderByDescending(x => x.DataPos)
                                        .Where(x =>
                                            x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                            &&
                                            x.DataPos < Convert.ToDateTime(item.DtPosUtc)
                                        )
                                        .FirstOrDefault();


                                MessageFile global = ctx.messageFiles.FirstOrDefault(x =>
                                    x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                    &&
                                    x.Tipo == "GLOBAL"
                                    );


                                if (global == null || mInferior == null || DateTime.Now.AddHours(-3).Subtract(mInferior.DataPos).TotalMinutes >= 15)
                                {
                                    var anterior =  await ctx.messageFiles
                                        .OrderByDescending(x => x.DataPos)
                                        .Where(x => x.DataPos < eais.dt_pos_utc && x.Mobile == eais.vessel_name.Trim().ToUpper())
                                        .FirstOrDefaultAsync();

                                    CalculoGeo calcGeo;

                                    if (anterior != null)
                                    {
                                        calcGeo = new CalculoGeo(
                                         eais.latitude,
                                         eais.longitude,
                                         Convert.ToDouble(anterior.Lat),
                                         Convert.ToDouble(anterior.Lon),
                                         eais.dt_pos_utc,
                                         anterior.DataPos);
                                    }
                                    else
                                    {
                                        calcGeo = new CalculoGeo(
                                        eais.latitude,
                                        eais.longitude,
                                        0,
                                        0,
                                        eais.dt_pos_utc,
                                        DateTime.Now);
                                        calcGeo.velocidade_knots = 0;
                                        calcGeo.azimute_b_grau = 0;
                                    }


                                    


                                   

                                    string sql = $@"
                                            INSERT INTO public.tb_dpsync(id, input_xml, esn, unixtime, payload, output_xml, output_csv, 
                                                mobile, data_convertida, lat, lon, obs, data_criacao, tipo, data_pos, velocity, course)
                                            VALUES ((select max(id)+1 from tb_dpsync), '{eais.id.ToString()}', '{eais.id.ToString()}', null, null, null,null, 
                                                '{eais.vessel_name.Trim().ToUpper()}', '{eais.dt_pos_utc.ToString("yyyyMMdd_HHmmss")}',
                                                '{eais.latitude.ToString()}', '{eais.longitude.ToString()}', null, current_timestamp, 'EAIS', '{ eais.dt_pos_utc.ToString("yyyy-MM-dd HH:mm:ss")}',
                                                {calcGeo.velocidade_knots.ToString().Replace(",",".")}, {calcGeo.azimute_b_grau.ToString().Replace(",", ".")})
                                        ";
                                    var execute = ctx.Database.ExecuteSqlRaw(sql);  //  RETURNING id INTO last_id;




                                    int retconsul = await ctx.SaveChangesAsync();
                                    MessageFile elementoComId = ctx.messageFiles.FirstOrDefault(x =>
                                       x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                       &&
                                       x.DataPos == Convert.ToDateTime(item.DtPosUtc)
                                       );


                                    eais.id_geral = elementoComId.Id;
                                    calcGeo.id = elementoComId.Id;
                                    calcGeoList.Add(calcGeo);

                                    if (calcGeo.lat_a != 0 && calcGeo.lon_a !=0)
                                    {
                                        ctx.calculoGeos.Add(calcGeo);
                                        await ctx.SaveChangesAsync();
                                    }
                                    
                                }


                            

                            }


                        }





                        foreach (EAIS item in eaisList)
                        {
                            if (item.id_geral==-99)
                            {
                                continue;
                            }
                            Barco barco = ctx.barcos.FirstOrDefault(
                                b => 
                                b.nome_eais.Trim().ToUpper() == item.vessel_name.Trim().ToUpper() 
                                || 
                                b.nome_global.Trim().ToUpper() == item.vessel_name.Trim().ToUpper()
                            );
                            if (barco == null)
                            {
                                ctx.barcos.Add(
                                        new Barco
                                        {
                                            nome_eais = item.vessel_name.Trim().ToUpper(),
                                            nome_global = item.vessel_name.Trim().ToUpper(),
                                            id_eais = item.id

                                        }
                                    ); 



                            }


                            EAIS aux = ctx.eais.FirstOrDefault(e => e.id == item.id && e.dt_pos_utc == item.dt_pos_utc);
                            if (aux == null)
                            {
                                ctx.eais.Add(item);
                                await ctx.SaveChangesAsync();

                                barco = ctx.barcos.FirstOrDefault(
                                b =>
                                b.nome_eais.Trim().ToUpper() == item.vessel_name.Trim().ToUpper()
                                ||
                                b.nome_global.Trim().ToUpper() == item.vessel_name.Trim().ToUpper()
                                );


                                CalculoGeo calculo = calcGeoList.Find(x => x.id == item.id_geral);
                                //inserir arquivo txt-------------------------------------------------------------------------------------------

                                string caminho_txt = "";

                                if (barco != null)
                                {
                                    caminho_txt = barco.pasta_arquivos_txt + "\\" + barco.nome_eais;
                                    if (string.IsNullOrEmpty(barco.pasta_arquivos_txt))
                                    {
                                        caminho_txt = _CAMINHO_PADRAO_TXT + "\\" + barco.nome_eais;
                                    }
                                }

                                string _NAME_OUTPUT_FILE = barco.nome_eais + "_" + item.dt_pos_utc.ToString("yyyyMMdd_HHmmss");

                                if (!Directory.Exists(caminho_txt))
                                    Directory.CreateDirectory(caminho_txt);
                                var txt = new StringBuilder();
                                string newLine = NmeaGPGGA(item.dt_pos_utc, item.latitude.ToString(), item.longitude.ToString(), calculo.velocidade_knots, calculo.azimute_b_grau);
                                txt.Append(newLine);

                                //after your loop
                                File.WriteAllText(
                                    caminho_txt + "\\" + _NAME_OUTPUT_FILE + ".txt",
                                    txt.ToString());


                                //inserir arquivo xml--------------------------------------------------------------------------------------------

                                string caminho_xml = "";

                                if (barco != null)
                                {
                                    caminho_xml = barco.pasta_arquivos_xml + "\\" + barco.nome_eais;
                                    if (string.IsNullOrEmpty(barco.pasta_arquivos_xml))
                                    {
                                        caminho_xml = _CAMINHO_PADRAO_XML + "\\" + barco.nome_eais;
                                    }
                                }


                                if (!Directory.Exists(caminho_xml))
                                    Directory.CreateDirectory(caminho_xml);

                                //Salvando xml de saída
                                serializer = new XmlSerializer(typeof(OutputXml));
                                var localArquivo = caminho_xml + "\\" + _NAME_OUTPUT_FILE + ".xml";
                                var xmlNamespaces = new XmlSerializerNamespaces();

                                using (var textWriter = new StreamWriter(localArquivo))
                                {
                                   

                                    serializer.Serialize(
                                        textWriter,
                                        new OutputXml { 
                                            Date = item.dt_pos_utc.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Id =item.id_geral,
                                            Lat = Math.Round(item.latitude, 6),
                                            Lon = Math.Round(item.longitude, 6),
                                            Mobile = item.vessel_name.Trim().ToUpper(),
                                            Velocity = calculo.velocidade_knots,
                                            Course = calculo.azimute_b_grau
                                        },
                                        xmlNamespaces
                                        );
                                }





                            }

                        }
                    }




                }
            }
            catch (Exception ex)
            {
                using (var ctx = new DPSyncContext())
                {
                    ctx.logWebs.Add(new LogWeb { aviso = "ex: " + ex.Message + ". InnerEx: " + ex.InnerException, dataCriacao = DateTime.Now});
                    await ctx.SaveChangesAsync();

                }
            }

            return true;
        }

        public static string NmeaGPGGA(DateTime date, string lat, string lon, double velocity, double couse)
        {
            string data = date.ToString("yyyyMMdd");
            string hora = date.ToString("HHmmss");
            return $@"$GPGGA,{hora},{lat.Replace(",", ".")},S,{lon.Replace(",", ".")},W";
        }

        [HttpGet]
        public async static Task<bool> CalculosGeodesicos()
        {
            try
            {
                using (var ctx = new DPSyncContext())
                {


                    //var result = ctx.atualAnteriorDTOs.FromSqlRaw($@"
                    //WITH cte AS (
                    //SELECT 
                    //mobile,
                    //id,
                    //lat as lat_b,
                    //lon as lon_b,
                    //data_pos as data_pos_b,
                    //LEAD (lat,1) OVER (PARTITION BY mobile ORDER BY data_pos desc) AS lat_a,
                    //LEAD (lon,1) OVER (PARTITION BY mobile ORDER BY data_pos desc) AS lon_a,
                    //LEAD (data_pos,1) OVER (PARTITION BY mobile ORDER BY data_pos desc) AS data_pos_a
                    //FROM tb_dpsync
                    //)
                    //select * from cte 
                    //WHERE data_pos_b >= NOW()::DATE - 1 AND data_pos_a >= NOW()::DATE - 1  

                    //")
                    //.ToListAsync();

                    //foreach (var item in result.Result.ToList())
                    //{

                    //}


                    var anterior = await ctx.messageFiles.OrderByDescending(x=>x.DataPos).Where(x=>x.DataPos < DateTime.Now && x.Mobile == "SKANDI  BOTAFOGO").FirstOrDefaultAsync();
               
                    CalculoGeo calcGeo = new CalculoGeo( -22.84875, -43.13145, -22.84878, -43.13148, 
                        new DateTime(2021,11,6,0,9,40),
                        new DateTime(2021,11,6,0,4,8));


                }
            }
            catch (Exception ex)
            {

                using (var ctx = new DPSyncContext())
                {
                    ctx.logWebs.Add(new LogWeb { aviso = "ex: " + ex.Message + ". InnerEx: " + ex.InnerException, dataCriacao = DateTime.Now });
                    await ctx.SaveChangesAsync();

                }
            }
            return true;
        }



    }
}
