using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace miosync.gpx.garmin
{

    /**
     **/
    public enum Sport_t
    {
        Running,
        Biking,
        Other
    }

    public enum CadenceSensorType_t
    {
        Footpod,
        Bike
    }
    /**
     * versionType
     **/
    public class Version_t
    {
        public int VersionMajor;
        public int VersionMinor;
        public int BuildMajor;
        public int BuildMinor;
    }

    public class Creator_t
    {
        public string Name;
        public uint UnitId;
        public ushort ProductID;
        public Version_t Version;
    }

    /**
     * HeartRateBpm_t
     **/
    public class HeartRateBpm_t
    {
        [XmlElement("Value")]
        public int Value;

        [XmlAttribute(AttributeName = "type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string type { get { return "HeartRateInBeatsPerMinute_t"; } set { }  }

        //[XmlNamespaceDeclarations]
        //public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public HeartRateBpm_t()
        {
            //xmlns.
            //xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            this.Value = 0;
        }
    }

    public class Position_t
    {
        [XmlElement("LatitudeDegrees")]
        public float LatitudeDegrees;

        [XmlElement("LongitudeDegrees")]
        public float LongitudeDegrees;
    }

    public class TPX_t
    {
        public float Speed;

        [XmlAttribute("CadenceSensor")]
        public CadenceSensorType_t CadenceSensor;

        [XmlAttribute(AttributeName = "xmlns")]
        public string xmlns { get { return "http://www.garmin.com/xmlschemas/ActivityExtension/v2"; } set { }  }

    }

    public class TrackpointExtensions_t
    {
        public TPX_t TPX;

        public TrackpointExtensions_t()
        {
            this.TPX = new TPX_t();
        }
    }

    public class Trackpoint_t
    {
        public string Time;
        public Position_t Position;
        public float AltitudeMeters;
        public float DistanceMeters;
        public HeartRateBpm_t HeartRateBpm;
        public int Cadence;
        
        [XmlElement("Extensions"), NonSerialized, XmlIgnore()]
        public TrackpointExtensions_t Extensions;

        [XmlElement("SensorState")]
        public string SensorState;

        /** default constructor for Trackpoint */
        public Trackpoint_t()
        {
            this.HeartRateBpm = new HeartRateBpm_t();
            this.Extensions = new TrackpointExtensions_t();
            this.Position = new Position_t();
        }
    }

    public class Track_t
    {
        [XmlElement("Trackpoint")]
        public Trackpoint_t[] Trackpoint;
    }

    public class Lap_t
    {
        public float TotalTimeSeconds;
        public float DistanceMeters;
        public float MaximumSpeed;
        public int Calories;
        public HeartRateBpm_t AverageHeartRateBpm;
        public HeartRateBpm_t MaximumHeartRateBpm;
        public string Intensity;
        public int Cadence;
        public string TriggerMethod;
        public Track_t Track;

        public Lap_t()
        {
            this.AverageHeartRateBpm = new HeartRateBpm_t();
            this.MaximumHeartRateBpm = new HeartRateBpm_t();
        }
        
        [XmlAttribute("StartTime")]
        public string StartTime;
    }

    public class Activity_t
    {
        public string Id;

        [XmlAttribute("Sport")]
        public Sport_t Sport;

        [XmlElement("Lap")]
        public Lap_t[] Lap;
        public Creator_t Creator;
    }

    /**
     * Activities
     **/
    public class Activities_t
    {
        [XmlElement("Activity")]
        public Activity_t activity;
    }

    public class Build_t
    {
        public Version_t Version;
        public string Type;
        public string Time;
        public string Builder;
    }

    //[XmlElement("Author")]
    public class Author_t
    {
        public string Name;
        public Build_t Build;
        public string LangID;
        public string PartNumber;
    }

    /**
     * GPX is the root element in the XML file.
     **/
    [XmlRoot("TrainingCenterDatabase")]
    public class TrainingCenterDatabase_t
    {
        [XmlElement("Activities")]
        public Activities_t Activities;

        [XmlElement("Author")]
        public Author_t Author;

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns;

        public TrainingCenterDatabase_t()
        {
            this.xmlns = new XmlSerializerNamespaces();
            this.xmlns.Add("", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            this.xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            this.xmlns.Add("schemaLocation", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2 http://www.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd");

            // custom namespaces!
            this.xmlns.Add("ns2", "http://www.garmin.com/xmlschemas/UserProfile/v2");
            this.xmlns.Add("ns3", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");
            this.xmlns.Add("ns4", "http://www.garmin.com/xmlschemas/ProfileExtension/v1");
            this.xmlns.Add("ns5", "http://www.garmin.com/xmlschemas/ActivityGoals/v1");
        }

    }
}
