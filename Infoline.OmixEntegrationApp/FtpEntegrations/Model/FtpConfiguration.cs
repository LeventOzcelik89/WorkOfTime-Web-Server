﻿namespace Infoline.OmixEntegrationApp.FtpEntegrations.Model
{
    public class FtpConfiguration
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool SearchAllDirectory { get; set; }
        public string Directory { get; set; }
        public string FileExtension { get; set; }
    }
}
