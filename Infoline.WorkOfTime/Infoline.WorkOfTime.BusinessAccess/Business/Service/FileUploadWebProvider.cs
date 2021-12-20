using System;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess.Business.Service
{
    internal class FileUploadWebProvider
    {
        private HttpRequestBase request;
        private Guid id;

        public FileUploadWebProvider(HttpRequestBase request, Guid id)
        {
            this.request = request;
            this.id = id;
        }
    }
}