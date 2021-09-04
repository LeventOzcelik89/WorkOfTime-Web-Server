
using System;
using System.Net;


namespace Infoline.GIS
{
    public class UTM : MapCoordinate
    {
        public double Zone { get; set; }

        public Hemisphere Hemi { get; set; }
        public WGS84 ToWGS84()
        {
            return GeoUTMConverter.ToWGS84(this);
        }
        public override string ToString()
        {
            return string.Format("H:{0} Z:{1} X:{2} Y:{3}", Hemi, Zone, X, Y);
        }
    }
    public enum Hemisphere
    {
        Northern = 0,
        Southern = 1
    }

    class GeoUTMConverter
    {


        static double pi = Math.PI;// 3.14159265358979;
        static double sm_a = 6378137.0;
        static double sm_b = 6356752.314;
        static double UTMScaleFactor = 0.9996;



        public static UTM ToUTM(WGS84 coord)
        {
            var utm = new UTM();
            utm.Zone = Math.Floor((coord.Longitude + 180.0) / 6) + 1;
            //if(coord.Latitude >= 56.0 && coord.Latitude < 64.0 && 
            //    coord.Longitude >= 3.0 && coord.Longitude < 12.0)
            //{
            //    utm.Zone = 32;
            //}
            //if(coord.Latitude >= 72 && coord.Latitude < 84)
            //{
            //     if ( coord.Longitude >= 0.0  && coord.Longitude <  9.0 ) {utm.Zone = 31;}
            //     if ( coord.Longitude >= 9.0  && coord.Longitude < 21.0 ) {utm.Zone = 33;}
            //     if ( coord.Longitude >= 21.0 && coord.Longitude < 33.0 ) {utm.Zone = 35;}
            //     if ( coord.Longitude >= 33.0 && coord.Longitude < 42.0 ) {utm.Zone = 37;}
            //}
            GeoUTMConverterXY(DegToRad(coord.Latitude), DegToRad(coord.Longitude), utm);
            return utm;
        }
        public static UTM ToUTM(WGS84 coord, double zone)
        {
            var utm = new UTM();
            utm.Zone = zone;
            GeoUTMConverterXY(DegToRad(coord.Latitude), DegToRad(coord.Longitude), utm);
            return utm;
        }

        public static WGS84 ToWGS84(UTM utm)
        {
            double x = utm.X, y = utm.Y;
            var coord = new WGS84();

            double cmeridian;

            x -= 500000.0;
            x /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            if (utm.Hemi == Hemisphere.Southern)
                y -= 10000000.0;

            y /= UTMScaleFactor;

            cmeridian = UTMCentralMeridian(utm.Zone);
            double[] latlon = MapXYToLatLon(x, y, cmeridian);

            coord.Latitude = RadToDeg(latlon[0]);
            coord.Longitude = RadToDeg(latlon[1]);
            return coord;
        }

        static double DegToRad(double degrees)
        {
            return (degrees / 180.0 * pi);
        }

        static double RadToDeg(double radians)
        {
            return (radians / pi * 180.0);
        }

        //static double MetersToFeet(double meters)
        //{
        //    return (meters * 3.28084);
        //}

        //static double FeetToMeters(double feet)
        //{
        //    return (feet / 3.28084);
        //}

        static double ArcLengthOfMeridian(double phi)
        {
            return alpha*
                   (phi + (beta * Math.Sin(2.0 * phi))
                    + (gamma * Math.Sin(4.0 * phi))
                    + (delta * Math.Sin(6.0 * phi))
                    + (epsilon * Math.Sin(8.0 * phi)));
        }

        static double UTMCentralMeridian(double zone)
        {
            return (DegToRad(-183.0 + (zone * 6.0)));
        }
        static double n, alpha_, beta_, gamma_, delta_, epsilon_, ep2;
        static double alpha, beta, gamma, delta, epsilon;
        static GeoUTMConverter()
        {
            /* Precalculate n (Eq. 10.18) */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            alpha_ = ((sm_a + sm_b) / 2.0)
              * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0)
             + (269.0 * Math.Pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            gamma_ = (21.0 * Math.Pow(n, 2.0) / 16.0)
                + (-55.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0)
                + (-417.0 * Math.Pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);
            /* Precalculate ep2 */

            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            alpha = ((sm_a + sm_b) / 2.0)
            * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));

