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

                        bool _INSERIR_POR_INTERVALO = true;
                        string _CAMINHO_PADRAO_TXT = @"c:\dpsync_web_saida_txt";
                        string _CAMINHO_PADRAO_XML = @"c:\dpsync_web_saida_xml";




                        List<EAIS> eaisList = new List<EAIS>();
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



                            //int max = 0;
                            //try
                            //{
                            //    max = ctx.messageFiles.Max(x => x.Id);
                            //}
                            //catch
                            //{


                            //}
                          
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
                                
                                if (_INSERIR_POR_INTERVALO == false)
                                {
                                    //ctx.messageFiles.Add(
                                    //           new MessageFile
                                    //           {
                                    //               Id = ++max,
                                    //               DataPos = eais.dt_pos_utc,
                                    //               DataConvertida = eais.dt_pos_utc.ToString("yyyyMMdd_HHmmss"),
                                    //               Esn = eais.id.ToString(),
                                    //               Tipo = "EAIS",
                                    //               InputXml = eais.id.ToString(),
                                    //               Lat = eais.latitude.ToString(),
                                    //               Lon = eais.longitude.ToString(),
                                    //               Mobile = eais.vessel_name.Trim().ToUpper(),
                                    //               DataCriacao = DateTime.Now




                                    //           }
                                    //       );
                                }
                                else
                                {
                                    MessageFile mInferior  = ctx.messageFiles.OrderByDescending(x => x.DataPos)
                                        .Where(x =>
                                            x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                            &&
                                            x.DataPos < Convert.ToDateTime(item.DtPosUtc)
                                        )
                                        .FirstOrDefault();


                                    //MessageFile mSuperior = ctx.messageFiles.OrderBy(x => x.DataPos)
                                    //   .Where(x =>
                                    //       x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                    //       &&
                                    //       x.DataPos > Convert.ToDateTime(item.DtPosUtc)
                                    //   )
                                    //   .FirstOrDefault();

                                    //if (mInferior == null)
                                    //{
                                    //    continue;
                                    //}

                                    MessageFile global = ctx.messageFiles.FirstOrDefault(x =>
                                        x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                        &&
                                        x.Tipo == "GLOBAL"
                                        );
                                    if (global!=null)
                                    {

                                    }

                                    if (global == null || mInferior==null || DateTime.Now.AddHours(-3).Subtract(mInferior.DataPos).TotalMinutes >= 15)
                                    {
                                      //  max = max + 1;
                                        //ctx.messageFiles.Add(
                                        //        new MessageFile
                                        //        {
                                        //            Id = max,
                                        //            DataPos = eais.dt_pos_utc,
                                        //            DataConvertida = eais.dt_pos_utc.ToString("yyyyMMdd_HHmmss"),
                                        //            Esn = eais.id.ToString(),
                                        //            Tipo = "EAIS",
                                        //            InputXml = eais.id.ToString(),
                                        //            Lat = eais.latitude.ToString(),
                                        //            Lon = eais.longitude.ToString(),
                                        //            Mobile = eais.vessel_name.Trim().ToUpper(),
                                        //            DataCriacao = DateTime.Now
                                        //        }
                                        //    );
                                        string sql = $@"
                                            INSERT INTO public.tb_dpsync(id, input_xml, esn, unixtime, payload, output_xml, output_csv, 
                                                mobile, data_convertida, lat, lon, obs, data_criacao, tipo, data_pos)
                                            VALUES ((select max(id)+1 from tb_dpsync), '{eais.id.ToString()}', '{eais.id.ToString()}', null, null, null,null, 
                                               '{eais.vessel_name.Trim().ToUpper()}', '{eais.dt_pos_utc.ToString("yyyyMMdd_HHmmss")}', '{eais.latitude.ToString()}', '{eais.longitude.ToString()}', null, current_timestamp, 'EAIS', '{ eais.dt_pos_utc}')
                                        ";
                                        var execute = ctx.Database.ExecuteSqlRaw(sql);  //  RETURNING id INTO last_id;

                                        ctx.SaveChanges();
                                        MessageFile elementoComId = ctx.messageFiles.FirstOrDefault(x =>
                                           x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper()
                                           &&
                                           x.DataPos == Convert.ToDateTime(item.DtPosUtc)
                                           );


                                        eais.id_geral = elementoComId.Id;
                                    }
                                    

                                    // List<MessageFile> mList = ctx.messageFiles.OrderByDescending(x => x.DataPos)
                                    //     .Where(x => x.Mobile.Trim().ToUpper() == item.VesselName.Trim().ToUpper())
                                    //     .Take(2)
                                    //     .ToList();

                                    // if (mList.Count < 2)
                                    // {
                                    //     ctx.messageFiles.Add(
                                    //            new MessageFile
                                    //            {
                                    //                Id = ++max,
                                    //                DataPos = eais.dt_pos_utc,
                                    //                DataConvertida = eais.dt_pos_utc.ToString("yyyyMMdd_HHmmss"),
                                    //                Esn = eais.id.ToString(),
                                    //                Tipo = "EAIS",
                                    //                InputXml = eais.id.ToString(),
                                    //                Lat = eais.latitude.ToString(),
                                    //                Lon = eais.longitude.ToString(),
                                    //                Mobile = eais.vessel_name.Trim().ToUpper(),
                                    //                DataCriacao = DateTime.Now




                                    //            }
                                    //        );
                                    // }
                                    //else  if (mList[0].DataPos.Subtract(mList[1].DataPos).Minutes >= 15
                                    //     && eais.dt_pos_utc <= mList[0].DataPos
                                    //     && eais.dt_pos_utc >= mList[1].DataPos)
                                    // {



                                    //     ctx.messageFiles.Add(
                                    //             new MessageFile
                                    //             {
                                    //                 Id = ++max,
                                    //                 DataPos = eais.dt_pos_utc,
                                    //                 DataConvertida = eais.dt_pos_utc.ToString("yyyyMMdd_HHmmss"),
                                    //                 Esn = eais.id.ToString(),
                                    //                 Tipo = "EAIS",
                                    //                 InputXml = eais.id.ToString(),
                                    //                 Lat = eais.latitude.ToString(),
                                    //                 Lon = eais.longitude.ToString(),
                                    //                 Mobile = eais.vessel_name.Trim().ToUpper(),
                                    //                 DataCriacao = DateTime.Now



                                    //             }
                                    //         );
                                    // }


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
                                string newLine = NmeaGPGGA(item.dt_pos_utc, item.latitude.ToString(), item.longitude.ToString());
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
                                            Mobile = item.vessel_name.Trim().ToUpper()
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

        public static string NmeaGPGGA(DateTime date, string lat, string lon)
        {
            string data = date.ToString("yyyyMMdd");
            string hora = date.ToString("HHmmss");
            return $@"$GPGGA,{hora},{lat.Replace(",", ".")},S,{lon.Replace(",", ".")},W";
        }
    }
}
