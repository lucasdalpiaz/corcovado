using Corcovado.Contexto;
using Corcovado.Modelos;
using Corcovado.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Corcovado.WebApi.Repositorios
{
    public class REAIS : IEAIS
    {
        private readonly DPSyncContext _context;


        public REAIS(DPSyncContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        #region gerais
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;

        }
        #endregion


        public async Task<bool> RealizarPricedimento()
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
                    Console.WriteLine(item);
                }

                return true;
            }
        }

        public async Task<EAIS[]> GetAllEais()
        {
            IQueryable<EAIS> query = _context.eais;



            query = query.AsNoTracking()
                        .OrderBy(c => c.id);

            return await query.ToArrayAsync();
        }
    }
}
