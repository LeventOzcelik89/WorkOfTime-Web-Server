using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMSYS_FilesModel : VWSYS_Files
    {
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
    public class UploadFileObject
    {
        public Guid id { get; set; }
        public string tip { get; set; }
        public string file64 { get; set; }
        public string fileName { get; set; }
        public string fileGroup { get; set; }
    }
}