            /* Precalculate beta */
            beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) / 16.0)
               + (-3.0 * Math.Pow(n, 5.0) / 32.0);

            /* Precalculate gamma */
            gamma = (15.0 * Math.Pow(n, 2.0) / 16.0)
                + (-15.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta */
            delta = (-35.0 * Math.Pow(n, 3.0) / 48.0)
                + (105.0 * Math.Pow(n, 5.0) / 256.0);

            /* Precalculate epsilon */
            epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);

        }
        static double FootpointLatitude(double y)
        {
            /* Precalculate y_ (Eq. 10.23) */
            var y_ = y / alpha_;

            /* Now calculate the sum of the series (Eq. 10.21) */
            return y_ + (beta_ * Math.Sin(2.0 * y_))
                + (gamma_ * Math.Sin(4.0 * y_))
                + (delta_ * Math.Sin(6.0 * y_))
                + (epsilon_ * Math.Sin(8.0 * y_));

        }

        static double[] MapLatLonToXY(double phi, double lambda, double lambda0)
        {
            double[] xy = new double[2];

            double N, nu2, t, t2, l;

            double l3coef, l4coef, l5coef, l6coef, l7coef, l8coef;

            double tmp;
            var cosphi = Math.Cos(phi);

            /* Precalculate nu2 */

            nu2 = ep2 * cosphi *cosphi;

            /* Precalculate N */

            N = (sm_a * sm_a) / (sm_b * Math.Sqrt(1 + nu2));



            /* Precalculate t */

            t = Math.Tan(phi);

            t2 = t * t;
            var t3 = t2 * t;
            var t4 = t3 * t;
            tmp = t3 - Math.Pow(t, 6.0);

            /* Precalculate l */
            l = lambda - lambda0;

            /* Precalculate coefficients for l**n in the equations below
               so a normal human being can read the expressions for easting
               and northing
               -- l**1 and l**2 have coefficients of 1.0 */

            l3coef = 1.0 - t2 + nu2;

            l4coef = 5.0 - t2 + 9 * nu2 + 4.0 * (nu2 * nu2);

            l5coef = 5.0 - 18.0 * t2 + t4 + 14.0 * nu2
                - 58.0 * t2 * nu2;

            l6coef = 61.0 - 58.0 * t2 + t4 + 270.0 * nu2
                - 330.0 * t2 * nu2;

            l7coef = 61.0 - 479.0 * t2 + 179.0 * t4 - t3;
            l8coef = 1385.0 - 3111.0 * t2 + 543.0 *t4 - t3;

            /* Calculate easting (x) */
            xy[0] = N * cosphi * l
                + (N / 6.0 * Math.Pow(cosphi, 3.0) * l3coef * Math.Pow(l, 3.0))
                + (N / 120.0 * Math.Pow(cosphi, 5.0) * l5coef * Math.Pow(l, 5.0))
                + (N / 5040.0 * Math.Pow(cosphi, 7.0) * l7coef * Math.Pow(l, 7.0));

            /* Calculate northing (y) */
            xy[1] = ArcLengthOfMeridian(phi)
                + (t / 2.0 * N * Math.Pow(cosphi, 2.0) * Math.Pow(l, 2.0))
                + (t / 24.0 * N * Math.Pow(cosphi, 4.0) * l4coef * Math.Pow(l, 4.0))
                + (t / 720.0 * N * Math.Pow(cosphi, 6.0) * l6coef * Math.Pow(l, 6.0))
                + (t / 40320.0 * N * Math.Pow(cosphi, 8.0) * l8coef * Math.Pow(l, 8.0));


            return xy;
        }

        static double[] MapXYToLatLon(double x, double y, double lambda0)
        {
            double[] latlon = new double[2];

            double phif, Nf, Nfpow, nuf2, tf, tf2, tf4, cf;
            double x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            double x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(y);



            /* Precalculate cos (phif) */
            cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            Nf = (sm_a * sm_a) / (sm_b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
               below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
               -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2
                - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2
                + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            latlon[0] = phif + x2frac * x2poly * (x * x)
                + x4frac * x4poly * Math.Pow(x, 4.0)
                + x6frac * x6poly * Math.Pow(x, 6.0)
                + x8frac * x8poly * Math.Pow(x, 8.0);

            /* Calculate longitude */
            latlon[1] = lambda0 + x1frac * x
                + x3frac * x3poly * Math.Pow(x, 3.0)
                + x5frac * x5poly * Math.Pow(x, 5.0)
                + x7frac * x7poly * Math.Pow(x, 7.0);

            return latlon;
        }

        static void GeoUTMConverterXY(double lat, double lon, UTM utm)
        {
            double[] xy = MapLatLonToXY(lat, lon, UTMCentralMeridian(utm.Zone));

            xy[0] = xy[0] * UTMScaleFactor + 500000.0;
            xy[1] = xy[1] * UTMScaleFactor;
            if (xy[1] < 0.0)
                xy[1] = xy[1] + 10000000.0;

            utm.X = (int)(xy[0]);
            utm.Y = (int)(xy[1]);

            //utm.X = FeetToMeters(utm.X);
            //utm.Y = FeetToMeters(utm.Y);
        }




    }
}