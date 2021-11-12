using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Entities
{
    public class FtpObjectsWithFileName<T>
    {
        public DirectoryItem DirectoryItem { get; set; }
        public IEnumerable<T> Files { get; set; }


    }
}
