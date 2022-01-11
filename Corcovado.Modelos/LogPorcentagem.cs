using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.Modelos
{
    public class LogPorcentagem
    {
        public double Porcentagem { get; set; }
        public int Realizado { get; set; }
        public int Esperado { get; set; }
        public int Posicoes { get; set; }
        public int QtdMobile { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
