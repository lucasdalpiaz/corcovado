using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.Modelos
{
    public class EAIS
    {
        public double id { get; set; }
        [NotMapped]
        public int id_geral { get; set; }
        public string vessel_name { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double sog { get; set; }
        public double cog { get; set; }
        public double rot { get; set; }
        public double heading { get; set; }
        public DateTime dt_pos_utc { get; set; }
    }
}
