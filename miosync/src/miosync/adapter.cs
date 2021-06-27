using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace miosync
{
    public enum AdapterType
    {
        NONE,
        PAUSE_DETECTION,
        STRAVA,
        SPORTSTRACKER,
        GARMIN,
        MTBFORUM
    };

    class adapter
    {
        public static Exception adapt(AdapterType t, String inputFile, String outputFile)
        {
            // ok .. file 1 selected; file 2 selected;
            try
            {
                StreamReader inStream = new StreamReader(inputFile);
                StreamWriter outStream = new StreamWriter(outputFile);

                adapter.adapt(t, inStream, outStream);

                inStream.Close();
                outStream.Close();
            }
            catch (Exception e)
            {
                return e;
            }

            return null;
        }


        public static bool process_EXTENSIONS(StreamReader r, StreamWriter w)
        {
            string line = null;
            string speed = string.Empty;
            string course = string.Empty;
            string acceleration = string.Empty;
            string cadence = string.Empty;
            string heartrate = string.Empty;
            
            do 
            {
                line = r.ReadLine();

                if (line.IndexOf("<speed") >= 0)
                    w.WriteLine(line);  // speed!
                else if (line.IndexOf("<course") >= 0)
                    course = line;
                else if (line.IndexOf("<acceleration") >= 0)
                    acceleration = line;
                else if (line.IndexOf("<cadence") >= 0)
                    cadence = line;
                else if (line.IndexOf("<heartrate") >= 0)
                    heartrate = line;
            } while (line.IndexOf("</extensions>") < 0);

            
            cadence = cadence.Replace("cadence", "gpxtpx:cad").Trim(); // transform cadence
            heartrate = heartrate.Replace("heartrate", "gpxtpx:hr").Trim(); // transform cadence

            string data = string.Format(
@"          <gpxtpx:TrackPointExtension>
              {0}          
              {1}
            </gpxtpx:TrackPointExtension>",
                cadence,
                heartrate);

            w.WriteLine(data);
            w.WriteLine(line);  // write end extensions
            return true;
        }

        public static bool process_TRKPT(StreamReader r, StreamWriter w)
        {
            string line = null;

            do 
            {
                line = r.ReadLine();

                w.WriteLine(line);
                if (line.IndexOf("<extensions>") >= 0)
                {
                    process_EXTENSIONS(r, w);
                }

            } while (line.IndexOf("</trkpt>") < 0); // when occur "trkpt break!"
            return true;
        }


        public static bool adapt(AdapterType type, StreamReader input, StreamWriter output)
        {
            string line = null;

            do
            {
                line = input.ReadLine();

                if (line == null)
                    continue;

                if (line.IndexOf("<gpx version") >= 0)
                {
                    if (type == AdapterType.NONE)
                    {
                        
                    }
                    else if (type == AdapterType.STRAVA)
                    {
                        line = "<gpx creator=\"StravaGPX\" version=\"1.1\" xmlns=\"http://www.topografix.com/GPX/1/1\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd\" xmlns:gpxtpx=\"http://www.garmin.com/xmlschemas/TrackPointExtension/v1\" xmlns:gpxx=\"http://www.garmin.com/xmlschemas/GpxExtensions/v3\">";
                    }
                    else if (type == AdapterType.SPORTSTRACKER)
                    {

                    }
                    else if (type == AdapterType.GARMIN)
                    {

                    }
                    else if (type == AdapterType.MTBFORUM)
                    {

                    }
                    output.WriteLine(line); // change gpx header
                }
                else if (line.IndexOf("<trkpt") >= 0)
                {   // track ...

                    output.WriteLine(line); // write trkpt header
                    process_TRKPT(input, output);   //  process block...
                }
                else
                    output.WriteLine(line); // write line without change!

            } while (line != null);

            return false;
        }

        public static AdapterType detectInputFile(string inputFile)
        {
            AdapterType r = AdapterType.NONE;   // standard mio!

            try
            {
                StreamReader reader = new StreamReader(inputFile);

                string line = reader.ReadLine();
                
                while(line != null && r == AdapterType.NONE)
                {
                    int begin = line.IndexOf("creator");

                    if (begin >= 0)   // identified CREATOR
                    {
                        begin += 9; // move on attribute value

                        int end = line.IndexOf('\"', begin);

                        string value = line.Substring(begin, end - begin).Trim();

                        switch (value)
                        {
                            case "StravaGPX":
                                r = AdapterType.STRAVA;
                                break;
                        }
                    }

                    line = reader.ReadLine();
                };

                reader.Close();
            }
            catch (Exception)
            {   // ignore this exception.. try to parse as MIO

            }

            return r;
        }

        public static object fromMIO(XmlSerializer deserializer, StreamReader reader, AdapterType outputType)
        {
            // read input file
            gpx.miogpx input = (gpx.miogpx)deserializer.Deserialize(reader);

            object result = null;
            switch (outputType)
            {
                case AdapterType.GARMIN:
                    result = (object)gpx.convert.toGarmin(input);
                    //serializer.Serialize()
                    break;
                case AdapterType.MTBFORUM:
                case AdapterType.SPORTSTRACKER:
                case AdapterType.STRAVA:
                    result = (object)gpx.convert.toStrava(input);
                    break;
                case AdapterType.PAUSE_DETECTION:
                    result = (object)gpx.convert.removeZero(input);
                    break;
                default:
                    throw new Exception("Metodo di conversione non supportato");
            }

            return result;
        }

        public static object fromStrava(XmlSerializer deserializer, StreamReader reader, AdapterType outputType)
        {
            // read input file
            gpx.strava.gpx_t input = (gpx.strava.gpx_t)deserializer.Deserialize(reader);

            object result = null;
            switch (outputType)
            {
                case AdapterType.GARMIN:
                    result = (object)gpx.convert.toGarmin(input);
                    //serializer.Serialize()
                    break;
                /*case AdapterType.MTBFORUM:
                case AdapterType.SPORTSTRACKER:
                case AdapterType.STRAVA:
                    //result = (object)gpx.convert.toStrava(input);
                    throw new Exception("Metodo di conversione non supportato");
                    break;
                case AdapterType.PAUSE_DETECTION:
                    //result = (object)gpx.convert.removeZero(input);
                    throw new Exception("Metodo di conversione non supportato");
                    break;*/
                default:
                    throw new Exception("Metodo di conversione non supportato");
            }

            return result;
        }
        public static void Convert(string inputFile, string outputFile, AdapterType outputType)
        {
            System.Xml.Serialization.XmlSerializer serializer = null;
            System.IO.StreamReader reader = null;
            System.IO.StreamWriter writer = null;

            switch(outputType)
            {
                case AdapterType.GARMIN:
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.garmin.TrainingCenterDatabase_t));
                    break;
                case AdapterType.STRAVA:
                case AdapterType.SPORTSTRACKER:
                case AdapterType.MTBFORUM:
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.strava.gpx_t));
                    break;
                case AdapterType.PAUSE_DETECTION:
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.miogpx));
                    break;
                case AdapterType.NONE:
                    break;
            }

            if (serializer == null)
            {   // no serialization -> transfer simple file!
                System.IO.File.Copy(inputFile, outputFile); // 
            }
            else
            {
                try
                {
                    AdapterType inputType = adapter.detectInputFile(inputFile);

                    if (inputType == outputType)
                    {   // path non gestito!
                        throw new Exception("Errore: Formato file input uguale a formato file output.");
                    }
                    System.Xml.Serialization.XmlSerializer deserializer = null;

                    object result = null;
                    reader = new StreamReader(inputFile);
                    writer = new StreamWriter(outputFile);


                    if (inputType == AdapterType.NONE)
                    {
                        deserializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.miogpx));

                        result = fromMIO(deserializer, reader, outputType);
                    }
                    else if (inputType == AdapterType.STRAVA)
                    {
                        deserializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.strava.gpx_t));

                        result = fromStrava(deserializer, reader, outputType);
                    }

                    MemoryStream tempStream = new MemoryStream();

                    serializer.Serialize(tempStream, result);

                    tempStream.Position = 0;

// adjust "StravaGPS"

                    StreamReader textReader = new StreamReader(tempStream);

                    string line = null;

                    do
                    {
                        line = textReader.ReadLine();

                        if (line == null)
                            continue;

                        if (line.IndexOf("StravaGPS") >= 0)
                        {
                            line = "<gpx creator=\"StravaGPX\" version=\"1.1\" xmlns=\"http://www.topografix.com/GPX/1/1\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd\" xmlns:gpxtpx=\"http://www.garmin.com/xmlschemas/TrackPointExtension/v1\" xmlns:gpxx=\"http://www.garmin.com/xmlschemas/GpxExtensions/v3\">";
                        }

                        if (line.IndexOf("<TrainingCenterDatabase") >= 0)
                        {
                            line = "<TrainingCenterDatabase xmlns=\"http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2 http://www.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd\">";
                        }

                        writer.WriteLine(line);
                    } while (line != null);

                    tempStream.Close();
                    writer.Close();
                    reader.Close();
                    
                }
                catch (Exception exception)
                {
                    if (reader != null)
                        reader.Close();

                    if (writer != null)
                    {   // delete output file!
                        writer.Close();
                        System.IO.File.Delete(outputFile);
                    }

                    throw exception;
                }
            }
        }



    }
}
