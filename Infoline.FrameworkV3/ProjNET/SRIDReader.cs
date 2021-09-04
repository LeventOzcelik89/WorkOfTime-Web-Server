using System.Collections.Generic;
using System.IO;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems;
using System.Text;

namespace ProjNet
{
    public class SRIDReader
    {
        private const string filename = @"SRID.csv";

        public struct WKTstring {
            /// <summary>
            /// Well-known ID
            /// </summary>
            public int WKID;
            /// <summary>
            /// Well-known Text
            /// </summary>
            public string WKT;
        }

        /// <summary>
        /// Enumerates all SRID's in the SRID.csv file.
        /// </summary>
        /// <returns>Enumerator</returns>
        public static IEnumerable<WKTstring> GetSRIDs()
        {
            string path = System.AppDomain.CurrentDomain.GetData("DataDirectory") != null ? 
                                System.AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\" : "";

            var filePath = Path.Combine(path, filename);
            if (File.Exists(filePath))
            {
                using (StreamReader sr = File.OpenText(path + filename))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        int split = line.IndexOf(';');
                        if (split > -1)
                        {
                            WKTstring wkt = new WKTstring();
                            wkt.WKID = int.Parse(line.Substring(0, split));
                            wkt.WKT = line.Substring(split + 1);
                            yield return wkt;
                        }
                    }
                    sr.Close();
                }
            }
            else
            {
                using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SRID)))
                {
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            int split = line.IndexOf(';');
                            if (split > -1)
                            {
                                WKTstring wkt = new WKTstring();
                                wkt.WKID = int.Parse(line.Substring(0, split));
                                wkt.WKT = line.Substring(split + 1);
                                yield return wkt;
                            }
                        }
                        sr.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Gets a coordinate system from the SRID.csv file
        /// </summary>
        /// <param name="id">EPSG ID</param>
        /// <returns>Coordinate system, or null if SRID was not found.</returns>
        public static ICoordinateSystem GetCSbyID(int id)
        {
   			CoordinateSystemFactory fac = new CoordinateSystemFactory();
            foreach (WKTstring wkt in GetSRIDs())
            {
                if (wkt.WKID == id)
                {
                    return CoordinateSystemWktReader.Parse(wkt.WKT) as ICoordinateSystem;
                }
            }
            return null;
        }
    }
}
