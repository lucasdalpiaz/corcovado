using Corcovado.Contexto;
using Corcovado.Modelos;
using Corcovado.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
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


                    List<EAIS> eaisList = new List<EAIS>();

                    foreach (var item in configuracao.FeatureMembers.LVI)
                    {
                        eaisList.Add(
                            new EAIS
                            {
                                id = item.Id,
                                vessel_name = item.VesselName,
                                longitude = item.Longitude,
                                latitude = item.Latitude,
                                sog = item.Sog,
                                cog = item.Cog,
                                rot = item.Rot,
                                heading = item.Heading,
                                dt_pos_utc = Convert.ToDateTime(item.DtPosUtc)
                            }
                        );

                    }

                    using (var ctx = new DPSyncContext())
                    {

                        foreach (EAIS item in eaisList)
                        {
                            EAIS aux = ctx.eais.FirstOrDefault(e => e.id == item.id && e.dt_pos_utc == item.dt_pos_utc);
                            if (aux == null)
                            {
                                ctx.eais.Add(item);
                                await ctx.SaveChangesAsync();

                                string raiz = @"c:\teste_dp_web";

                                string _NAME_OUTPUT_FILE = item.vessel_name + "_" + item.dt_pos_utc.ToString("yyyyMMdd_HHmmss");

                                if (!Directory.Exists(raiz))
                                    Directory.CreateDirectory(raiz);
                                var txt = new StringBuilder();
                                string newLine = NmeaGPGGA(item.dt_pos_utc, item.latitude.ToString(), item.longitude.ToString());
                                txt.Append(newLine);

                                //after your loop
                                File.WriteAllText(
                                    raiz + "\\" + _NAME_OUTPUT_FILE + ".txt",
                                    txt.ToString());


                            }

                        }
                    }



                    return true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        public static string NmeaGPGGA(DateTime date, string lat, string lon)
        {
            string data = date.ToString("yyyyMMdd");
            string hora = date.ToString("HHmmss");
            return $@"$GPGGA,{hora},{lat.Replace(",", ".")},S,{lon.Replace(",", ".")},W";
        }
    }
}
