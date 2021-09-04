using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Infoline.Framework.Helper
{

    public class WebDownload : WebClient
    {
        public int Timeout { get; set; }
        CookieContainer cookiecontainer;
        public string ticid { get; set; }
        string username { get; set; }
        string pass { get; set; }
        string LOGINDOMAIN { get; set; }
        public string ticketsid { get; set; }
        public WebDownload(CookieContainer cookiecontainer, string _logindomain, string _username, string _password)
        {
            this.cookiecontainer = cookiecontainer;
            this.Timeout = 300000;
            this.username = _username;
            this.pass = _password;
            this.LOGINDOMAIN = _logindomain;
            login();
        }
        void login()
        {
            var nvc = new NameValueCollection()
                {
                   {"username", this.username},
                   {"password", this.pass},

                };
            ticketsid = Encoding.UTF8.GetString(this.UploadValues(LOGINDOMAIN, nvc));

        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address) as HttpWebRequest;
            result.Timeout = this.Timeout;
            result.CookieContainer = cookiecontainer;
            result.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return result;
        }
        public string GetData(string url)
        {
            var text = Encoding.UTF8.GetString(this.DownloadData(url));
            if (text.IndexOf("InvalidUser") > 0)
            {
                login();
                text = Encoding.UTF8.GetString(this.DownloadData(url));
            }
            return text;
        }
        public string PostData(string url, NameValueCollection nvc)
        {
            return Encoding.UTF8.GetString(this.UploadValues(url, nvc));
        }
        public string PostString(string url, string str)
        {
            var text = this.UploadString(url, str);
            if (text.IndexOf("InvalidUser") > 0)
            {
                login();
                text = this.UploadString(url, str);
            }
            if (text.IndexOf("InvalidUser") > 0)
                return null;

            return text;
        }
        public byte[] Data(string link)
        {
            login();
            return this.DownloadData(link);


        }

    }
}
