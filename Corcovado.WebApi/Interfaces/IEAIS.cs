using Corcovado.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.WebApi.Interfaces
{
    public interface IEAIS : IGeral
    {
        Task<bool> RealizarPricedimento();

        Task<EAIS[]> GetAllEais();
    }
}
