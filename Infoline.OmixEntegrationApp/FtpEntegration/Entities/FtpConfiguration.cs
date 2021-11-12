namespace Infoline.OmixEntegrationApp.FtpEntegration.Entities
{
    public class FtpConfiguration
    {

        
        /// <summary>
        /// Ftp'nin Urli
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Giriş yapmak için gerekli kullanıcı adı 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Giriş yapmak için gerekli şifre
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Tüm dizinler aransın mı ? 
        /// </summary>
        public bool SearchAllDirectory { get; set; }
        /// <summary>
        /// İçeriğinin aranması gereken dizin
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// Dosya uzantısı
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// Dosyanın tarihinin hesaplanması için gerekli olan alan. Örnek: yyyyMMddss 
        /// </summary>
        public string FileDateTimeFormat { get; set; }

    }
}
