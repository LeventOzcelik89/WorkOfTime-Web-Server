using Infoline.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Authentication
{
    public class Token
    {
        public Guid AboneId { get; private set; }
        public Guid TabletId { get; private set; }
        public string AppVersion { get; private set; }
        public DateTime DateCreated { get; private set; }
        public Nullable<Guid> UserId { get; private set; }
        public string Coordinate { get; private set; }

        public Token(Guid aboneId, Guid tabletId, string appVersion, DateTime dateCreated, Nullable<Guid> userId, string coordinate)
        {
            this.AboneId = aboneId;
            this.TabletId = tabletId;
            this.AppVersion = appVersion;
            this.DateCreated = dateCreated;
            this.UserId = userId;
            this.Coordinate = coordinate;
        }

        public string Encrypt()
        {
            return new CryptographyHelper().Encrypt(this.ToString());
        }

        public override string ToString()
        {
            return String.Format(SystemConst.TOKEN__ABONE_ID + "={0};"
                + SystemConst.TOKEN__TABLET_ID + "={1};"
                + SystemConst.TOKEN__APP_VERSION + "={2};"
                + SystemConst.TOKEN__DATECREATED + "={3};"
                + SystemConst.TOKEN__USER_ID + "={4};"
                + SystemConst.TOKEN__COORDINATE + "={5};", this.AboneId, this.TabletId, this.AppVersion, this.DateCreated, this.UserId, this.Coordinate);
        }

        public static Token Decrypt(string encryptedToken)
        {
            var decrypted = new CryptographyHelper().Decrypt(encryptedToken);

            return (Token)Newtonsoft.Json.JsonConvert.DeserializeObject(decrypted, typeof(Token));
        }
    }
}
