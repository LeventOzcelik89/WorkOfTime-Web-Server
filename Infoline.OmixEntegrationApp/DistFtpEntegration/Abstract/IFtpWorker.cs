﻿using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract
{
    public interface IFtpWorker
    {

        IEnumerable<SellIn> GetToDayFile();
    }
}