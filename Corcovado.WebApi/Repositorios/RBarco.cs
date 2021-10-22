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
    public class RBarco : IBarco
    {
        private readonly DPSyncContext _context;


        public RBarco(DPSyncContext context)
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



      

     

        #region EXPECIFICOS

        public async Task<Barco[]> Ativados()
        {
            IQueryable<Barco> query = _context.barcos;



            query = query.AsNoTracking().Where(x => x.esn_global.ToUpper().Trim() != "D")
                        .OrderBy(c => c.nome_eais);

            return await query.ToArrayAsync();
        }

        public async Task<Barco> Ativar(string barco)
        {
            barco = barco.Trim().ToUpper();
            Barco b = await _context.barcos.FirstOrDefaultAsync(x => x.nome_eais.ToUpper().Trim() == barco || x.nome_global.ToUpper().Trim() == barco);
            if (b != null)
            {
                b.esn_global = "A";
                _context.barcos.Update(b);
                await _context.SaveChangesAsync();
            }
            return b;
        }

        public async Task<Barco> Desativar(string barco)
        {
            barco = barco.Trim().ToUpper();
            Barco b = await _context.barcos.FirstOrDefaultAsync(x => x.nome_eais.ToUpper().Trim() == barco || x.nome_global.ToUpper().Trim() == barco);
            if (b != null)
            {
                b.esn_global = "D";
                _context.barcos.Update(b);
                await _context.SaveChangesAsync();
            }
            return b;
        }

        public async Task<Barco[]> Desativados()
        {
            IQueryable<Barco> query = _context.barcos;



            query = query.AsNoTracking().Where(x => x.esn_global.ToUpper().Trim() == "D")
                        .OrderBy(c => c.nome_eais);

            return await query.ToArrayAsync();
        }
        #endregion


        public async Task<Barco[]> RetornarTodos()
        {
            IQueryable<Barco> query = _context.barcos;



            query = query.AsNoTracking()
                        .OrderBy(c => c.nome_eais);

            return await query.ToArrayAsync();
        }




    }
}
