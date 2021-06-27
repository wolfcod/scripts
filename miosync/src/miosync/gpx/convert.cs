using System;
using System.Collections.Generic;
using System.Globalization;

using System.Text;

namespace miosync.gpx
{
    class convert
    {

        public static DateTime StringAsDateTime(string s)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            
            //""2013-05-26T10:39:41Z"

            char [] seperator = { '-', 'T', ':', 'Z'};

            string [] date = s.Split(seperator);

            return new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]),
                Convert.ToInt32(date[3]), Convert.ToInt32(date[4]), Convert.ToInt32(date[5]));
        }

        public static string DateTimeAsString(DateTime t)
        {
            return string.Format("{0,4}-{1,2}-{2,2}T{3,2}:{4,2}:{5,2}Z",
                t.Year,
                t.Month,
                t.Day,
                t.Hour,
                t.Minute,
                t.Second).Replace(' ', '0');
        }

        /**
         * $+buildVersion:Version_t
         * create a new Version object
         **/
        public static garmin.Version_t buildVersion()
        {
            garmin.Version_t v = new garmin.Version_t();

            v.VersionMinor = 2;
            v.VersionMajor = 3;
            v.BuildMajor = 3;
            v.BuildMinor = 0;

            return v;
        }

        /**
         *
         **/
        public static float DistanceMeters(garmin.Track_t track)
        {
            float f = 0;

            foreach (garmin.Trackpoint_t tp in track.Trackpoint)
            {
                f += tp.DistanceMeters;
            }

            return f;
        }

        /**
         * $+AverageHeartRateBmp:int
         * return the average of heart rate/bmp recorded into trackpoints of "track"
         **/
        public static int AverageHeartRateBpm(garmin.Track_t track)
        {
            int hrbmp = 0;

            foreach (garmin.Trackpoint_t tp in track.Trackpoint)
            {
                hrbmp += tp.HeartRateBpm.Value;
            }

            hrbmp /= track.Trackpoint.Length;   // a fast way to get this value!
            return hrbmp;
        }

        /**
         *
        **/
        public static int MaxHeartRateBmp(garmin.Track_t track)
        {
            int max = 0;

            foreach (garmin.Trackpoint_t tp in track.Trackpoint)
                if (tp.HeartRateBpm.Value > max)
                    max = tp.HeartRateBpm.Value;

            return max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        internal static int CadenceAverage(garmin.Track_t track)
        {
            int sum = 0;
            int steps = 0;

            foreach(garmin.Trackpoint_t tp in track.Trackpoint)
                if (tp.Cadence > 0)
                {
                    sum += tp.Cadence;
                    steps++;
                }

            if (steps == 0 || sum == 0)
                return 0;

            return sum / steps;
        }

        public static float VincentFormula(float lat1, float lon1, float lat2, float lon2)
        {
            double a = 6378137.0f, b = 6356752.3142,  f = 1/298.257223563;
            double rad = Math.PI / 180.0f, lon_rad = (lon2 - lon1) * rad;
            double U1 = Math.Atan((1 - f) * Math.Tan(lat1 * rad));
            double U2 = Math.Atan((1 - f) * Math.Tan(lat2 * rad));
            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);
            double cosSqAlpha = 0, cos2SigmaM = 0, sinSigma = 0, cosSigma = 0 , sigma =0 , sinAlpha=0;
            double cosLambda, sinLambda;
            double uSq, A, B, C, deltaSigma, s;
            double lambda = lon_rad, lambdaP = 2 * Math.PI;
            int iterations = 20;

            try {
            
            while (Math.Abs(lambda-lambdaP) > 0.000000000001f && --iterations > 0) 
            {
                sinLambda = Math.Sin(lambda);
                cosLambda = Math.Cos(lambda);

                sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) +
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 *
                    sinU2 - sinU1 * cosU2 * cosLambda));

                if (sinSigma == 0.0f)
                    return 0.0f;  // I punti sono coincidenti

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);

                sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;

                cosSqAlpha = 1 - sinAlpha * sinAlpha;
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2/cosSqAlpha;

                //if (isnan(cos2SigmaM)) cos2SigmaM = 0;

                C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));

                lambdaP = lambda;
                lambda = lon_rad + (1-C) * f * sinAlpha *
                    (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1
                    + 2 * cos2SigmaM * cos2SigmaM)));
            }

            if (iterations == 0)
              return 0.0f;  // La formula non e' riuscita a convergere

            uSq = cosSqAlpha * (a * a - b * b) / (b * b);

            A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 *
            uSq)));
            B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma *
            (-1 + 2 * cos2SigmaM * cos2SigmaM)-
            B / 6* cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4
            * cos2SigmaM * cos2SigmaM)));

            s = b * A * (sigma - deltaSigma);
            return Convert.ToSingle(s);
            } catch(Exception) {
                return 0.0f;
            }
        }

        public static garmin.TrainingCenterDatabase_t toGarmin(gpx.miogpx gpx)
        {
            garmin.TrainingCenterDatabase_t tcx = new garmin.TrainingCenterDatabase_t();

            garmin.Build_t build = new garmin.Build_t();

            build.Version = convert.buildVersion();

            garmin.Author_t author = new garmin.Author_t();

            author.Build = build;
            author.LangID = "IT";       // IT by default
            author.Name = gpx.trk.extensions.profile;   // profile
            author.PartNumber = gpx.metadata.Time;
            author.Build.Builder = "MIOSYNC";
            author.Build.Time = DateTime.Today.ToString();
            author.Build.Type = "Release";

            tcx.Author = author;

            garmin.Creator_t creator = new garmin.Creator_t();

            creator.Name = "EDGE305";
            creator.ProductID = 450;
            creator.UnitId = 1111111111;
            creator.Version = convert.buildVersion();
            creator.Version.VersionMajor = 2;
            creator.Version.VersionMinor = 90;
            creator.Version.BuildMajor = 0;
            creator.Version.BuildMinor = 0;

            // Activities
            garmin.Activities_t activies = new garmin.Activities_t();

            garmin.Activity_t activity = new garmin.Activity_t();

            activity.Sport = garmin.Sport_t.Biking;
            activity.Id = gpx.metadata.Time;
            activity.Creator = creator;

            activity.Lap = new garmin.Lap_t[gpx.trk.trkseg.Length];
            int lapEntry = 0;

            foreach (trkseg_t trkseg in gpx.trk.trkseg)
            {
                activity.Lap[lapEntry] = new garmin.Lap_t();

                garmin.Lap_t lap = activity.Lap[lapEntry];

                lap.Intensity = "Active";
                lap.TriggerMethod = "Manual";

                // fill lap with trkseg!

                lap.Track = new garmin.Track_t();

                lap.Track.Trackpoint = new garmin.Trackpoint_t[trkseg.trkpt.Length];

                int trkpEntry = 0;

                float distanceMeters = 0.0F;
                trkpt_t prev_t = null;
                DateTime minTime = convert.StringAsDateTime(trkseg.trkpt[0].time);
                DateTime maxTime = convert.StringAsDateTime(trkseg.trkpt[trkseg.trkpt.Length - 1].time);

                foreach (trkpt_t trkpt in trkseg.trkpt)
                {   // process trackpoint
                    lap.Track.Trackpoint[trkpEntry] = new garmin.Trackpoint_t();
                    garmin.Trackpoint_t trackpoint = lap.Track.Trackpoint[trkpEntry];

                    trackpoint.AltitudeMeters = trkpt.ele;
                    trackpoint.Cadence = trkpt.extensions.cadence;

                    //////////////////////////////////////////////////////////////////////////
                    //
                    //if (prev_t != null)
                    //{   // calcola la distanza
                    //    float decLatA = prev_t.lat;
                    //    float decLonA = prev_t.lon;
                    //    float decLatB = trkpt.lat;
                    //    float decLonB = trkpt.lon;
                    //    float pi = 3.1415927F;
                    //    float r = 6372.795477598F;
                    //    float radLatA = pi * decLatA / 180.0F;
                    //    float radLonA = pi * decLonA / 180.0F;
                    //    float radLatB = pi * decLatA / 180.0F;
                    //    float radLonB = pi * decLonB / 180.0F;
                    //    float phi = Math.Abs(radLonA - radLonB);
                    //    double p = Math.Acos(( (Math.Sin(radLatA) * Math.Sin(radLatB)) + (Math.Cos(radLatA) * Math.Cos(radLatB) * Math.Cos(phi))));
                    //    float distance = Convert.ToSingle( p * r * 1000.0F);

                    //    if (distance < trkpt.extensions.speed)
                    //    {
                    //        distance = trkpt.extensions.speed;
                    //    }
                    //    distanceMeters += distance;
                    //}

                   /* if (prev_t != null)
                    {
                        float distance = prev_t.extensions.speed + trkpt.extensions.speed;
                        distanceMeters += distance;
                    }*/

                    if (prev_t != null)
                    {
                        float distance = convert.VincentFormula(prev_t.lat, prev_t.lon, trkpt.lat, trkpt.lon);

                        if (prev_t.ele != trkpt.ele)
                        {
                            float elevation = Convert.ToSingle(Math.Abs(prev_t.ele - trkpt.ele));

                            distance = Convert.ToSingle(Math.Sqrt(Math.Pow(distance, 2) + Math.Pow(elevation, 2)));
                        }

                        distanceMeters += distance;
                    }
                    //distanceMeters += trkpt.extensions.speed * 2;
                    //distanceMeters += trkpt.extensions.speed;
                    //////////////////////////////////////////////////////////////////////////
                    trackpoint.DistanceMeters = distanceMeters;
                    trackpoint.HeartRateBpm.Value = trkpt.extensions.heartrate;
                    trackpoint.Position.LatitudeDegrees = trkpt.lat;
                    trackpoint.Position.LongitudeDegrees = trkpt.lon;
                    trackpoint.SensorState = "Absent";
                    trackpoint.Extensions.TPX.CadenceSensor = garmin.CadenceSensorType_t.Bike;
                    trackpoint.Extensions.TPX.Speed = trkpt.extensions.speed;
                    trackpoint.Time = trkpt.time;

                    prev_t = trkpt;
                    trkpEntry++;
                }

                lap.AverageHeartRateBpm.Value = convert.AverageHeartRateBpm(lap.Track);
                lap.DistanceMeters = gpx.trk.extensions.length; // meters
                //convert.DistanceMeters(lap.Track);
                lap.MaximumHeartRateBpm.Value = convert.MaxHeartRateBmp(lap.Track);
                lap.Cadence = convert.CadenceAverage(lap.Track);
                //lap.TotalTimeSeconds = gpx.trk.extensions.timelength;
                lap.Calories = 0;
                lap.MaximumSpeed = gpx.trk.extensions.maxspeed;
                lap.StartTime = trkseg.trkpt[0].time;

                TimeSpan diff = maxTime - minTime;

                lap.TotalTimeSeconds = Convert.ToSingle(diff.TotalSeconds);
                
                lap.DistanceMeters = lap.Track.Trackpoint[lap.Track.Trackpoint.Length - 1].DistanceMeters;

                //lap.DistanceMeters = gpx.trk.extensions.le
                
                lapEntry++;
            }

            float calories = Convert.ToSingle(gpx.trk.extensions.calories);    // total calories
            float caloriesXsec = calories / gpx.trk.extensions.timelength;      // calories x second

            // update calories
            foreach (garmin.Lap_t lap in activity.Lap)
            {
                lap.Calories = Convert.ToInt32(caloriesXsec * lap.TotalTimeSeconds);
                //lap.
            }
            activies.activity = activity;
            tcx.Activities = activies;
            
            return tcx;
        }


        public static garmin.TrainingCenterDatabase_t toGarmin(gpx.strava.gpx_t gpx)
        {
            garmin.TrainingCenterDatabase_t tcx = new garmin.TrainingCenterDatabase_t();

            garmin.Build_t build = new garmin.Build_t();

            build.Version = convert.buildVersion();

            garmin.Author_t author = new garmin.Author_t();

            author.LangID = "IT";       // IT by default
            author.Name = gpx.trk.extensions.profile;   // profile
            author.PartNumber = gpx.metadata.Time;

            tcx.Author = author;

            garmin.Creator_t creator = new garmin.Creator_t();

            creator.Name = author.Name;
            creator.ProductID = 500;
            creator.UnitId = 500;
            creator.Version = convert.buildVersion();

            // Activities
            garmin.Activities_t activies = new garmin.Activities_t();

            garmin.Activity_t activity = new garmin.Activity_t();

            activity.Sport = garmin.Sport_t.Biking;
            activity.Id = gpx.metadata.Time;
            activity.Creator = creator;

            activity.Lap = new garmin.Lap_t[gpx.trk.trkseg.Length];
            int lapEntry = 0;

            foreach (strava.trkseg_t trkseg in gpx.trk.trkseg)
            {
                activity.Lap[lapEntry] = new garmin.Lap_t();

                garmin.Lap_t lap = activity.Lap[lapEntry];

                lap.Intensity = "Active";
                lap.TriggerMethod = "Manual";

                // fill lap with trkseg!

                lap.Track = new garmin.Track_t();

                lap.Track.Trackpoint = new garmin.Trackpoint_t[trkseg.trkpt.Length];

                int trkpEntry = 0;

                float distanceMeters = 0.0F;

                DateTime minTime = convert.StringAsDateTime(trkseg.trkpt[0].time);
                DateTime maxTime = convert.StringAsDateTime(trkseg.trkpt[trkseg.trkpt.Length - 1].time);

                foreach (strava.trkpt_t trkpt in trkseg.trkpt)
                {   // process trackpoint
                    lap.Track.Trackpoint[trkpEntry] = new garmin.Trackpoint_t();
                    garmin.Trackpoint_t trackpoint = lap.Track.Trackpoint[trkpEntry];

                    trackpoint.AltitudeMeters = trkpt.ele;
                    trackpoint.Cadence = trkpt.extensions.TrackPointExtension.Cadence;
                    distanceMeters += trkpt.extensions.speed;
                    trackpoint.DistanceMeters = distanceMeters;
                    trackpoint.HeartRateBpm.Value = trkpt.extensions.TrackPointExtension.Heartrate;
                    trackpoint.Position.LatitudeDegrees = trkpt.lat;
                    trackpoint.Position.LongitudeDegrees = trkpt.lon;
                    trackpoint.SensorState = "Absent";
                    trackpoint.Extensions.TPX.CadenceSensor = garmin.CadenceSensorType_t.Bike;
                    trackpoint.Extensions.TPX.Speed = trkpt.extensions.speed;
                    trackpoint.Time = trkpt.time;

                    trkpEntry++;
                }

                lap.AverageHeartRateBpm.Value = convert.AverageHeartRateBpm(lap.Track);
                lap.DistanceMeters = gpx.trk.extensions.length; // meters
                //convert.DistanceMeters(lap.Track);
                lap.MaximumHeartRateBpm.Value = convert.MaxHeartRateBmp(lap.Track);
                lap.Cadence = gpx.trk.extensions.avgcadence;
                lap.TotalTimeSeconds = gpx.trk.extensions.timelength;
                lap.Calories = gpx.trk.extensions.calories;
                lap.MaximumSpeed = gpx.trk.extensions.maxspeed;
                lap.StartTime = trkseg.trkpt[0].time;

                TimeSpan diff = maxTime - minTime;

                lap.TotalTimeSeconds = Convert.ToSingle(diff.TotalSeconds);

                lap.DistanceMeters = lap.Track.Trackpoint[lap.Track.Trackpoint.Length - 1].DistanceMeters;

                //lap.DistanceMeters = gpx.trk.extensions.le

                lapEntry++;
            }

            activies.activity = activity;
            tcx.Activities = activies;

            return tcx;
        }



        public static strava.gpx_t toStrava(gpx.miogpx gpx)
        {
            strava.gpx_t st = new strava.gpx_t();

            st.creator = "StravaGPS";
            st.version = "1.1";

            st.metadata = new strava.metadata_t();

            st.metadata.Name = gpx.metadata.Name;
            st.metadata.Time = gpx.metadata.Time;

            st.trk = new strava.trk_t();

            st.trk.cmt = gpx.trk.cmt;
            st.trk.Description = gpx.trk.Description;
            st.trk.name = gpx.trk.name;
            st.trk.type = gpx.trk.type;

            // copy extensions value

            st.trk.extensions = new strava.trk_extensions_t();

            st.trk.extensions.avgcadence = gpx.trk.extensions.avgcadence;
            st.trk.extensions.avggrade = gpx.trk.extensions.avggrade;
            st.trk.extensions.avgspeed = gpx.trk.extensions.avgspeed;
            st.trk.extensions.calories = gpx.trk.extensions.calories;
            st.trk.extensions.length = gpx.trk.extensions.length;
            st.trk.extensions.maxacceleration = gpx.trk.extensions.maxacceleration;
            st.trk.extensions.maxaltitude = gpx.trk.extensions.maxaltitude;
            st.trk.extensions.maxcadence = gpx.trk.extensions.maxcadence;
            st.trk.extensions.maxheartrate = gpx.trk.extensions.maxheartrate;
            st.trk.extensions.maxlat = gpx.trk.extensions.maxlat;
            st.trk.extensions.maxlon = gpx.trk.extensions.maxlon;
            st.trk.extensions.maxspeed = gpx.trk.extensions.maxspeed;
            st.trk.extensions.minacceleration = gpx.trk.extensions.minacceleration;
            st.trk.extensions.minaltitude = gpx.trk.extensions.minaltitude;
            st.trk.extensions.minheartrate = gpx.trk.extensions.minheartrate;
            st.trk.extensions.minlat = gpx.trk.extensions.minlat;
            st.trk.extensions.minlon = gpx.trk.extensions.minlon;
            st.trk.extensions.profile = gpx.trk.extensions.profile;
            st.trk.extensions.time = gpx.trk.extensions.time;
            st.trk.extensions.timelength = gpx.trk.extensions.timelength;
            st.trk.extensions.totalascent = gpx.trk.extensions.totalascent;
            st.trk.extensions.totaldescent = gpx.trk.extensions.totaldescent;


            st.trk.trkseg = new strava.trkseg_t[gpx.trk.trkseg.Length];

            int trkseg_count = 0;
            foreach (trkseg_t trkseg in gpx.trk.trkseg)
            {   // process all trkseg

                strava.trkseg_t strava_trkseg = new strava.trkseg_t();

                st.trk.trkseg[trkseg_count++] = strava_trkseg;

                strava_trkseg.trkpt = new strava.trkpt_t[trkseg.trkpt.Length];

                int trkpt_count = 0;

                foreach (trkpt_t trkpt in trkseg.trkpt)
                {
                    strava.trkpt_t st_trkpt = new strava.trkpt_t();

                    strava_trkseg.trkpt[trkpt_count++] = st_trkpt;

                    st_trkpt.ele = trkpt.ele;
                    st_trkpt.lat = trkpt.lat;
                    st_trkpt.lon = trkpt.lon;
                    st_trkpt.time = trkpt.time;

                    st_trkpt.extensions.speed = trkpt.extensions.speed;
                    st_trkpt.extensions.TrackPointExtension.Cadence = trkpt.extensions.cadence;
                    st_trkpt.extensions.TrackPointExtension.Heartrate = trkpt.extensions.heartrate;
                }

            }
            return st;
        }

        public static gpx.miogpx removeZero(gpx.miogpx gpxIn)
        {
            gpx.miogpx gpxOut = new miogpx();

            gpxOut.metadata = new gpx.metadata_t();
            
            gpxOut.metadata.Name = gpxIn.metadata.Name;
            gpxOut.metadata.Time = gpxIn.metadata.Time;

            gpxOut.trk = new gpx.trk_t();

            gpxOut.trk.cmt = gpxIn.trk.cmt;
            gpxOut.trk.Description = gpxIn.trk.Description;
            gpxOut.trk.name = gpxIn.trk.name;
            gpxOut.trk.type = gpxIn.trk.type;

            // copy extensions value

            gpxOut.trk.extensions = new gpx.trk_extensions_t();

            gpxOut.trk.extensions.avgcadence = gpxIn.trk.extensions.avgcadence;
            gpxOut.trk.extensions.avggrade = gpxIn.trk.extensions.avggrade;
            gpxOut.trk.extensions.avgspeed = gpxIn.trk.extensions.avgspeed;
            gpxOut.trk.extensions.avgheartrate = gpxIn.trk.extensions.avgheartrate;
            gpxOut.trk.extensions.calories = gpxIn.trk.extensions.calories;
            gpxOut.trk.extensions.length = gpxIn.trk.extensions.length;
            gpxOut.trk.extensions.maxacceleration = gpxIn.trk.extensions.maxacceleration;
            gpxOut.trk.extensions.maxaltitude = gpxIn.trk.extensions.maxaltitude;
            gpxOut.trk.extensions.maxcadence = gpxIn.trk.extensions.maxcadence;
            gpxOut.trk.extensions.maxheartrate = gpxIn.trk.extensions.maxheartrate;
            gpxOut.trk.extensions.maxlat = gpxIn.trk.extensions.maxlat;
            gpxOut.trk.extensions.maxlon = gpxIn.trk.extensions.maxlon;
            gpxOut.trk.extensions.maxspeed = gpxIn.trk.extensions.maxspeed;
            gpxOut.trk.extensions.minacceleration = gpxIn.trk.extensions.minacceleration;
            gpxOut.trk.extensions.minaltitude = gpxIn.trk.extensions.minaltitude;
            gpxOut.trk.extensions.minheartrate = gpxIn.trk.extensions.minheartrate;
            gpxOut.trk.extensions.minlat = gpxIn.trk.extensions.minlat;
            gpxOut.trk.extensions.minlon = gpxIn.trk.extensions.minlon;
            gpxOut.trk.extensions.profile = gpxIn.trk.extensions.profile;
            gpxOut.trk.extensions.time = gpxIn.trk.extensions.time;
            gpxOut.trk.extensions.timelength = gpxIn.trk.extensions.timelength;
            gpxOut.trk.extensions.totalascent = gpxIn.trk.extensions.totalascent;
            gpxOut.trk.extensions.totaldescent = gpxIn.trk.extensions.totaldescent;
            
            gpxOut.trk.trkseg = new gpx.trkseg_t[gpxIn.trk.trkseg.Length];


            int trkseg_count = 0;

            // create timeline of track
            List<string> timeline = new List<string>();

            foreach (trkseg_t trkseg in gpxIn.trk.trkseg)
            {   // process all trkseg

                if (trkseg.trkpt == null)
                {   // this item must be removed! INVALID or EMPTY
                    gpx.trkseg_t [] temp = new gpx.trkseg_t[gpxOut.trk.trkseg.Length - 1]; // remove 1 item!
                    
                    // copy previous objects
                    for (int i = 0; i < gpxOut.trk.trkseg.Length - 1; i++)
                        temp[i] = gpxOut.trk.trkseg[i];

                    gpxOut.trk.trkseg = temp;   // replace!
                    continue;
                }

                gpx.trkseg_t strava_trkseg = new gpx.trkseg_t();

                gpxOut.trk.trkseg[trkseg_count++] = strava_trkseg;

                // count numbers of element with speed != 0 in this track!
                int trkpt_nonzero = 0;

                foreach (trkpt_t trkpt in trkseg.trkpt)
                {   // create timeline!
                    timeline.Add(trkpt.time);   // add in timeline
                    if (trkpt.extensions.speed != 0)
                        trkpt_nonzero++;
                }

                strava_trkseg.trkpt = new gpx.trkpt_t[trkpt_nonzero];

                System.Diagnostics.Debug.WriteLine(
                    string.Format("Track {0} - Points {1} / {2}",
                    trkseg.trkpt[0].time,
                    trkseg.trkpt.Length, trkpt_nonzero));

                int trkpt_count = 0;

                int timeline_cursor = 0;

                foreach (trkpt_t trkpt in trkseg.trkpt)
                {
                    if (trkpt.extensions.speed != 0)
                    {   // 
                        gpx.trkpt_t st_trkpt = new gpx.trkpt_t();

                        strava_trkseg.trkpt[trkpt_count++] = st_trkpt;

                        st_trkpt.ele = trkpt.ele;
                        st_trkpt.lat = trkpt.lat;
                        st_trkpt.lon = trkpt.lon;
                        st_trkpt.time = timeline[timeline_cursor];

                        st_trkpt.extensions = new gpx.trk_trkseg_trkpk_extensions_t();
                        st_trkpt.extensions.speed = trkpt.extensions.speed;
                        st_trkpt.extensions.acceleration = trkpt.extensions.acceleration;
                        st_trkpt.extensions.cadence = trkpt.extensions.cadence;
                        st_trkpt.extensions.course = trkpt.extensions.course;
                        st_trkpt.extensions.heartrate = trkpt.extensions.heartrate;
                        
                        timeline_cursor++;  // move cursor on next
                    }
                    else
                    {   // pause detected!
                        
                    }
                }

                timeline.RemoveRange(0, timeline_cursor);

            }
            return gpxOut;
        }

    }
}
