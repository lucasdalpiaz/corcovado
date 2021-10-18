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
    }
}
