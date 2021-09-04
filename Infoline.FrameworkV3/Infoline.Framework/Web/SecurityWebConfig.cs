using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace Infoline.Web
{
    public class SecurityWebConfig : IConfigurationSectionHandler
    {
        public SecurityWebConfig()
        {
        }

        static SecurityWebConfig _config = null;
        public static SecurityWebConfig GetConfig
        {
            get
            {
                if (_config == null)
                    _config = System.Configuration.ConfigurationManager.GetSection("WebSecurity") as SecurityWebConfig;
                if (_config == null) throw new Exception("Missing WebSecurity section in web.config.");
                return _config;
            }
        }

        #region IConfigurationSectionHandler Members
        NameValueCollection _settings = new NameValueCollection();
        public NameValueCollection Settings
        {
            get
            {
                return _settings;
            }
        }
        const string LoginPageKey = "LoginPage";
        const string CookieNameKey = "CookieName";
        const string PopupPageURLKey = "PopupPageURL";
        private const string TicketLifeKey = "TicketLife";
        public object Create(object parent, object configContext, XmlNode section)
        {
            XmlElement element = section as XmlElement;

            foreach (XmlAttribute att in section.Attributes)
            {
                _settings[att.Name] = att.Value;
            }
            if (element.HasAttribute(LoginPageKey)) _loginaPage = element.GetAttribute(LoginPageKey);
            if (element.HasAttribute(PopupPageURLKey)) _popURL = element.GetAttribute(PopupPageURLKey);
            if (element.HasAttribute(CookieNameKey)) _cookieName = element.GetAttribute(CookieNameKey);
            if (element.HasAttribute(TicketLifeKey)) _ticketLife = Convert.ToInt32(element.GetAttribute(TicketLifeKey));
            _publicpaths.Add("/WebResource.axd");
            foreach (XmlElement pathelement in section.ChildNodes)
            {
                if (pathelement.Name == "Securepath")
                {
                    if (pathelement.InnerText.Length > 0)
                        _securePaths.Add(pathelement.InnerText);
                }
                else if (pathelement.Name == "Publicpath")
                {
                    if (pathelement.InnerText.Length > 0)
                        _publicpaths.Add(pathelement.InnerText);
                }
            }
            return this;
        }
        #endregion

        internal System.Collections.Generic.List<string> _securePaths = new System.Collections.Generic.List<string>();
        internal System.Collections.Generic.List<string> _publicpaths = new System.Collections.Generic.List<string>();
        public bool IsSecurePath(string path)
        {
            int pp = 0, sp = 0;
            foreach (string p in _publicpaths)
            {

                if (p != null && path.StartsWith(p, StringComparison.InvariantCultureIgnoreCase) && pp < p.Length)
                    pp = p.Length;
            }

            foreach (string p in _securePaths)
            {
                if (p != null && path.StartsWith(p, StringComparison.InvariantCultureIgnoreCase) && sp < p.Length)
                    sp = p.Length;
            }
            return (sp > 0 && pp == 0) || (pp > 0 && sp > pp);
        }
        public void AddPublicPath(string path)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;
            if (!path.EndsWith("/"))
                path = path + "/";
            if (!_publicpaths.Contains(path))
                _publicpaths.Add(path);
        }
        public void RemovePublicPath(string path)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;
            if (!path.EndsWith("/"))
                path = path + "/";
            if (!_publicpaths.Contains(path))
                _publicpaths.Remove(path);
        }

        public List<string> GetPublicPath()
        {
            return _publicpaths;
        }
        #region Property
        string _popURL = "pop.aspx";
        public string PopupURL
        {
            get
            {
                return _popURL;
            }
        }
        string _cookieName = "_securityCookie";
        public string CookieName
        {
            get
            {
                return _cookieName;
            }
        }
        string _loginaPage = "login.aspx";
        public string LoginPage
        {
            get
            {
                return _loginaPage;
            }
        }

        private int _ticketLife = 60;
        public int TicketLife
        {
            get
            {
                return _ticketLife;
            }
        }
        #endregion
    }
}