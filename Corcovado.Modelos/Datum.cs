using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.Modelos
{
    public class Datum
    {
        public string nome { get; set; }
        public double a { get; set; }
        public double b { get; set; }

        public double f { get; set; }
        public Datum(string nome, double a, double b)
        {
            this.a = a;
            this.b = b;
            this.nome = nome;
            this.f = (this.a-this.b)/this.a;
        }
    }
}
