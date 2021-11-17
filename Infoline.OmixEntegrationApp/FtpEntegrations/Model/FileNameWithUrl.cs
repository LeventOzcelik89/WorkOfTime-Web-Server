using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegrations.Model
{
    public class FileNameWithUrl
    {
        public string FileName { get; set; }
        public string DirectoryFileName { get; set; }
        public DateTime FileCreatedDate { get; set; }

    }
}
