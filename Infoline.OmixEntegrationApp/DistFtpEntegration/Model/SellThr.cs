namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Model
{
    public class SellThr:SellIn
    {
        public string InvoiceNumber { get; set; }
        public string CustomerOperatorCode { get; set; }
        public string CustomerGENPACode { get; set; }
        public string CustomerName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string TaxNumber { get; set; }
    }
}
