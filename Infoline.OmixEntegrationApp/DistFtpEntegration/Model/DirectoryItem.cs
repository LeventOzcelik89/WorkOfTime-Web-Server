using System;
using System.Collections.Generic;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Model
{
    public class DirectoryItem
    {
        public string BaseUri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AbsolutePath { get { return string.Format("{0}/{1}", BaseUri, Name); } }
        public DateTime DateCreated { get; set; }
        public bool IsDirectory { get; set; }
        public string Name { get; set; }
        public List<DirectoryItem> Items { get; set; }
    }
}
