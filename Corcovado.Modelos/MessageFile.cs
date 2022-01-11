
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Corcovado.Modelos
{
    public  class MessageFile
    {
        public int Id { get; set; }
        #region Inputs
        public string InputXml { get; set; }
        [JsonIgnore]
        public string Esn { get; set; }
        public string Unixtime { get; set; }
        public string Payload { get; set; }
        #endregion


        #region Outputs
        public string OutputXml { get; set; }
        public string OutputCsv { get; set; }
        public string Mobile { get; set; }
        public string DataConvertida { get; set; }
        public DateTime DataPos { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Tipo { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        #endregion
        [JsonIgnore]
        public string Obs { get; set; }



    }
}
