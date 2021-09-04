using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infoline.DownloadFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string baseUrl = "http://localhost/";
        short taskOperationStatus = 40;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDownloadFiles_Click(object sender, EventArgs e)
        {
            var db = new InfolineDatabase();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == 1997).FirstOrDefault();
            db = tenant.GetDatabase();
        }
    }
}
