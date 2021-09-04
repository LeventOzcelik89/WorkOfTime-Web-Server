using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Infoline.Framework.Helper
{

    public class CryptographyHelper
    {
        private const string publicKey = "TsLb15R99c.-85rM"; // 16 bitx
        private const string vectorKey = "nf!Mks+0cc+205RR";

        private readonly ICryptoTransform _decryptor;
        private readonly ICryptoTransform _encryptor;
        private static readonly byte[] IV = Encoding.UTF8.GetBytes(vectorKey);
        private readonly byte[] _password;
        private readonly RijndaelManaged _cipher;
        private ICryptoTransform Decryptor { get { return _decryptor; } }
        private ICryptoTransform Encryptor { get { return _encryptor; } }

        public CryptographyHelper(string password = publicKey)
        {
            _password = (Encoding.ASCII.GetBytes(password));
            _cipher = new RijndaelManaged();
            _decryptor = _cipher.CreateDecryptor(_password, (IV));
            _encryptor = _cipher.CreateEncryptor(_password, (IV));
        }

        public string Decrypt(string text)
        {
            try
            {
                text = text.Trim();
                var isBase64String = (text.Length % 4 == 0) && Regex.IsMatch(text, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

                if (isBase64String)
                {
                    byte[] input = Convert.FromBase64String(text);

                    var newClearData = Decryptor.TransformFinalBlock(input, 0, input.Length);
                    return Encoding.UTF8.GetString(newClearData);
                }
                else
                    return text;
            }
            catch
            {
                return null;
            }
        }

        public string Encrypt(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(Encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        
    }

    public class CryptographyHelperV2
    {
        //  private const string publicKey = "TsLb15R99c.-85rM"; // 16
        private const string publicKey = "TsLb185RR99c.-85rM"; //18
        private const string vectorKey = "nf!Mks+0cc+205RR";

        private readonly ICryptoTransform _decryptor;
        private readonly ICryptoTransform _encryptor;
        private static readonly byte[] IV = Encoding.UTF8.GetBytes(vectorKey);
        private readonly byte[] _password;
        private readonly RijndaelManaged _cipher;
        private ICryptoTransform Decryptor { get { return _decryptor; } }
        private ICryptoTransform Encryptor { get { return _encryptor; } }

        public CryptographyHelperV2(string _publicKey = publicKey)
        {
            var md5 = new MD5CryptoServiceProvider();
            _password = md5.ComputeHash(Encoding.ASCII.GetBytes(_publicKey));
            _cipher = new RijndaelManaged();
            _decryptor = _cipher.CreateDecryptor(_password, md5.ComputeHash(IV));
            _encryptor = _cipher.CreateEncryptor(_password, md5.ComputeHash(IV));
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);

            var newClearData = Decryptor.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(newClearData);
        }

        public string Encrypt(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(Encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }
    
}
