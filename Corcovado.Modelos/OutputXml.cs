using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Corcovado.Modelos
{
	[XmlRoot(ElementName = "position")]
	public class OutputXml
	{

		[XmlElement(ElementName = "mobile")]
		public string Mobile { get; set; }

		[XmlElement(ElementName = "date")]
		public string Date { get; set; }

		[XmlElement(ElementName = "lat")]
		public double Lat { get; set; }

		[XmlElement(ElementName = "lon")]
		public double Lon { get; set; }

		[XmlElement(ElementName = "velocity")]
		public double Velocity { get; set; }

		[XmlElement(ElementName = "course")]
		public double Course { get; set; }

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }
	}

}
