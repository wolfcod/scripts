using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace miosync
{
    class merge
    {

        public void FixTime(string gpxfile, string textfile, string outputfile)
        {
            System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.miogpx));
            gpx.miogpx mio = (gpx.miogpx) deserializer.Deserialize(new System.IO.StreamReader(gpxfile));
            
            System.IO.MemoryStream memorystream = new System.IO.MemoryStream();
            System.IO.FileStream textstream = new System.IO.FileStream(textfile, System.IO.FileMode.Open);
            System.IO.StreamWriter output = new System.IO.StreamWriter(outputfile);

            byte[] buffer = new byte[textstream.Length];

            textstream.Read(buffer, 0, (int)textstream.Length);

            memorystream.Write(buffer, 0, (int)textstream.Length);

            textstream.Close();

            memorystream.Seek(0, System.IO.SeekOrigin.Begin);

            System.IO.StreamReader r = new System.IO.StreamReader(memorystream);


            //System.Diagnostics.Debug.WriteLine(topografix.trk.trkseg.trkpt[0].ele);

            int x = 0;

            int trkpt_count = 0;

            foreach (gpx.trkseg_t seg in mio.trk.trkseg)
                trkpt_count += seg.trkpt.Length;

            gpx.trkpt_t[] tracks = new gpx.trkpt_t[trkpt_count];

            trkpt_count = 0;

            // create an unique array of elements...
            foreach (gpx.trkseg_t seg in mio.trk.trkseg)
            {
                foreach (gpx.trkpt_t t in seg.trkpt)
                {
                     tracks[trkpt_count++] = t;
                }
            }

            string l = null;
            float i = 0;
            int pos = 0;

            while ((l = r.ReadLine()) != null)
            {
                DateTime time = gpx.convert.StringAsDateTime(tracks[pos].time);
                
                float difference = (352.0f - (pos * 0.0757477942758769f)) * (-1.0f);

                time = time.AddSeconds(Convert.ToInt32(difference));

                l = l.Replace("<time>2013-09-22T00:00:00Z</time>", string.Format("<time>{0}</time></trkpt>", gpx.convert.DateTimeAsString(time)));
                i = i + 1.64321067355283f;

                pos = Convert.ToInt32(i);
                output.WriteLine(l);
            }

            output.Close();

        }



        public void TrackToTime(string trackfile, string gpsfile, string output)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.miogpx)); ;
            System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx.topografix.gpx_t));

            gpx.topografix.gpx_t topografix = (gpx.topografix.gpx_t)deserializer.Deserialize(new System.IO.StreamReader(trackfile));
            gpx.miogpx mio = (gpx.miogpx) serializer.Deserialize(new System.IO.StreamReader(gpsfile));


            System.Diagnostics.Debug.WriteLine(topografix.trk.trkseg.trkpt[0].ele);

            int x = 0;
            
            float path_points = 0;
            float path_distance = 0;

            int trkpt_count = 0;

            foreach (gpx.trkseg_t seg in mio.trk.trkseg)
                trkpt_count += seg.trkpt.Length;

            gpx.trkpt_t[] tracks = new gpx.trkpt_t[trkpt_count*2];

            trkpt_count = 0;

            // explode!
            foreach (gpx.trkseg_t seg in mio.trk.trkseg)
            {
                foreach (gpx.trkpt_t t in seg.trkpt)
                {
                    DateTime date = gpx.convert.StringAsDateTime(t.time);

                    if (date.Year == 2009)
                    {
                        date = date.AddHours(10.5);
                        date = date.AddYears(4);
                        date = date.AddMonths(7);
                        date = date.AddDays(7);
                        t.time = gpx.convert.DateTimeAsString(date);
                    }

                    gpx.trkpt_t t1 = new gpx.trkpt_t();

                    t1.ele = t.ele;
                    t1.extensions = t.extensions;
                    t1.lat = t.lat;
                    t1.lon = t.lon;
                    
                    date = gpx.convert.StringAsDateTime(t.time);
                    date = date.AddSeconds(1);
                    t1.time = gpx.convert.DateTimeAsString(date);

                    tracks[trkpt_count] = t;
                    tracks[trkpt_count + 1] = t1;

                    trkpt_count+=2;
                }
            }

            gpx.miogpx miofile = new gpx.miogpx();

            miofile.metadata = new gpx.metadata_t();
            miofile.trk = new gpx.trk_t();

            miofile.trk.trkseg = new gpx.trkseg_t[1];
            miofile.trk.trkseg[0] = new gpx.trkseg_t();

            miofile.trk.trkseg[0].trkpt = new gpx.trkpt_t[topografix.trk.trkseg.trkpt.Length];

            DateTime start = new DateTime(2013, 9, 22, 8, 0, 0);

            for (int i = 0; i < topografix.trk.trkseg.trkpt.Length - 1 && x < trkpt_count; i++)
            {
                gpx.topografix.trkpt_t point0 = topografix.trk.trkseg.trkpt[i];
                gpx.topografix.trkpt_t point1 = topografix.trk.trkseg.trkpt[i+1];
                
                float f = gpx.convert.VincentFormula(point0.lat, point0.lon, point1.lat, point1.lon);

                path_distance += f;

                gpx.trkpt_t p0 = null;

                while(path_points < path_distance)
                {
                    if (x >= trkpt_count)
                        break;

                    p0 = tracks[x];
                    path_points += p0.extensions.speed;
                    x++;
                }

                System.Diagnostics.Debug.WriteLine(string.Format("mt {0} - speed {1} / used points {2} {3}", path_distance, path_points, x, point0.ToString()));

                if (p0 != null)
                {
                    p0.lat = point0.lat;
                    p0.lon = point0.lon;
                    miofile.trk.trkseg[0].trkpt[i] = p0;


                    //if (p0.time.)
                }


                //delta.extensions = new gpx.trk_trkseg_trkpk_extensions_t();
            }

            serializer.Serialize(new System.IO.StreamWriter(output), miofile);
        }

        
    }
}
