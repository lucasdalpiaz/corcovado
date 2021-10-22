using Corcovado.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.WebApi.Interfaces
{
    public interface IMessageFile : IGeral
    {
        Task<Corcovado.Modelos.response.Position[]> RetornaMaioresQueData(string data, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaMenoresQueData(string data, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaIguaisData(string data, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaEntreData(string dataIni, string dataFim, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaTodos();

        Task<Corcovado.Modelos.response.Position[]> RetornaMaioresQueId(int id, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaMenoresQueId(int id, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaIguaisId(int id, string mobile = null);
        Task<Corcovado.Modelos.response.Position[]> RetornaEntreId(int idIni, int idFim, string mobile = null);
    }
}
