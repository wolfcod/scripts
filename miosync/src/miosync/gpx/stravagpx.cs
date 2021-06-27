using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace miosync.gpx.strava
{
    /**
 * metadata
 * Information about the GPX file, author, and copyright restrictions 
 * goes in the metadata section. Providing rich, meaningful information 
 * about your GPX files allows others to search for and use your GPS data.
 **/
    public class metadata_t
    {
        private string _name = string.Empty;
        private string _desc = string.Empty;
        private string _author = string.Empty;
        private string _copyright = string.Empty;
        private string _link = string.Empty;
        private string _time = string.Empty;
        private string _keywords = string.Empty;
        private string _bounds = string.Empty;
        private string _extensions = string.Empty;

        /** 
         * The name of the GPX file.
         **/
        [XmlElement("name")]
        public string Name { get { return this._name; } set { this._name = value; } }

        [XmlElement("time")]
        public string Time { get { return this._time; } set { this._time = value; } }


    }

    public class trk_extensions_t
    {
        public string profile;
        public string time;
        public int length;  // length in tracks?
        public int timelength;
        public float minlat;
        public float minlon;
        public float maxlat;
        public float maxlon;
        public float avgspeed;
        public float maxspeed;
        public string minacceleration;
        public string maxacceleration;
        public int minaltitude;
        public int maxaltitude;
        public int totalascent;
        public int totaldescent;
        public int calories;
        public float avggrade;
        public int avgcadence;
        public int maxcadence;
        public int avgheartrate;
        public int minheartrate;
        public int maxheartrate;
    }

    [XmlRoot("TrackPointExtensions")]
    public class TrackPointExtensions_t
    {
        [XmlElement("cad")]
        public int Cadence;

        [XmlElement("hr")]
        public int Heartrate;
    }
    public class trk_trkseg_trkpk_extensions_t
    {
        public float speed;
        //public string course;
        //public string acceleration;
        //public int cadence;
        //public int heartrate;
        [XmlElement(ElementName="TrackPointExtension", Namespace="http://www.garmin.com/xmlschemas/TrackPointExtension/v1")]
        public TrackPointExtensions_t TrackPointExtension;

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns;
        public trk_trkseg_trkpk_extensions_t()
        {
            this.xmlns = new XmlSerializerNamespaces();
            this.xmlns.Add("gpxtpx", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");

            this.TrackPointExtension = new TrackPointExtensions_t();
        }
    }

    // single trackpoint of a track
    public class trkpt_t
    {
        [XmlAttribute("lat")]
        public float lat;

        [XmlAttribute("lon")]
        public float lon;

        public float ele;
        public string time;

        public trk_trkseg_trkpk_extensions_t extensions;

        public trkpt_t()
        {
            this.extensions = new trk_trkseg_trkpk_extensions_t();
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

        [XmlElement("type")]
        public string type;

        [XmlElement("desc")]
        public string Description;

        [XmlElement("cmt")]
        public string cmt;

        [XmlElement("extensions")]
        public trk_extensions_t extensions;

        [XmlElement("trkseg")]
        public trkseg_t[] trkseg;
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

        [XmlElement("metadata")]
        public metadata_t metadata;
        public trk_t trk;

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns;
        
        public gpx_t()
        {
            this.xmlns = new XmlSerializerNamespaces();
            //this.xmlns.Add("", "http://www.topografix.com/GPX/1/1");
            this.xmlns.Add("schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd");
            this.xmlns.Add("gpxtpx", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
            this.xmlns.Add("gpxx", "http://www.garmin.com/xmlschemas/GpxExtensions/v3");

        }
    }
}
