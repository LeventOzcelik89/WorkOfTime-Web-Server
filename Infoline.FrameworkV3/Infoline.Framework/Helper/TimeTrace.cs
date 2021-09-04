using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infoline.Framework.Helper
{
    public class TimeTrace : IDisposable
    {
        DateTime start = DateTime.Now;
        string zone;
        public TimeTrace(string msg)
        {
            zone = msg;

        }

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("{0} {1}", zone, DateTime.Now.Subtract(start).TotalMilliseconds);
        }
    }
}
