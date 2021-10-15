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
    public class RLog : ILog
    {

        private readonly DPSyncContext _context;


        public RLog(DPSyncContext context)
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
        public async Task<Log[]> GetAllLogsAsync()
        {
            IQueryable<Log> query = _context.logs;

       

            query = query.AsNoTracking()
                        .OrderBy(c => c.dataCriacao);

            return await query.ToArrayAsync();
        }
    }
}
