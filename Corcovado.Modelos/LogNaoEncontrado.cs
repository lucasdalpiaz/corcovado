using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.Modelos
{
    public class LogNaoEncontrado
    {
        public int QtdFalha { get; set; }
        public string mobile { get; set; }
        public int Id { get; set; }
        public int IdAnterior { get; set; }
        public DateTime DataPos { get; set; }
        public DateTime DataPosAnterior { get; set; }
        [NotMapped]
        public DateTime DataCriacao { get; set; }
    }
}
