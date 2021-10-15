using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Corcovado.Modelos
{
	// using System.Xml.Serialization;
	// XmlSerializer serializer = new XmlSerializer(typeof(FeatureCollection));
	// using (StringReader reader = new StringReader(xml))
	// {
	//    var test = (FeatureCollection)serializer.Deserialize(reader);
	// }

	[XmlRoot(ElementName = "Point", Namespace = "http://www.opengis.net/gml")]
	public class Point
	{

		[XmlElement(ElementName = "pos", Namespace = "http://www.opengis.net/gml")]
		public string Pos;

		[XmlAttribute(AttributeName = "srsName", Namespace = "")]
		public string SrsName;

		[XmlAttribute(AttributeName = "srsDimension", Namespace = "")]
		public int SrsDimension;

		[XmlText]
		public string Text;
	}

	[XmlRoot(ElementName = "position", Namespace = "https://services.exactearth.com/gws")]
	public class Position
	{

		[XmlElement(ElementName = "Point", Namespace = "http://www.opengis.net/gml")]
		public Point Point;
	}

	[XmlRoot(ElementName = "LVI", Namespace = "https://services.exactearth.com/gws")]
	public class LVI
	{

		[XmlElement(ElementName = "mmsi", Namespace = "https://services.exactearth.com/gws")]
		public int Mmsi;

		[XmlElement(ElementName = "imo", Namespace = "https://services.exactearth.com/gws")]
		public int Imo;

		[XmlElement(ElementName = "vessel_name", Namespace = "https://services.exactearth.com/gws")]
		public string VesselName;

		[XmlElement(ElementName = "callsign", Namespace = "https://services.exactearth.com/gws")]
		public string Callsign;

		[XmlElement(ElementName = "vessel_type", Namespace = "https://services.exactearth.com/gws")]
		public string VesselType;

		[XmlElement(ElementName = "vessel_type_code", Namespace = "https://services.exactearth.com/gws")]
		public int VesselTypeCode;

		[XmlElement(ElementName = "vessel_type_cargo", Namespace = "https://services.exactearth.com/gws")]
		public object VesselTypeCargo;

		[XmlElement(ElementName = "vessel_class", Namespace = "https://services.exactearth.com/gws")]
		public string VesselClass;

		[XmlElement(ElementName = "length", Namespace = "https://services.exactearth.com/gws")]
		public int Length;

		[XmlElement(ElementName = "width", Namespace = "https://services.exactearth.com/gws")]
		public int Width;

		[XmlElement(ElementName = "flag_country", Namespace = "https://services.exactearth.com/gws")]
		public string FlagCountry;

		[XmlElement(ElementName = "flag_code", Namespace = "https://services.exactearth.com/gws")]
		public int FlagCode;

		[XmlElement(ElementName = "destination", Namespace = "https://services.exactearth.com/gws")]
		public string Destination;

		[XmlElement(ElementName = "eta", Namespace = "https://services.exactearth.com/gws")]
		public int Eta;

		[XmlElement(ElementName = "draught", Namespace = "https://services.exactearth.com/gws")]
		public double Draught;

		[XmlElement(ElementName = "position", Namespace = "https://services.exactearth.com/gws")]
		public Position Position;

		[XmlElement(ElementName = "longitude", Namespace = "https://services.exactearth.com/gws")]
		public double Longitude;

		[XmlElement(ElementName = "latitude", Namespace = "https://services.exactearth.com/gws")]
		public double Latitude;

		[XmlElement(ElementName = "sog", Namespace = "https://services.exactearth.com/gws")]
		public double Sog;

		[XmlElement(ElementName = "cog", Namespace = "https://services.exactearth.com/gws")]
		public double Cog;

		[XmlElement(ElementName = "rot", Namespace = "https://services.exactearth.com/gws")]
		public double Rot;

		[XmlElement(ElementName = "heading", Namespace = "https://services.exactearth.com/gws")]
		public double Heading;

		[XmlElement(ElementName = "nav_status", Namespace = "https://services.exactearth.com/gws")]
		public string NavStatus;

		[XmlElement(ElementName = "nav_status_code", Namespace = "https://services.exactearth.com/gws")]
		public int NavStatusCode;

		[XmlElement(ElementName = "source", Namespace = "https://services.exactearth.com/gws")]
		public string Source;

		[XmlElement(ElementName = "ts_pos_utc", Namespace = "https://services.exactearth.com/gws")]
		public double TsPosUtc;

		[XmlElement(ElementName = "ts_static_utc", Namespace = "https://services.exactearth.com/gws")]
		public double TsStaticUtc;

		[XmlElement(ElementName = "ts_insert_utc", Namespace = "https://services.exactearth.com/gws")]
		public double TsInsertUtc;

		[XmlElement(ElementName = "dt_pos_utc", Namespace = "https://services.exactearth.com/gws")]
		public string DtPosUtc;

		[XmlElement(ElementName = "dt_static_utc", Namespace = "https://services.exactearth.com/gws")]
		public string DtStaticUtc;

		[XmlElement(ElementName = "dt_insert_utc", Namespace = "https://services.exactearth.com/gws")]
		public string DtInsertUtc;

		[XmlElement(ElementName = "vessel_type_main", Namespace = "https://services.exactearth.com/gws")]
		public string VesselTypeMain;

		[XmlElement(ElementName = "vessel_type_sub", Namespace = "https://services.exactearth.com/gws")]
		public string VesselTypeSub;

		[XmlElement(ElementName = "message_type", Namespace = "https://services.exactearth.com/gws")]
		public int MessageType;

		[XmlElement(ElementName = "eeid", Namespace = "https://services.exactearth.com/gws")]
		public double Eeid;

		[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml")]
		public double Id;

		[XmlText]
		public string Text;
	}

	[XmlRoot(ElementName = "featureMembers", Namespace = "http://www.opengis.net/gml")]
	public class FeatureMembers
	{

		[XmlElement(ElementName = "LVI", Namespace = "https://services.exactearth.com/gws")]
		public List<LVI> LVI;
	}

	[XmlRoot(ElementName = "FeatureCollection", Namespace = "http://www.opengis.net/wfs")]
	public class FeatureCollection
	{

		[XmlElement(ElementName = "featureMembers", Namespace = "http://www.opengis.net/gml")]
		public FeatureMembers FeatureMembers;

		[XmlAttribute(AttributeName = "xs", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xs;

		[XmlAttribute(AttributeName = "wfs", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Wfs;

		[XmlAttribute(AttributeName = "gml", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Gml;

		[XmlAttribute(AttributeName = "exactAIS", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string ExactAIS;

		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi;

		[XmlAttribute(AttributeName = "numberOfFeatures", Namespace = "")]
		public int NumberOfFeatures;

		[XmlAttribute(AttributeName = "timeStamp", Namespace = "")]
		public DateTime TimeStamp;

		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation;

		[XmlText]
		public string Text;
	}


}
