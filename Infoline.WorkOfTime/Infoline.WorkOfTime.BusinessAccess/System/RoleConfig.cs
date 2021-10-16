using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    public class PropInfo : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PropInfo(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
    }
    public class SHRoles
    {
        //Sistem Geliştirici
        [PropInfo("Sistem Yöneticisi", "Sistem rol geliştirmeleri ve tanımlama işlemlerini yapar.")]
        public const string SistemYonetici = "00000000-0000-0000-0000-000000000000";
        //Sistem Geliştirici

        //Personel Rolü
        [PropInfo("Personel Rolü", "İzin Talep edebilir,Görevlendirme doldurabilir.Özlük bilgilerine ulaşabilir.")]
        public const string Personel = "00000000-0000-0000-0000-100000000000";
        //Personel Rolü

        //Insan Kaynakları
        [PropInfo("Personel İdari Yönetici Rolü", "Personel taleplerini onaylar.Görevlendirme bilgilendirmeleri Mail yoluyla düşer.Personel 2,6 aylık değerlendirmeleri onayına sunulur.")]
        public const string IdariPersonelYonetici = "00000000-0000-0000-0000-200000000000";
        [PropInfo("Insan Kaynakları Yönetici Rolü", "Tüm personel işlemlerini yapabilir.")]
        public const string IKYonetici = "00000000-0000-0000-0000-220000000000";
        [PropInfo("Personel Talebi Rolü", "Personel talebi yapar.")]
        public const string PersonelTalebi = "00000000-0000-0000-0000-230000000000";
        [PropInfo("İş Görüşmesi Mülakat Rolü", "Yeni iş görüşmesi(mülakat) gerçekleştirir.")]
        public const string IsGorusmesiMulakat = "00000000-0000-0000-0000-240000000000";
        //Insan Kaynakları


        //Satın Alma
        [PropInfo("Satın Alma Talep Rolü", "Satın alma talebi yapabilir.")]
        public const string SatinAlmaTalebi = "00000000-0000-0000-0000-300000000000";
        [PropInfo("Satın Alma Personeli Rolü", "Satın alma tekliflerini toplar.")]
        public const string SatinAlmaPersonel = "00000000-0000-0000-0000-310000000000";
        [PropInfo("Satın Alma Onay Rolü", "Satın alma taleplerini onaylayıp reddedebilir.")]
        public const string SatinAlmaOnaylayici = "00000000-0000-0000-0000-330000000000";
        //Satın Alma

        //Satış
        [PropInfo("Satış Sipariş Onay Rolü", "Satış teklifi ve satış siparişini onaylar veya reddeder.")]
        public const string SatisOnaylayici = "00000000-0000-0000-0000-400000000000";
        [PropInfo("Satış Personeli/Takım Lideri Rolü", "Satış Fırsatı,Teklifi ve siparişi ekleyebilir.Aktiviteler vs oluşturabilir.")]
        public const string SatisPersoneli = "00000000-0000-0000-0000-420000000000";
        [PropInfo("Satış&Sipariş Fatura Rolü", "Kendi eklediği onaylanan siparişlerin ve tekliflerin faturasını girebilir.")]
        public const string SatisFatura = "00000000-0000-0000-0000-430000000000";
        //Satış

        //CRM //Açıklamalar düzeltilmeli
        [PropInfo("Satış CRM Yöneticisi Rolü", "Tüm Satış Fırsatlarını görüntüleyebilir.Raporlarına erişebilir.")]
        public const string CRMYonetici = "00000000-0000-0000-0000-710000000000";
        //CRM

        //Muhasebe
        [PropInfo("Ön Muhasebe Rolü", "Müşteri carilerini görüntülebilir,ekleyebilir,Hesap hareketlerini görüntüleyebilir. Fatura girebilir. Tahsilat yapabilir.")]
        public const string OnMuhasebe = "00000000-0000-0000-0000-500000000000";
        [PropInfo("Muhasebe Satış Faturaları Rolü", "Satış faturalarını görüntüleyebilir. Satış faturası girebilir. Onaylanmış satış siparişi ve satış tekliflerini görebilir, faturaya çevirebilir.")]
        public const string MuhasebeSatis = "00000000-0000-0000-0000-510000000000";
        [PropInfo("Muhasebe Alış Faturaları Rolü", "Alış faturalarını görüntüleyebilir. Alış faturası girebilir. Onaylanmış satın alma tekliflerini görebilir ve faturaya çevirebilir.")]
        public const string MuhasebeAlis = "00000000-0000-0000-0000-520000000000";
        //Muhasebe

        //Saha Görev
        [PropInfo("Görev Personeli Rolü", "Saha görevini yapacak ve çözüm bildirecek personellerdir.")]
        public const string SahaGorevPersonel = "00000000-0000-0000-0000-600000000000";
        [PropInfo("Görev Yöneticisi Rolü", "Saha görevi açar, düzenler ve bildirilen çözümleri onaylar,saha görev raporlarını görüntüleyebilir.")]
        public const string SahaGorevYonetici = "00000000-0000-0000-0000-610000000000";
        [PropInfo("Görev Operatör Rolü", "Saha görevi açar, düzenler ve bildirilen çözümleri onaylar.")]
        public const string SahaGorevOperator = "00000000-0000-0000-0000-620000000000";
        [PropInfo("Alt Yüklenici Personeli Rolü", "Kendisine açılan talepleri görüntüler.")]
        public const string YukleniciPersoneli = "00000000-0000-0000-0000-690000000000";
        [PropInfo("Görev Müşteri Rolü", " ")]
        public const string SahaGorevMusteri = "00000000-0000-0000-0000-110000000000";
        //Saha Görev


        //Uretim 
        [PropInfo("Üretim Süreci Yönetici Rolü", "Üretim Sürec Modülü üzerinde tüm işlemleri gerçekleştirebilir.")]
        public const string UretimYonetici = "00000000-0000-0000-0000-101000000000";
        [PropInfo("Üretim Süreci Personel Rolü", "Üretim Sürec Modülü üzerinde operasyon işlemlerini gerçekleştirebilir.")]
        public const string UretimPersonel = "00000000-0000-0000-0000-102000000000";
        //Uretim


        //Proje
        [PropInfo("Proje Personeli Rolü", "Projeleri Görüntüleyebilir.")]
        public const string ProjePersonel = "00000000-0000-0000-0000-800000000000";
        [PropInfo("Proje Yöneticisi Rolü", "Projeleri Ekleyebilir,Görüntüleyebilir,Silebilir.")]
        public const string ProjeYonetici = "00000000-0000-0000-0000-810000000000";
        //Proje

        //Ürün Stok
        [PropInfo("Ürün&Stok Yöneticisi Rolü", "Ürün&Stok veri girişlerini sağlar.Sistemdeki envanterleri görüntüleyebilir.Stok fiyat güncellemelerini yapar.Stok planlamalarını yapar.")]
        public const string StokYoneticisi = "00000000-0000-0000-0000-900000000000";
        [PropInfo("Depo Sorumlusu Rolü", "Ürün Stok işlemleri yapar.Kendi depolarını görüntüleyebilir.")]
        public const string DepoSorumlusu = "00000000-0000-0000-0000-920000000000";
        //Ürün Stok

        //Ticket / HelpDesk Yönetimi
        [PropInfo("Yardım Masası Yönetim Rolü", "Yönetici olarak atandığı konuları görüntüler ve alt konu ile çözüm önerisi oluşturabilir. Raporları görüntüler. Ticket oluşturabilir. Oluşturan ticket için personel ataması yapabilir.")]
        public const string YardimMasaYonetim = "00000000-0000-0000-0002-000000000000";
        [PropInfo("Yardım Masası Personel Rolü", "Başkası ve kendi adına ticket oluşturabilir. Kendi adına açılmış ticketları görüntüleyebilir ve çözümleyebilir. Ticket detayından mesaj yazabilir.")]
        public const string YardimMasaPersonel = "00000000-0000-0000-0002-100000000000";
        [PropInfo("Yardım Masası Talep Personel Rolü", "Yardım Masası (HelpDesk/Ticket) Sisteminde ticket oluşturabilecek kişilere verilecek roldür.")]
        public const string YardimMasaTalep = "00000000-0000-0000-0002-200000000000";
        [PropInfo("Yardım Masası Müşteri Rolü", "Başkası ve kendi adına ticket oluşturabilir. Kendi adına açılmış ticketları görüntüleyebilir ve çözümleyebilir. Ticket detayından mesaj yazabilir.")]
        public const string YardimMasaMusteri = "00000000-0000-0000-0002-300000000000";
        //Ticket / HelpDesk Yönetimi

        //Akademik Personel
        [PropInfo("Akademik Personel Rolü", "Akademik personeller izin talep vs. işlemlerini gerçekleştirebilir.")]
        public const string AkademikPersonel = "00000000-0000-0000-0003-000000000000";
        //Akademik Personel

        //Dil Yönetimi
        [PropInfo("Dil Tanımlamaları Rolü", "Dil ile ilgil tanımlamaları yapar.")]
        public const string Cevirmen = "00000000-0000-0000-0004-000000000000";
        //Dil Yönetimi

        //Bayi Yönetimi
        [PropInfo("Bayi Personeli Rolü", "Potansiyel, Sipariş, Teklif ve Görev işlemlerini yapar.")]
        public const string BayiPersoneli = "00000000-0000-0000-0005-000000000000";
        [PropInfo("Bayi Görev Personeli Rolü", "Saha görevlerini yapar.")]
        public const string BayiGorevPersoneli = "00000000-0000-0000-0005-200000000000";
        [PropInfo("Çağrı Merkezi Rolü", "Bayinin potansiyel işlemlerini yapar.")]
        public const string CagriMerkezi = "00000000-0000-0000-0005-100000000000";
        //Bayi Yönetimi

        //Doküman Yönetimi
        [PropInfo("Doküman Yönetim Rolü", "Doküman sayfaları ile ilgili işlemleri yapar.")]
        public const string DokumanYonetimRolu = "00000000-0000-0000-0006-000000000000";
        //Doküman Yönetimi

    }

    public class PageInfo : Attribute
    {
        public string Description { get; set; }
        public string[] Roles { get; set; }
        public PageInfo(string description, params string[] roles)
        {
            this.Description = description;
            this.Roles = roles;
        }

        public static List<SH_Pages> GetPages()
        {
            var list = new List<SH_Pages>();
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("Infoline.WorkOfTime.WebProject"))
                .Select(a => a.GetTypes().Where(c => c.Name.EndsWith("Controller"))).FirstOrDefault();

            if (controllers == null)
            {
                return list;
            }


            foreach (var cntrlr in controllers)
            {
                var ctrlrattr = cntrlr.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name).ToList();
                var methods = cntrlr.GetMethods().Where(a => a.DeclaringType.FullName.StartsWith("Infoline.WorkOfTime.WebProject")).ToArray();
                foreach (var mthd in methods.GroupBy(a => a.Name))
                {
                    string area = cntrlr.FullName.IndexOf("Areas.") > -1 ? cntrlr.FullName.Split(new string[] { "Areas." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault().Split('.').FirstOrDefault() : null;
                    string controller = cntrlr.Name.Replace("Controller", "");
                    string method = mthd.Key;
                    string action = String.Format("{0}/{1}/{2}", string.IsNullOrEmpty(area) ? "" : "/" + area, controller, method);
                    var attr = mthd.SelectMany(a => a.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name)).ToList();

                    var page = new SH_Pages
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        Action = action,
                        Area = area,
                        Controller = controller,
                        Method = method,
                        AllowEveryone = ctrlrattr.Contains("AllowEveryone") || attr.Contains("AllowEveryone")
                    };
                    list.Add(page);
                }
            }

            return list;
        }

        public static List<string> GetUnAutorizePagesByRole(params string[] role)
        {
            var list = new List<string>();
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("Infoline.WorkOfTime.WebProject"))
                .Select(a => a.GetTypes().Where(c => c.Name.EndsWith("Controller"))).FirstOrDefault();

            foreach (var cntrlr in controllers)
            {
                var ctrlrattr = cntrlr.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name).ToList();
                var methods = cntrlr.GetMethods().Where(a => a.DeclaringType.FullName.StartsWith("Infoline.WorkOfTime.WebProject")).ToArray();
                foreach (var mthd in methods.GroupBy(a => a.Name))
                {
                    string area = cntrlr.FullName.IndexOf("Areas.") > -1 ? cntrlr.FullName.Split(new string[] { "Areas." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault().Split('.').FirstOrDefault() : null;
                    string controller = cntrlr.Name.Replace("Controller", "");
                    string method = mthd.Key;
                    string action = String.Format("{0}/{1}/{2}", string.IsNullOrEmpty(area) ? "" : "/" + area, controller, method);
                    var attr = mthd.SelectMany(a => a.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name)).ToList();
                    if (ctrlrattr.Contains("AllowEveryone") || attr.Contains("AllowEveryone") == false)
                    {
                        if (attr.IndexOf("PageInfo") > -1)
                        {
                            var pageInfoList = mthd.SelectMany(a => a.GetCustomAttributes(typeof(PageInfo), false)).Select(a => (PageInfo)a).ToArray();
                            var roller = pageInfoList.SelectMany(a => a.Roles).GroupBy(a => a).Select(a => a.Key).ToArray();
                            if (!roller.Any(x => role.Contains(x)))
                            {
                                list.Add(action);
                            }
                        }
                        else
                        {
                            list.Add(action);
                        }
                    }
                }
            }

            return list;
        }






        public static void Execute(List<string> tenantRoles = null)
        {
            var db = new WorkOfTimeDatabase();
            #region Roller
            var userRoles = new List<SH_UserRole>();
            var roles = new List<SH_Role>();
            foreach (var item in typeof(SHRoles).GetFields())
            {
                var id = new Guid(item.GetValue(null).ToString());
                var prop = ((PropInfo[])item.GetCustomAttributes(typeof(PropInfo), false)).FirstOrDefault();
                roles.Add(new SH_Role
                {
                    id = id,
                    rolname = prop != null ? prop.Name : item.Name,
                    roledescription = prop != null ? prop.Description : item.Name,
                    roletype = (int)EnumSH_RolesRoleType.systemdefine,
                });
            }
            #endregion
            #region Sayfalar
            var pages = new List<SH_Pages>();
            var pagesRole = new List<SH_PagesRole>();
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("Infoline.WorkOfTime.WebProject"))
                .Select(a => a.GetTypes().Where(c => c.Name.EndsWith("Controller"))).FirstOrDefault();

            foreach (var cntrlr in controllers)
            {
                var ctrlrattr = cntrlr.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name).ToList();
                var methods = cntrlr.GetMethods().Where(a => a.DeclaringType.FullName.StartsWith("Infoline.WorkOfTime.WebProject")).ToArray();
                foreach (var mthd in methods.GroupBy(a => a.Name))
                {
                    string area = cntrlr.FullName.IndexOf("Areas.") > -1 ? cntrlr.FullName.Split(new string[] { "Areas." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault().Split('.').FirstOrDefault() : null;
                    string controller = cntrlr.Name.Replace("Controller", "");
                    string method = mthd.Key;
                    string action = String.Format("{0}/{1}/{2}", string.IsNullOrEmpty(area) ? "" : "/" + area, controller, method);
                    string description = string.Empty;
                    var attr = mthd.SelectMany(a => a.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name)).ToList();

                    var page = new SH_Pages
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        Action = action,
                        Area = area,
                        Controller = controller,
                        Method = method,
                        AllowEveryone = ctrlrattr.Contains("AllowEveryone") || attr.Contains("AllowEveryone")
                    };

                    if (attr.IndexOf("PageInfo") > -1)
                    {
                        var pageInfoList = mthd.SelectMany(a => a.GetCustomAttributes(typeof(PageInfo), false)).Select(a => (PageInfo)a).ToArray();
                        var roller = pageInfoList.SelectMany(a => a.Roles).GroupBy(a => a).ToArray();
                        page.Description = pageInfoList.Where(a => a.Description != null).Select(a => a.Description).FirstOrDefault();
                        pagesRole.AddRange(roller.Select(rol => new SH_PagesRole
                        {
                            action = action,
                            roleid = new Guid(rol.Key)
                        }).ToList());
                    }

                    pages.Add(page);
                }
            }
            #endregion
            #region Dosyalar
            var filesRole = new List<SH_FilesRole>();
            foreach (var key in LocalFileProvider._dataTableFileGroup.Keys)
            {
                foreach (var fb in LocalFileProvider._dataTableFileGroup[key])
                {
                    var rolList = new List<string>();
                    rolList.AddRange(fb.insert != null ? fb.insert : new string[] { });
                    rolList.AddRange(fb.delete != null ? fb.delete : new string[] { });
                    rolList.AddRange(fb.preview != null ? fb.preview : new string[] { });
                    rolList = rolList.GroupBy(a => a).Select(a => a.Key).ToList();

                    foreach (var role in rolList)
                    {
                        filesRole.Add(new SH_FilesRole
                        {
                            roleid = new Guid(role),
                            dataTable = key,
                            fileGroup = fb.fileGroup,
                            delete = fb.delete.Contains(role),
                            insert = fb.insert.Contains(role),
                            preview = fb.preview.Contains(role),
                        });
                    }


                    filesRole.Add(new SH_FilesRole
                    {
                        roleid = new Guid(SHRoles.SistemYonetici),
                        dataTable = key,
                        fileGroup = fb.fileGroup,
                        delete = true,
                        insert = true,
                        preview = true,
                    });


                }
            }
            #endregion


            var trans = db.BeginTransaction();
            var roleUserDefine = db.GetSH_Role().Where(a => a.roletype == (int)EnumSH_RolesRoleType.userdefine);
            var pagesRoleDB = db.GetSH_PagesRole();
            var filesRoleDB = db.GetSH_FilesRole();
            var usersRoleDB = db.GetSH_UserRole();
            var developerUserDB = db.GetSH_UserById(Guid.Empty);
            var developerRoleDB = db.GetSH_UserRoleByUserIdRoleId(Guid.Empty, Guid.Empty);
            roles.AddRange(roleUserDefine);

            //Kullanıcı Rolleri Ayarlanıyor
            foreach (var item in usersRoleDB.GroupBy(a => new { a.roleid, a.userid }).ToArray())
            {
                if (roles.Count(a => a.id == item.Key.roleid) > 0)
                {
                    userRoles.Add(item.FirstOrDefault());
                }
            }
            //Kullanıcı Rolleri Ayarlanıyor


            //Custom Rollerdeki File Roller ekleniyor
            filesRole.AddRange(filesRoleDB.Where(a => roleUserDefine.Count(c => c.id == a.roleid) > 0).ToList());
            //Custom Rollerdeki File Roller ekleniyor

            //Custom Rollerdeki Page Roller ekleniyor
            pagesRole.AddRange(pagesRoleDB.Where(a => roleUserDefine.Count(c => c.id == a.roleid) > 0 && pages.Select(c => c.Action).Contains(a.action)).ToList());
            //Custom Rollerdeki Page Roller ekleniyor

            var rs = new ResultStatus { result = true };

            /*Delete İşlemleri*/
            rs &= db.DeleteSH_PagesRoleAll(trans);
            rs &= db.DeleteSH_FilesRoleAll(trans);
            rs &= db.DeleteSH_UserRoleAll(trans);
            rs &= db.DeleteSH_PagesAll(trans);
            rs &= db.DeleteSH_RolesAll(trans);
            /*Delete İşlemleri*/


            /*Insert İşlemleri*/
            if (tenantRoles != null && tenantRoles.Count() > 0)
            {
                var tenantRoleIds = tenantRoles.Select(a => new Guid(a));
                roles = roles.Where(a => tenantRoleIds.Contains(a.id) || a.roletype == (int)EnumSH_RolesRoleType.userdefine).ToList();
                filesRole = filesRole.Where(a => tenantRoleIds.Contains(a.roleid.Value) || roles.Where(x => x.roletype == (int)EnumSH_RolesRoleType.userdefine).Select(x => x.id).ToArray().Contains(a.roleid.Value)).ToList();
                userRoles = userRoles.Where(a => tenantRoleIds.Contains(a.roleid.Value) || roles.Where(x => x.roletype == (int)EnumSH_RolesRoleType.userdefine).Select(x => x.id).ToArray().Contains(a.roleid.Value)).ToList();
                pagesRole = pagesRole.Where(a => tenantRoleIds.Contains(a.roleid.Value) || roles.Where(x => x.roletype == (int)EnumSH_RolesRoleType.userdefine).Select(x => x.id).ToArray().Contains(a.roleid.Value)).ToList();
                var actions = pagesRole.GroupBy(a => a.action).Select(c => c.Key);
                pages = pages.Where(a => actions.Contains(a.Action)).ToList();
                pagesRole.AddRange(pages.Select(a => new SH_PagesRole { action = a.Action, roleid = new Guid(SHRoles.SistemYonetici) }).ToList());
            }

            rs &= db.BulkInsertSH_Role(roles, trans);
            rs &= db.BulkInsertSH_Pages(pages, trans);
            rs &= db.BulkInsertSH_UserRole(userRoles, trans);
            rs &= db.BulkInsertSH_FilesRole(filesRole, trans);
            rs &= db.BulkInsertSH_PagesRole(pagesRole, trans);


            /*Insert İşlemleri*/

            if (developerUserDB == null)
            {
                rs &= db.InsertSH_User(new SH_User
                {
                    id = Guid.Empty,
                    email = "developer@infoline-tr.com",
                    loginname = "developer",
                    firstname = "workoftime",
                    lastname = "developer",
                    title = "Yazılım Geliştirici",
                    password = db.GetMd5Hash(db.GetMd5Hash("devinfo2021.")),
                    status = true,
                }, trans);
            }

            if (developerRoleDB.Count() == 0)
            {
                rs &= db.InsertSH_UserRole(new SH_UserRole
                {
                    id = Guid.NewGuid(),
                    roleid = new Guid(SHRoles.SistemYonetici),
                    userid = Guid.Empty,
                }, trans);
            }

            if (rs.result) { trans.Commit(); } else { trans.Rollback(); }

        }

    }


}
