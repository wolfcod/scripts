using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace miosync.gpx.topografix
{
    [XmlRoot("wpt")]
    public class wpt_t
    {
        [XmlAttribute("lat")]
        public float lat;

        [XmlAttribute("lon")]
        public float lon;

        [XmlElement("ele")]
        public int ele;

        [XmlElement("name")]
        public string name;

    }

    // single trackpoint of a track
    public class trkpt_t
    {
        [XmlAttribute("lat")]
        public float lat;

        [XmlAttribute("lon")]
        public float lon;

        public float ele;


        public override string ToString()
        {
            return string.Format("{0}:{1}", lat, lon);
        }
   }

    public class trkseg_t
    {
        [XmlElement("trkpt")]
        public trkpt_t[] trkpt;
    }


    /**
     * trk container
     **/
    public class trk_t
    {
        [XmlElement("name")]
        public string name;

        [XmlElement("trkseg")]
        public trkseg_t trkseg;
    }


    /**
     * GPX is the root element in the XML file.
     **/
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/1", IsNullable = false)]
    public class gpx_t
    {
        [XmlAttribute("creator")]
        public string creator;

        [XmlAttribute("version")]
        public string version;

        /*[XmlElement("wpt")]
        public wpt_t wpt;*/

        [XmlElement("trk")]
        public trk_t trk;
                
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns;

        
        public gpx_t()
        {
            this.xmlns = new XmlSerializerNamespaces();
            //this.xmlns.Add("", "http://www.topografix.com/GPX/1/1");
            this.xmlns.Add("schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
            this.xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            
        }
    }
}
