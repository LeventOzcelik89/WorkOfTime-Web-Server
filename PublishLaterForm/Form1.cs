using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PublishLaterForm
{
    public partial class Form1 : Form
    {
        public List<TEN_Tenant> executes { get; set; } = TenantConfig.GetTenants();
        public string connection { get; set; } = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(ConfigurationManager.AppSettings["RemoteConnection"]);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var execute in this.executes)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        var uri = execute.WebDomain.Split(',').FirstOrDefault() + "/Account/Execute";
                        string JsonData = client.DownloadString(new Uri(uri));
                        textBox1.AppendText("\r\n" + execute.DBCatalog + " Execute Başarılı");
                    }
                }
                catch (Exception ex)
                {
                    textBox1.AppendText("\r\n" + execute.DBCatalog + " Execute Başarısız" + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var refreshView = "";
            using (var _db = new InfolineDatabase(this.connection.Replace("IntranetManagement", "WorkOfTime"), DatabaseType.Mssql))
            {
                var views = _db.ExecuteReader<sysobjects>("select name from sys.objects  AS so WHERE so.type = 'V'").ToList();
                refreshView = string.Join(" ", views.Select(x => "exec sp_refreshview '" + x.name + "';"));
            }
            foreach (var execute in this.executes)
            {
                try
                {
                    using (var _db = new InfolineDatabase(this.connection.Replace("IntranetManagement", execute.DBCatalog), DatabaseType.Mssql))
                    {
                        var res = _db.ExecuteNonQuery(refreshView);

                        textBox1.AppendText("\r\n" + (res.result ? execute.DBCatalog + " Refresh View Başarılı" : execute.DBCatalog + " Refresh View Başarısız"));

                    }
                }

                catch (System.Exception)
                {
                    textBox1.AppendText("\r\n" + " Refresh View Başarısız");
                }
            }
        }

        public void button3_Click(object sender, EventArgs e)
        {
            var refreshView = "";

            using (var _db = new InfolineDatabase(this.connection.Replace("IntranetManagement", "WorkOfTime"), DatabaseType.Mssql))
            {
                refreshView = richTextBox1.Text;
            }
            foreach (var execute in executes)
            {
                try
                {
                    using (var _db = new InfolineDatabase(this.connection.Replace("IntranetManagement", execute.DBCatalog), DatabaseType.Mssql))
                    {
                        refreshView = refreshView.Replace("GO", "");
                        var res = _db.ExecuteNonQuery(refreshView);
                        textBox1.AppendText("\r\n" + (res.result ? execute.DBCatalog + " Sorgu Başarılı" : execute.DBCatalog + " Sorgu Başarısız"));
                    }
                }
                catch (System.Exception ex)
                {
                    textBox1.AppendText("\r\n" + execute.DBCatalog + " Sorgu Başarısız." + ex.Message);
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.button3_Click(new object(), new EventArgs());
            this.button2_Click(new object(), new EventArgs());
            this.button1_Click(new object(), new EventArgs());
        }
    }

    public class sysobjects
    {
        public string name { get; set; }
        public string object_id { get; set; }
        public string principal_id { get; set; }
        public string schema_id { get; set; }
        public string parent_object_id { get; set; }
        public string type { get; set; }
        public string type_desc { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string is_ms_shipped { get; set; }
        public string is_published { get; set; }
        public string is_schema_published { get; set; }

    }
}
