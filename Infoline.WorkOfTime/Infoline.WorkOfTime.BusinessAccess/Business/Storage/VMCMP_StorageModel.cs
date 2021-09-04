using GeoAPI.Geometries;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Business.Storage
{
    public class VMCMP_StorageModel : VWCMP_Storage
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }


        public VMCMP_StorageModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var storage = db.GetVWCMP_StorageById(this.id);

            if (storage == null)
            {
                if (this.pid.HasValue)
                {

                }
                else
                {

                }


                this.code = this.code ?? BusinessExtensions.B_GetIdCode();

            }

            if (storage != null)
            {
                this.B_EntityDataCopyForMaterial(storage, true);

            }

            return this;
        }
    }

    public class StorageRequest
    {
        public IGeometry location { get; set; }
        public string keyword { get; set; }
        public int? skip { get; set; }
        public int? take { get; set; }
    }

    public class StorageReponse
    {

        public Guid id { get; set; }
        public double? distance { get; set; }
        public string fullName { get; set; }
        public IGeometry location { get; set; }
        public string adress { get; set; }
    }
}
