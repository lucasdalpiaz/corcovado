using Corcovado.Contexto;
using Corcovado.Modelos;
using Corcovado.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.WebApi.Repositorios
{
    public class RMessageFile : IMessageFile
    {

        private readonly DPSyncContext _context;


        public RMessageFile(DPSyncContext context)
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

        #region POR DATA
        public async Task<Corcovado.Modelos.response.Position[]> RetornaMaioresQueData(string data, string mobile = null)
        {

            var query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos >= DateTime.ParseExact(data, "yyyyMMdd_HHmmss", null))
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos >= DateTime.ParseExact(data, "yyyyMMdd_HHmmss", null) && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }


        public async Task<Modelos.response.Position[]> RetornaMenoresQueData(string data, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos <= DateTime.ParseExact(data, "yyyyMMdd_HHmmss", null))
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos <= DateTime.ParseExact(data, "yyyyMMdd_HHmmss", null) && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        public async Task<Modelos.response.Position[]> RetornaIguaisData(string data, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos == DateTime.ParseExact(data, "yyyyMMdd_HHmmss", null))
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos == DateTime.ParseExact(data, "yyyyMMdd_HHmmss", null) && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        public async Task<Modelos.response.Position[]> RetornaEntreData(string dataIni, string dataFim, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos >= DateTime.ParseExact(dataIni, "yyyyMMdd_HHmmss", null) && m.DataPos <= DateTime.ParseExact(dataFim, "yyyyMMdd_HHmmss", null))
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.DataPos >= DateTime.ParseExact(dataIni, "yyyyMMdd_HHmmss", null) && m.DataPos <= DateTime.ParseExact(dataFim, "yyyyMMdd_HHmmss", null) && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        #endregion

        public async Task<Corcovado.Modelos.response.Position[]> RetornaTodos()
        {
            var query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });




            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        #region POR ID
        public async Task<Modelos.response.Position[]> RetornaMaioresQueId(int id, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.Id)
                        where (m.Id >=id)
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.Id >= id && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        public async Task<Modelos.response.Position[]> RetornaMenoresQueId(int id, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.Id)
                        where (m.Id <= id)
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.Id <= id && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        public async Task<Modelos.response.Position[]> RetornaIguaisId(int id, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.Id)
                        where (m.Id ==id)
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.Id == id && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }

        public async Task<Modelos.response.Position[]> RetornaEntreId(int idIni, int idFim, string mobile = null)
        {
            var query = from m in _context.messageFiles
                        orderby (m.Id)
                        where (m.Id >= idIni && m.Id <= idFim)
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });

            if (!string.IsNullOrEmpty(mobile))
            {
                query = from m in _context.messageFiles
                        orderby (m.DataPos)
                        where (m.Id >= idIni && m.Id <= idFim && m.Mobile == mobile.Trim().ToUpper())
                        select (new
                        {
                            mobile = m.Mobile,
                            date = m.DataPos,
                            lat = m.Lat,
                            lon = m.Lon,
                            id = m.Id

                        });
            }


            await query.ToArrayAsync();
            IList<Corcovado.Modelos.response.Position> lista = new List<Modelos.response.Position>();

            foreach (var item in query)
            {
                lista.Add(new Corcovado.Modelos.response.Position
                {
                    mobile = item.mobile,
                    date = item.date.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = item.id.ToString(),
                    lat = item.lat,
                    lon = item.lon

                });

            }


            return lista.ToArray();
        }
        #endregion
    }
}
