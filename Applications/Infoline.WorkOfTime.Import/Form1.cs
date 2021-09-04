using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infoline.WorkOfTime.Import
{
    public partial class Form1 : Form
    {

        public class Columns
        {
            //TableColumn


            public bool KullanıcıKaydi { get; set; }

            public bool FirmaKaydi { get; set; }

            public string Ad { get; set; }
            public string Soyad { get; set; }
            public string DogumTarihi { get; set; }
            public string Departman { get; set; }
            public string Unvan { get; set; }
            public string Email { get; set; }
            public string SabitTel { get; set; }
            public string CepTel { get; set; }
            public string Adres { get; set; }
            public string YasadigiIl { get; set; }
            public string YasadigiIlce { get; set; }
            public string TCKimlikNo { get; set; }
            public string SirketCepTel { get; set; }
            public string CalistigiSirket { get; set; }
            public string SigortaUnvani { get; set; }
            public string Kademe { get; set; }
            public string IseBaslangicTarihi { get; set; }
            public string ProfilFoto_Vesikalık { get; set; }

        }

        BindingList<Columns> columnsList = new BindingList<Columns>();

        UT_City[] _city;
        UT_Town[] _town;

        static string ConnectionString = "";

        public Form1()
        {
            InitializeComponent();

            dataGridViewColumn.DataSource = columnsList;
        }

        private void btnBağlan_Click(object sender, EventArgs e)
        {
            if (Test())
            {
                panel1.Enabled = true;
                btnBağlan.BackColor = Color.Yellow;
                return;

            }
            else
            {
                btnBağlan.BackColor = Color.Red;

                panel1.Enabled = false;
            }
        }
        private bool Test()
        {
            try
            {
                ConnectionString = $"Data Source={txtServer.Text};Initial Catalog={txtDBName.Text};User ID={txtUser.Text};Password={txtPass.Text}";
                var db = new WorkOfTimeDatabase(ConnectionString);
                var _Companies = db.GetINV_Company();


                _city = db.GetUT_City();
                _town = db.GetUT_Town();


                return true;

            }
            catch (Exception ex)
            {

                ConnectionString = "";
                MessageBox.Show(ex.Message);
                return false;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnDosyaAc_Click(object sender, EventArgs e)
        {
            txtFileName.Text = "";
            panel3.Enabled = false;
            columnsList.Clear();

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Excel Dosyası |*.xlsx| Excel Dosyası|*.xls";
            file.FilterIndex = 1;
            if (file.ShowDialog() == DialogResult.OK & !string.IsNullOrEmpty(file.FileName) && File.Exists(file.FileName))
            {
                try
                {
                    var dt = tool.ReadExcel(file.FileName);
                    foreach (DataRow column in dt.Rows)
                    {
                        var s = column.ToObject<Columns>();



                        var db = new WorkOfTimeDatabase(ConnectionString);



                        s.FirmaKaydi = db.GetINV_CompanyByFullName(s.CalistigiSirket) != null;
                        s.KullanıcıKaydi = db.GetSH_UserByTckimlikNoCrypto(s.TCKimlikNo,new CryptographyHelper().Encrypt(s.TCKimlikNo)) != null;


                        columnsList.Add(s);


                    }
                    panel3.Enabled = true;

                    txtFileName.Text = file.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAktar_Click(object sender, EventArgs e)
        {

            if (columnsList.Count == 0)
            {
                MessageBox.Show("Kayıt Yok");
                return;
            }

            foreach (var item in columnsList)
            {


                var _cityId = _city.Where(a => tool.strCol(a.NAME_1) == tool.strCol(item.YasadigiIl)).FirstOrDefault();

                var _townId = _town.Where(a => tool.strCol(a.NAME_1) == tool.strCol(item.YasadigiIl) && tool.strCol(a.NAME_2) == tool.strCol(item.YasadigiIlce)).FirstOrDefault();


                if (item.FirmaKaydi == false)
                {
                    var _company = new INV_Company
                    {
                        FullName = item.CalistigiSirket,
                        //InvoiceCityId = _cityId?.id,
                        //InvoiceTownId = _townId?.id,
                        //InvoiceAddress = item.Adres,

                        //LocationCityId = _cityId?.id,
                        //LocationTownId = _townId?.id,
                        //Location = _townId.PolygonField,
                        IsOwner = true,
                        //LocationAddress = item.Adres,


                    };
                    var firma_insert = Insert(_company);
                    if (firma_insert.result == false)
                    {
                        MessageBox.Show(firma_insert.message, "Firma eklemede hata");
                    }
                    else
                    {
                        item.FirmaKaydi = true;
                    }
                }


                var db = new WorkOfTimeDatabase(ConnectionString);
                var _firma = db.GetINV_CompanyByFullName(item.CalistigiSirket);
                if (_firma != null)
                {

                    if (db.GetSH_UserByTckimlikNoCrypto(item.TCKimlikNo, new CryptographyHelper().Encrypt(item.TCKimlikNo)) == null)
                    {
                        var __user = new SH_User
                        {

                            city = _cityId?.id,
                            town = _townId?.id,

                            firstname = item.Ad,
                            lastname = item.Soyad,
                            birthday = Convert.ToDateTime(item.DogumTarihi),
                            title = item.Unvan,
                            email = item.Email,
                            phone = item.SabitTel,
                            cellphone = item.CepTel,
                            address = item.Adres,
                            status = true,
                            tckimlikno = item.TCKimlikNo,
                            

                        };

                       
                        var user_insert = PersonInserts(__user, _firma.id,Convert.ToDateTime(item.IseBaslangicTarihi));
                        if (user_insert.result == false)
                        {
                            MessageBox.Show(user_insert.message, "Kulanıcı eklemede hata");
                        }
                        else
                        {
                            item.KullanıcıKaydi = true;
                        }
                    }


                }
            }

            dataGridViewColumn.Refresh();

        }

        public ResultStatus Insert(INV_Company item)
        {
            //var userStatus = (PageSecurity)Session["userStatus"];
            //var feedback = new FeedBack();
            var updateCompanyList = new List<INV_Company>();

            var db = new WorkOfTimeDatabase(ConnectionString);
            var dbCompany = db.GetINV_CompanyByFullName(item.FullName);

            if (dbCompany != null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = ("Aynı Kayıt Zaten Bulunmaktadır.")
                };
            }

            //if (item.InvoiceCityId == null || item.InvoiceTownId == null || item.InvoiceAddress == null ||
            //    item.LocationCityId == null || item.LocationTownId == null || item.LocationAddress == null)
            //{
            //    return new ResultStatus
            //    {
            //        result = false,
            //        message = ("Eksik Adres Bilgisi. Lütfen adres bilgilerini doldurun")
            //    };
            //}

            item.created = DateTime.Now;
            item.createdby = Guid.Empty;

            item.Code = BusinessExtensions.B_GetIdCode();

            item.QRCode = item.Code.ToString();
            item.IsDistribution = false;//(Request["CalismaTipi"].IndexOf("0") > -1);
            item.IsSupplier = false; //(Request["CalismaTipi"].IndexOf("1") > -1);
            item.IsFactory = false; //(Request["CalismaTipi"].IndexOf("3") > -1);
            item.IsWarranty = false; //(Request["CalismaTipi"].IndexOf("4") > -1);
            item.IsOther = false; //(Request["CalismaTipi"].IndexOf("5") > -1);
            item.IsOwner = true; //(Request["CalismaTipi"].IndexOf("6") > -1);
            item.IsInstitution = false; //(Request["CalismaTipi"].IndexOf("7") > -1);

            //var companyAddress1 = new INV_CompanyAdress
            //{
            //    id = Guid.NewGuid(),
            //    created = DateTime.Now,
            //    createdby = Guid.Empty,
            //    AdressNo = (Int32)EnumINV_CompanyAdressCompanyAdressNo.Fatura,
            //    CompanyId = item.id,
            //    BaseAddress = true,
            //    CityId = item.InvoiceCityId,
            //    TownId = item.InvoiceTownId,
            //    Adress = item.InvoiceAddress
            //};

            //var companyAddress2 = new INV_CompanyAdress
            //{
            //    id = Guid.NewGuid(),
            //    created = DateTime.Now,
            //    createdby = Guid.Empty,
            //    AdressNo = item.PID == null ? (Int32)EnumINV_CompanyAdressCompanyAdressNo.Merkez : (Int32)EnumINV_CompanyAdressCompanyAdressNo.Sube,
            //    CompanyId = item.id,
            //    BaseAddress = true,
            //    CityId = item.LocationCityId,
            //    TownId = item.LocationTownId,
            //    Adress = item.LocationAddress
            //};



            //var depo = new INV_Storage
            //{
            //    created = DateTime.Now,
            //    createdby = Guid.Empty,
            //    StorageName = item.FullName + " Deposu",
            //    CompanyId = item.id,
            //    Adress = companyAddress1.Adress,
            //    CityId = item.LocationCityId,
            //    TownId = item.LocationTownId,
            //    //Location = ,
            //    Phone = item.Phone1,
            //};

            if (item.IsOwner == true)
            {
                var companyPid = db.GetINV_CompanyByPid(item.id);
                if (companyPid.Count() > 0)
                {
                    foreach (var company in companyPid)
                    {
                        company.changed = DateTime.Now;
                        company.changedby = Guid.Empty;
                        company.IsOwner = item.IsOwner;
                        updateCompanyList.Add(company);
                    }
                }
            }



            var resultList = new List<ResultStatus>();

            var trans = db.BeginTransaction();
            resultList.Add(db.InsertINV_Company(item, trans));
          //  resultList.Add(db.InsertINV_CompanyAdress(companyAddress1, trans));
          //  resultList.Add(db.InsertINV_CompanyAdress(companyAddress2, trans));
          //  resultList.Add(db.InsertINV_Storage(depo, trans));
            if (updateCompanyList.Count() > 0)
            {
                resultList.Add(db.BulkUpdateINV_Company(updateCompanyList, false, trans));
            }

            if (resultList.Count(a => a.result == false) > 0)
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "İşletme Kaydetme İşlemi Başarız Oldu. Lütfen Daha Sonra Tekrar Deneyiniz."
                };
            }

            trans.Commit();

            return new ResultStatus
            {
                result = true,
                message = ("İşletme Kaydetme İşlemi Başarılı")
            };

        }

        public ResultStatus PersonInserts(SH_User item,Guid CompanyId, DateTime jobStartDate)
        {
            var db = new WorkOfTimeDatabase(ConnectionString);
            //var userStatus = (PageSecurity)Session["userStatus"];
            //var feedback = new FeedBack();
            var resultList = new List<ResultStatus>();
            var trans = db.BeginTransaction();
            item.loginname = item.tckimlikno;

            item.idcode = BusinessExtensions.B_GetIdCode();

            //var validate = ((ResultStatus)ValidateTc(new CryptographyHelper().Encrypt(item.tckimlikno)).Data);

            //if (!validate.result)
            //{
            //    return Json(new ResultStatusUI
            //    {
            //        Result = validate.result,
            //        FeedBack = feedback.Warning(validate.message)
            //    }, JsonRequestBehavior.AllowGet);
            //}

            //validate = ((ResultStatus)ValidateLoginName(item.loginname).Data);

            //if (!validate.result)
            //{
            //    return Json(new ResultStatusUI
            //    {
            //        Result = validate.result,
            //        FeedBack = feedback.Warning(validate.message)
            //    }, JsonRequestBehavior.AllowGet);
            //}

            //validate = ((ResultStatus)ValidateEmail(item.email).Data);
            //if (!validate.result)
            //{
            //    return Json(new ResultStatusUI
            //    {
            //        Result = validate.result,
            //        FeedBack = feedback.Warning(validate.message)
            //    }, JsonRequestBehavior.AllowGet);
            //}

            //if (item.password != Request["rePassword"])
            //{
            //    return Json(new ResultStatusUI
            //    {
            //        Result = validate.result,
            //        FeedBack = feedback.Warning("Şifreler Uyuşmuyor")
            //    }, JsonRequestBehavior.AllowGet);
            //}

            var password = item.loginname;
            item.type = item.type ?? (int)EnumSH_UserType.Personel;
            item.created = DateTime.Now;
            item.createdby = Guid.Empty;
            item.firstname = item.firstname.ToUpper();
            item.lastname = item.lastname.ToUpper();
            item.tckimlikno = new CryptographyHelper().Encrypt(item.tckimlikno);
            item.password = db.GetMd5Hash(db.GetMd5Hash(password));
            item.status = true;
            resultList.Add(db.InsertSH_User(item, trans));



            INV_CompanyPerson _companyPerson = new INV_CompanyPerson();

            _companyPerson.created = DateTime.Now;
            _companyPerson.createdby = Guid.Empty;
            _companyPerson.IdUser = item.id;

            _companyPerson.Title = item.title;
            _companyPerson.JobStartDate = jobStartDate;
            _companyPerson.CompanyId = CompanyId;


            resultList.Add(db.InsertINV_CompanyPerson(_companyPerson, trans));


            //item.INV_CompanyPersonSalary.created = DateTime.Now;
            //item.INV_CompanyPersonSalary.createdby = userStatus.user.id;
            //item.INV_CompanyPersonSalary.IdUser = item.id;
            //item.INV_CompanyPersonSalary.StartDate = item.INV_CompanyPerson.JobStartDate;
            //item.INV_CompanyPersonSalary.EndDate = item.INV_CompanyPerson.JobStartDate.Value.AddYears(15);
            //resultList.Add(db.InsertINV_CompanyPersonSalary(item.INV_CompanyPersonSalary, trans));



            //item.INV_CompanyPersonDepartments.created = DateTime.Now;
            //item.INV_CompanyPersonDepartments.createdby = userStatus.user.id;
            //item.INV_CompanyPersonDepartments.IdUser = item.id;
            //item.INV_CompanyPersonDepartments.StartDate = item.INV_CompanyPerson.JobStartDate;
            //item.INV_CompanyPersonDepartments.IsBasePosition = true;
            //resultList.Add(db.InsertINV_CompanyPersonDepartments(item.INV_CompanyPersonDepartments, trans));


            resultList.Add(db.InsertSH_UserRole(new SH_UserRole
            {
                userid = item.id,
                roleid = new Guid("fd2c02ae-994a-4f63-bd98-c1467743d770"),
                status = true,
                created = DateTime.Now,
                createdby = Guid.Empty,
                id = Guid.NewGuid()
            }, trans));




            if (resultList.Any(x => x.result == false))
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = ("Personel Kaydetme İşlemi Başarısız. Lütfen Daha Sonra Tekrar Deneyiniz..")
                };
            }


            trans.Commit();
            //new ManagersCalculator().Run();

            //string url = "http://" + Request.Url.Authority;
            //var mesajIcerigi = string.Format(@"<h3>Tebrikler!</h3> <p> WorkOfTime sistemine kaydınız başarı ile gerçekleştirildi.
            //        Infoline ailesi olarak sizi en içten duygularımızla tebrik ediyoruz ve ailemize çok şey katacağınıza inanıyoruz.</p>
            //        <p>Sisteme <u> Kimlik Numaranız</u> ve <u>Şifreniz</u> ile giriş sağlayabilirsiniz.</p>
            //        <p><strong> Şifreniz : <strong><span style='color: #ed5565;'>{1}</span></p>
            //        <p> Giriş yapmak için lütfen <a href = '{2}/Account/SignIn' > Buraya tıklayınız! </a></p> ", item.loginname, password, url);

            //new Email().Template("Template1", "userMailFoto.jpg", "Üyelik Kaydı Bilgilendirmesi", mesajIcerigi)
            //          .Send(item.email, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "Üyelik Bilgisi"), true);

            return new ResultStatus
            {
                result = true,
                message = ("Kullanıcı kaydetme işlemi başarıyla gerçekleşti..")
            };
        }
    }
}
