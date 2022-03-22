using Infoline.WorkOfTime.BusinessAccess;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public class NavModel
    {
        private List<Menu> _root = new List<Menu>();

        public NavModel(PageSecurity userStatus)
        {
            if ((TenantConfig.Tenant.TenantCode == 1100 || TenantConfig.Tenant.TenantCode == 1000 || TenantConfig.Tenant.TenantCode == 1187) && userStatus.user.id == new Guid("DCEAA584-35B7-4637-AF55-48CCF013C9D3"))
            {
                _root.Add(GetMenuForScada());
            }
            else
            {
                if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)) && TenantConfig.Tenant.TenantCode == 1179)
                {
                    _root.Add(GetCRM());
                    _root.Add(GetSatisSiparis());
                    _root.Add(GetGorevYonetimi());
                }
                else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiGorevPersoneli)) && TenantConfig.Tenant.TenantCode == 1179)
                {
                    _root.Add(GetGorevYonetimi());
                }
                else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.YardimMasaMusteri)))
                {
                    if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevMusteri)))
                    {
                        _root.Add(GetCustomerPage());
                        _root.Add(GetGorevYonetimi());
                    }
                    _root.Add(GetYardimDestekYonetimi());
                }
                else
                {
                    _root.Add(GetHomePage());
                    _root.Add(MyJobs());
                    _root.Add(GetOfisYonetimi());
                    _root.Add(GetIK(userStatus));
                    if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.ISGSorumlusu)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
                    {
                        _root.Add(GetISG());
                    }
                    _root.Add(GetCRM());
                    _root.Add(GetSatisSiparis());
                    _root.Add(GetSatinAlma());
                    if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
                    {
                        _root.Add(GetStokSevkiyat());
                    }
                    if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
                    {
                        _root.Add(new Menu("Şirketlerim", "/CMP/VWCMP_Company/IndexCompany","fa fa-building"));
                    }
                    _root.Add(GetProduction());
                    _root.Add(GetService());
                    _root.Add(GetProje());
                    if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevMusteri)))
                    {
                        _root.Add(GetGorevYonetimi());

                    }
                    else
                    {
                        _root.Add(GorevYonetimiForCustomer());
                    }
                    _root.Add(GetIsletmeCariler());
                    _root.Add(GetMuhasabeYonetimi());
                    _root.Add(GetYardimDestekYonetimi());
                    if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
                    {
                        _root.Add(GetYonetim());
                        _root.Add(GetExternalLinks());
                    }
                }
            }



        }

        private Menu GetExternalLinks()
        {
            var menu = new Menu("Dış Bağlantılar", "#", "fa fa-link");
            var db = new WorkOfTimeDatabase();
            var links = db.GetVWSYS_ExternalLinks();
            foreach (var link in links)
            {
                menu.AddChild(new Menu(link.Label, link.Url, "", true));
            }
            return menu;
        }

        private Menu GetMenuForScada()
        {
            var menu = new Menu("Görevler", "/FTM/VWFTM_TaskGrid/ScadaTasks", " fa fa-wrench");
            return menu;
        }

        private Menu GetYardimDestekYonetimi()
        {
            var menu = new Menu("Yardım&Destek İşlemleri", "#", "icon-question");
            menu.AddChild(new Menu("Yardım Talepleri", "/HDM/VWHDM_Ticket"));
            menu.AddChild(new Menu("Yardım Taleplerim", "/HDM/VWHDM_Ticket/IndexMy"));
            menu.AddChild(new Menu("Talep Oluştur", "/HDM/VWHDM_Ticket/InsertBasic"));
            menu.AddChild(new Menu("Konu ve Öneriler", "/HDM/VWHDM_Issue"));
            menu.AddChild(new Menu("Sıkça Sorulan Sorular", "/HDM/VWHDM_Issue/Help"));
            return menu;
        }

        private Menu GetHomePage()
        {
            var mypage = new Menu("Ana Sayfa", "/Account/Index", "fa fa-home");
            return mypage;
        }

        private Menu GetCustomerPage()
        {
            var mypage = new Menu("Görev Raporu", "/Customer/Index", "fa fa-wrench");
            return mypage;
        }

        private Menu MyJobs()
        {
            var menu = new Menu("Benim İşlemlerim", "#", "fa fa-user");
            menu.AddChild(new Menu("Bilgilerim", "/Account/Profile"));
            menu.AddChild(new Menu("İzinlerim", "/INV/VWINV_Permit/MyIndex"));
            menu.AddChild(new Menu("İzin Onaylar", "/INV/VWINV_Permit/MyAboutIndex"));
            menu.AddChild(new Menu("Masraflarım", "/PA/VWPA_Transaction/IndexMyRequest"));
            menu.AddChild(new Menu("Masraf Onaylar", "/PA/VWPA_Transaction/IndexRequest"));
            menu.AddChild(new Menu("Avanslarım", "/PA/VWPA_Advance/IndexMyRequest"));
            menu.AddChild(new Menu("Avans Onaylar", "/PA/VWPA_Advance/IndexRequest"));
            menu.AddChild(new Menu("Seyahat/Görevlendirmelerim", "/INV/VWINV_Commissions/MyIndex"));
            menu.AddChild(new Menu("Seyahat/Görevlendirme Onaylar", "/INV/VWINV_Commissions/MyAboutIndex"));
            menu.AddChild(new Menu("Değerlendirmelerim", "/PDS/VWPDS_FormPermitTaskUser"));
            menu.AddChild(new Menu("Yeni Personel Değerledir", "/INV/VWINV_CompanyPersonAssessment"));
            menu.AddChild(new Menu("Araç Kilometre Bilgilerim", "/CMP/VWCMP_CompanyCarKilometer/Index"));
            return menu;
        }

        private Menu GetOfisYonetimi()
        {
            var menu = new Menu("Ofis Yönetimi", "#", "fa fa-building-o");
            menu.AddChild(new Menu("Takvim / Gündem", "/INV/VWINV_CompanyPersonCalendar/Calendar"));
            menu.AddChild(new Menu("Şirket Rehberi", "/SH/VWSH_User/ContactOffice"));
            menu.AddChild(new Menu("Şirket Kütüphanesi", "/CMP/VWCMP_Company/PersonnelRegulationDocument"));
            menu.AddChild(new Menu("Şirket Organizasyon", "/INV/VWINV_CompanyDepartments/Preview"));
            menu.AddChild(new Menu("Şirket Portalı", "#"));
            menu.AddChild(new Menu("Şirket Bilgileri", "/CMP/VWCMP_Company/IndexMy"));
            menu.AddChild(new Menu("Şirket Şube/Depo/Kısımları", "/CMP/VWCMP_Storage/IndexMy"));
            var dokuman = new Menu("Doküman Yönetimi");
            dokuman.AddChild(new Menu("Dokümanlar", "/DOC/VWDOC_Document/Index"));
            dokuman.AddChild(new Menu("Tüm Revizyon Talepleri", "/DOC/VWDOC_DocumentRevisionRequest/Index"));
            dokuman.AddChild(new Menu("Revizyon Taleplerim", "/DOC/VWDOC_DocumentRevisionRequest/MyIndex"));
            dokuman.AddChild(new Menu("Revizyon Talepleri (Onay)", "/DOC/VWDOC_DocumentRevisionRequest/MyAboutIndex"));
            menu.AddChild(dokuman);

            return menu;
        }

        private Menu GetIK(PageSecurity userStatus)
        {
            var ik = new Menu("İnsan Kaynakları", "#", "fa fa-users");
            var personel = new Menu("Personel İşlemleri");
            personel.AddChild(new Menu("Personel Listesi", "/SH/VWSH_User/Index"));
            personel.AddChild(new Menu("Personel Raporları", "/INV/INV_CompanyPerson/Dashboard"));
            personel.AddChild(new Menu("Sertifika/Dosya/Eğitim Raporları", "/SH/VWSH_User/CertificateReport"));
            personel.AddChild(new Menu("Giriş-Çıkış Raporları", "/SH/VWSH_ShiftTracking/Index"));
            personel.AddChild(new Menu("Çalışma Durumu Raporları", "/SH/VWSH_ShiftTracking/TotalStaffWorkingStatus"));
            personel.AddChild(new Menu("Mesai Takip Raporları", "/SH/VWSH_ShiftTracking/StaffWorkingStatus"));
            personel.AddChild(new Menu("Detaylı Personel Raporları", "/SH/VWSH_UserReport"));
            personel.AddChild(new Menu("Organizasyon Şeması Yönetimi", "/INV/VWINV_CompanyDepartments"));


            ik.AddChild(personel);

            var izinislemleri = new Menu("İzin İşlemleri");
            izinislemleri.AddChild(new Menu("Tüm İzinler", "/INV/VWINV_Permit/Index"));
            izinislemleri.AddChild(new Menu("İzin Raporları", "/INV/VWINV_Permit/Dashboard"));
            izinislemleri.AddChild(new Menu("Personel İzin Özet Raporu", "/INV/VWINV_Permit/SummaryReport"));
            izinislemleri.AddChild(new Menu("Yıl Bazlı Personel İzin Raporları", "/INV/VWINV_Permit/YearlyStaffReport"));
            izinislemleri.AddChild(new Menu("İzin Tipi Tanımları", "/INV/INV_PermitType/Index"));
            izinislemleri.AddChild(new Menu("Resmi İzin Tanımları", "/INV/VWINV_PermitOffical/Index"));
            ik.AddChild(izinislemleri);

            var görevlendirme = new Menu("Görevlendirme İşlemleri");
            görevlendirme.AddChild(new Menu("Tüm Görevlendirmeler", "/INV/VWINV_Commissions/Index"));
            görevlendirme.AddChild(new Menu("Görevlendirme Raporları", "/INV/VWINV_Commissions/Dashboard"));

            ik.AddChild(görevlendirme);

            var maas = new Menu("Maaş İşlemleri");
            maas.AddChild(new Menu("Tüm Maaşlar", "/INV/VWINV_CompanyPersonSalary/Index"));
            maas.AddChild(new Menu("Maaş Raporları", "#"));
            maas.AddChild(new Menu("Maaş Geçmişim", "#"));
            ik.AddChild(maas);

            var masraf = new Menu("Masraf İşlemleri");
            masraf.AddChild(new Menu("Tüm Masraf Talepleri", "/PA/VWPA_Transaction/IndexAllRequest"));
            ik.AddChild(masraf);

            var avans = new Menu("Avans İşlemleri");
            avans.AddChild(new Menu("Tüm Avanslar", "/PA/VWPA_Advance/IndexAllRequest"));
            ik.AddChild(avans);

            var degerlendirme = new Menu("Performans 360");
            degerlendirme.AddChild(new Menu("Tüm Değerlendirmeler", "/PDS/VWPDS_FormResult/AllIndex"));
            degerlendirme.AddChild(new Menu("Değerlendirme Formları", "/PDS/VWPDS_EvaluateForm"));
            ik.AddChild(degerlendirme);

            var cvhavuzu = new Menu("Personel Adayı İşlemleri");
            cvhavuzu.AddChild(new Menu("CV Havuzu", "/HR/VWHR_Person"));
            cvhavuzu.AddChild(new Menu("Personel Talep İşlemleri", "/HR/VWHR_StaffNeeds/MyIndex"));
            cvhavuzu.AddChild(new Menu("Mülakatlar", "/HR/VWHR_PlanPerson/MyIndex"));
            ik.AddChild(cvhavuzu);

            var personeltakip = new Menu("Personel Takip İşlemleri");
            personeltakip.AddChild(new Menu("Personel Anlık Takip Haritası", "/SH/VWSH_UserLocationTracking/MapAll"));
            if (userStatus.user.id == Guid.Empty)
            {
                personeltakip.AddChild(new Menu("Personel Takip Haritası", "/SH/VWSH_UserLocationTracking/Map"));
            }
            ik.AddChild(personeltakip);

            return ik;
        }

        private Menu GetISG()
        {
            var menu = new Menu("İş Sağlığı ve Güvenliği", "#", "icon-medkit");
            menu.AddChild(new Menu("Kaza ve Olay Bildirimleri", "/SH/VWSH_WorkAccident/Index"));
            menu.AddChild(new Menu("Düzenleyici Önleyici Faaliyetler", "/SH/VWSH_CorrectiveActivity/Index"));


            return menu;
        }

        private Menu GetCRM()
        {
            var crm = new Menu("CRM - Müşteri İlişkileri", "#", "fa fa-dollar");

            crm.AddChild(new Menu("Potansiyel Fırsatlar", "/CRM/VWCRM_Presentation/Index"));
            crm.AddChild(new Menu("Satış Duvarı", "/CRM/VWCRM_Presentation/AgileBoard"));
            crm.AddChild(new Menu("Aktivite ve Randevular", "/CRM/VWCRM_Contact/Index"));
            crm.AddChild(new Menu("Toplantı Raporu", "/CRM/VWCRM_Contact/ContactCalendar"));
            crm.AddChild(new Menu("Satış Raporu", "/CRM/VWCRM_Presentation/SalesReport"));


            var musteriler = new Menu("Müşteriler");
            musteriler.AddChild(new Menu("Tüm Müşteri Firmaları", "/CMP/VWCMP_Company/IndexCustomer"));
            musteriler.AddChild(new Menu("Müşteri Firmalarım", "/CMP/VWCMP_Company/IndexMyCustomer"));
            musteriler.AddChild(new Menu("Müşteri Kişilerim", "/SH/VWSH_User/ContactMyCustomersPersons"));
            crm.AddChild(musteriler);


            var haritalar = new Menu("Haritalar");
            haritalar.AddChild(new Menu("Tüm CRM Haritası", "/CRM/VWCRM_Presentation/Map"));
            haritalar.AddChild(new Menu("CRM Haritam", "/CRM/VWCRM_Presentation/MyMap"));

            crm.AddChild(haritalar);


            var musteritanimlari = new Menu("Müşteri İlişkileri Tanımları");
            musteritanimlari.AddChild(new Menu("Ürün Puanları", "/PRD/VWPRD_ProductPointSelling/Index"));
            musteritanimlari.AddChild(new Menu("Potansiyel/Fırsat Aşamaları", "/CRM/VWCRM_ManagerStage/Index"));
            musteritanimlari.AddChild(new Menu("Rakip Firmalar", "/CRM/CRM_OpponentCompany/Index"));
            crm.AddChild(musteritanimlari);

            return crm;
        }

        private Menu GetMuhasebe()
        {
            var muhasebe = new Menu("Muhasebe ve Finans", "#", "icon-calc");

            var finansislemleri = new Menu("Finans İşlemleri");
            finansislemleri.AddChild(new Menu("Gelirler", "#"));
            finansislemleri.AddChild(new Menu("Giderler", "#"));
            finansislemleri.AddChild(new Menu("Kasalar", "#"));
            finansislemleri.AddChild(new Menu("Personel Maaş Ödemeleri", "#"));
            finansislemleri.AddChild(new Menu("Personel Harcırah Ödemeleri", "#"));
            finansislemleri.AddChild(new Menu("Personel Masraf Ödemeleri", "#"));
            finansislemleri.AddChild(new Menu("Finansal Raporlar", "#"));
            muhasebe.AddChild(finansislemleri);
            muhasebe.AddChild(new Menu("Cariler", "/CMP/VWCMP_Company/Index"));

            return muhasebe;
        }

        private Menu GetService()
        {
            var service = new Menu("Teknik Servis Yönetimi", "#", "fa icon-tools");
            service.AddChild(new Menu("Teknik Servis Kayıtları", "/SV/VWSV_Service"));
            service.AddChild(new Menu("Cihaz Problemleri", "/SV/VWSV_Problem"));
            //service.AddChild(new Menu("Değişen Cihazlar", "/SV/VWSV_ChangedDevice"));

            return service;
        }
        private Menu GetProje()
        {
            var projeyonetimi = new Menu("Proje Yönetimi", "#", "icon-briefcase");
            projeyonetimi.AddChild(new Menu("Görev Aldığım Projeler", "/PRJ/VWPRJ_Project/MyIndex"));
            projeyonetimi.AddChild(new Menu("Tüm Projeler", "/PRJ/VWPRJ_Project"));
            projeyonetimi.AddChild(new Menu("Proje Görev Raporu", "/PRJ/VWPRJ_ProjectTimeline/Index"));
            projeyonetimi.AddChild(new Menu("Proje Raporları", "/PRJ/VWPRJ_Project/Dashboard"));
            return projeyonetimi;
        }

        private Menu GetGorevYonetimi()
        {

            var sahayonetimi = new Menu("Görev Yönetimi", "#", "fa fa-wrench");
            sahayonetimi.AddChild(new Menu("Görevler", "/FTM/VWFTM_Task/Index"));
            sahayonetimi.AddChild(new Menu("Bakım Planları", "/FTM/VWFTM_TaskPlan/AllTaskDetail"));
            sahayonetimi.AddChild(new Menu("Günlük İş Takip", "/FTM/VWFTM_Task/WorkReport"));
            sahayonetimi.AddChild(new Menu("Personel İş Takip", "/FTM/VWFTM_Task/PersonDailyReport"));
            sahayonetimi.AddChild(new Menu("Bakım Yönetimi", "/FTM/VWFTM_TaskPlan/Index"));
            sahayonetimi.AddChild(new Menu("Görev Haritası", "/FTM/VWFTM_Task/Map"));
            sahayonetimi.AddChild(new Menu("Görev Şablonları", "/FTM/VWFTM_TaskTemplate/Index"));
            sahayonetimi.AddChild(new Menu("Bakım Envanterleri", "/PRD/VWPRD_Inventory/IndexMaintance"));
            sahayonetimi.AddChild(new Menu("Görev Formları", "/FTM/VWFTM_TaskForm"));
            sahayonetimi.AddChild(new Menu("Görev Takvimi", "/FTM/VWFTM_TaskPlan/Calendar"));

            var gorevRaporlari = new Menu("Görev Raporları");
            gorevRaporlari.AddChild(new Menu("Operasyon Raporu", "/FTM/VWFTM_Task/OperationReport"));
            gorevRaporlari.AddChild(new Menu("Dashboard", "/FTM/VWFTM_Task/WeeklyReport"));
            gorevRaporlari.AddChild(new Menu("Aktivite İzleme", "/FTM/VWFTM_Task/ActivityTracking"));
            gorevRaporlari.AddChild(new Menu("Personel Anlık Takip Haritası", "/SH/VWSH_UserLocationTracking/MapAll"));
            gorevRaporlari.AddChild(new Menu("Personel Takip Haritası", "/SH/VWSH_UserLocationTracking/Map"));
            gorevRaporlari.AddChild(new Menu("Müşteri Raporu", "/Customer"));
            gorevRaporlari.AddChild(new Menu("Detaylı Rapor", "/FTM/VWFTM_TaskGrid/Index"));
            gorevRaporlari.AddChild(new Menu("Ay/Yıl Bazlı Rapor", "/FTM/VWFTM_Task/MonthlyTaskReport"));
            gorevRaporlari.AddChild(new Menu("Personel Yıl/Ay Bazlı Rapor", "/FTM/VWFTM_Task/MonthlyPersonelReport"));
            gorevRaporlari.AddChild(new Menu("Çağrı Tipi Yıl/Ay Bazlı Rapor", "/FTM/VWFTM_Task/MonthlyTypeReport"));
            gorevRaporlari.AddChild(new Menu("Personel Raporu", "/FTM/VWFTM_Task/StaffReport"));
            sahayonetimi.AddChild(gorevRaporlari);

            return sahayonetimi;
        }
        private Menu GorevYonetimiForCustomer()
        {
            var sahayonetimi = new Menu("Müşteri Görev Yönetimi", "#", "fa fa-wrench");
            sahayonetimi.AddChild(new Menu("Görevler", "/FTM/VWFTM_Task/Index"));
            return sahayonetimi;
        }

        private Menu GetSatinAlma()
        {
            var satinalma = new Menu("Satınalma Yönetimi", "#", "icon-basket-4");
            satinalma.AddChild(new Menu("Satın Alma Talepleri", "/CMP/VWCMP_Request/Index"));
            satinalma.AddChild(new Menu("Satın Alma Teklifleri", "/CMP/VWCMP_Tender/IndexBuying"));
            satinalma.AddChild(new Menu("Alış Faturaları", "/CMP/VWCMP_Invoice/IndexBuying"));
            satinalma.AddChild(new Menu("Alış Faturası Raporları", "/CMP/VWCMP_Invoice/IndexBuyingReport"));
            return satinalma;
        }

        private Menu GetSatisSiparis()
        {
            var menu = new Menu("Satış&Sipariş Yönetimi", "#", "icon-money");
            menu.AddChild(new Menu("Satış Teklifleri", "/CMP/VWCMP_Tender/IndexSelling"));
            menu.AddChild(new Menu("Satış Siparişleri", "/CMP/VWCMP_Order/IndexSelling"));
            menu.AddChild(new Menu("Satış Faturaları", "/CMP/VWCMP_Invoice/IndexSelling"));
            return menu;
        }

        private Menu GetStokSevkiyat()
        {
            var menu = new Menu("Depo ve Stok", "#", "fa fa-cubes");
            menu.AddChild(new Menu("Ürün & Hizmet Listesi", "/PRD/VWPRD_Product/Index"));
            menu.AddChild(new Menu("Stok ve Envanter İşlemleri", "/PRD/VWPRD_Transaction/Index"));
            menu.AddChild(new Menu("Dağıtım/Sevkiyat İşlemleri", "/PRD/VWPRD_DistributionPlan/Index"));
            menu.AddChild(new Menu("Envanter Listesi", "/PRD/VWPRD_Inventory/Index"));
            if (TenantConfig.Tenant.TenantCode == 1194 || TenantConfig.Tenant.TenantCode == 1201 || TenantConfig.Tenant.TenantCode == 1100 || TenantConfig.Tenant.TenantCode == 1000)
            {
                menu.AddChild(new Menu("Aktivasyonlar", "/PRD/VWPRD_TitanDeviceActivated"));
                menu.AddChild(new Menu("Sell Out Raporu", "/PRD/VWPRD_TitanDeviceActivated/SellOutDashboard"));
                menu.AddChild(new Menu("Prim Kural Tanımları", "/PRD/VWPRD_ProductBonus/Index"));
                menu.AddChild(new Menu("Satış Tanımlama", "/PRD/VWPRD_ProductProgressPaymentImport/Index"));
                menu.AddChild(new Menu("Satış Onaylama", "/PRD/VWPRD_ProductProgressPayment/Index"));
                menu.AddChild(new Menu("Hakediş Tanımları", "/PRD/VWPRD_ProductPayment/Index"));
            }
            menu.AddChild(new Menu("Ürün Değişimi", "/PRD/VWPRD_StockTaskPlan/Index"));
            menu.AddChild(new Menu("Stok Özetleri", "/PRD/VWPRD_StockSummary/Index"));
            menu.AddChild(new Menu("Stok Hareketleri", "/PRD/VWPRD_StockAction/Index"));
            menu.AddChild(new Menu("Ürün Fiyat Listeleri", "/PRD/VWPRD_CompanyBasedPrice/Index"));
            menu.AddChild(new Menu("Ürün Stok Raporu", "/PRD/VWPRD_Product/StockReport"));
            menu.AddChild(new Menu("Şirketlerim", "/CMP/VWCMP_Company/IndexCompany"));
            menu.AddChild(new Menu("Depo/Şube/Kısımlar", "/CMP/VWCMP_Storage/IndexMy"));

            menu.AddChild(new Menu("Cari Depo/Şube/Kısımlar", "/CMP/VWCMP_Storage/Index"));
            menu.AddChild(new Menu("Araç Listesi", "/CMP/VWCMP_CompanyCars/Index"));



            return menu;
        }

        private Menu GetProduction()
        {
            var menu = new Menu("Üretim Yönetimi", "#", "fa fa-qrcode");
            menu.AddChild(new Menu("Üretim Emirleri", "/PRD/VWPRD_Production/Index"));
            menu.AddChild(new Menu("Üretim Şemaları", "/PRD/VWPRD_ProductionSchema/Index"));
            return menu;
        }

        private Menu GetIsletmeCariler()
        {
            var menu = new Menu("Firma&Cari Yönetimi", "#", "fa fa-building");
            menu.AddChild(new Menu("Firma&Cari Listesi", "/CMP/VWCMP_Company/Index"));
            menu.AddChild(new Menu("Firma&Cari Şube/Depo/Kısımları", "/CMP/VWCMP_Storage/Index"));
            if (TenantConfig.Tenant.TenantCode == 1187 || TenantConfig.Tenant.TenantCode == 1100 || TenantConfig.Tenant.TenantCode == 1000)
            {
                menu.AddChild(new Menu("Tedarikçi Puanlama", "/CMP/VWCMP_Storage/SupplierScoring"));
            }
            menu.AddChild(new Menu("Şube/Depo/Kısım Haritası", "/CMP/VWCMP_Storage/Map"));
            menu.AddChild(new Menu("Müşteri Rehberi", "/SH/VWSH_User/ContactCustomerPersons"));
            if (TenantConfig.Tenant.TenantCode == 1194 || TenantConfig.Tenant.TenantCode == 1100 || TenantConfig.Tenant.TenantCode == 1000)
            {
                menu.AddChild(new Menu("Bayi Personelleri", "/SH/VWSH_User/CompanyPersonIndex"));
                menu.AddChild(new Menu("Bayi Onay Listesi", "/CMP/VWCMP_Company/CompanyApproveIndex"));
            }
            return menu;
        }

        private Menu GetMuhasabeYonetimi()
        {
            var muhasabeYonetim = new Menu("Nakit Yönetimi", "#", "fa fa-bank");
            muhasabeYonetim.AddChild(new Menu("Kasa ve Bankalar", "/PA/VWPA_Account"));
            //muhasabeYonetim.AddChild(new Menu("Gelir Listesi", "/PA/VWPA_Transaction/IndexIncome"));
            muhasabeYonetim.AddChild(new Menu("Gider Listesi", "/PA/VWPA_Transaction/IndexExpense"));
            muhasabeYonetim.AddChild(new Menu("Nakit Akış Raporu", "/PA/VWPA_Transaction/Report"));
            muhasabeYonetim.AddChild(new Menu("Kasa&Banka Raporu", "/PA/VWPA_Ledger"));
            return muhasabeYonetim;
        }

        private Menu GetYonetim()
        {
            var sistemYonetim = new Menu("Sistem Ayarları ve Loglar", "#", "fa fa-cogs");


            var rules = new Menu("Kural Tanımlama İşlemleri");
            rules.AddChild(new Menu("Kurallar", "/UT/VWUT_Rules/Index"));
            //rules.AddChild(new Menu("Kullanıcı Kural Aşamaları", "/UT/VWUT_RulesUserStage/Index"));
            sistemYonetim.AddChild(rules);
            sistemYonetim.AddChild(new Menu("Dil Tanımlamaları", "/SYS/Language/Index"));
            sistemYonetim.AddChild(new Menu("Ürün Kategorileri", "/PRD/VWPRD_Category"));
            sistemYonetim.AddChild(new Menu("Pdks Cihaz Tanımlamaları", "/SH/VWSH_ShiftTrackingDevice"));
            sistemYonetim.AddChild(new Menu("Birim Tanımları", "/UT/UT_Unit/Index"));
            sistemYonetim.AddChild(new Menu("Para Birimleri", "/UT/UT_Currency/Index"));
            sistemYonetim.AddChild(new Menu("Lokasyonlar (Ülke/İl/İlçe)", "/UT/VWUT_Location/Index"));
            sistemYonetim.AddChild(new Menu("Firma&Cari Tip Tanımları", "/CMP/CMP_Types/Index"));
            sistemYonetim.AddChild(new Menu("Görev Konusu Tanımları", "/FTM/FTM_TaskSubject/Index"));
            sistemYonetim.AddChild(new Menu("Görev Yetki Tanımları", "/FTM/VWFTM_TaskAuthority/Index"));
            sistemYonetim.AddChild(new Menu("Sektör Tanımları", "/UT/VWUT_Sector/Index"));
            sistemYonetim.AddChild(new Menu("Banka Tanımları", "/UT/VWUT_Bank/Index"));
            sistemYonetim.AddChild(new Menu("Şablonlar", "/UT/VWUT_Template/Index"));
            sistemYonetim.AddChild(new Menu("Satış Teklifi Şablonları", "/CMP/CMP_InvoiceDocumentTemplate/Index"));
            sistemYonetim.AddChild(new Menu("Grup / Ekip Tanımlamaları", "/SH/SH_Group/Index"));
            sistemYonetim.AddChild(new Menu("Rol Tanımları", "/SH/VWSH_Role"));
            sistemYonetim.AddChild(new Menu("Email Logları", "/SYS/VWSYS_Email"));
            sistemYonetim.AddChild(new Menu("Bildirim Logları", "/SYS/VWSYS_Notification"));
            sistemYonetim.AddChild(new Menu("Dış Bağlantı Yönetimi", "/SYS/VWSYS_ExternalLinks/Index"));
            sistemYonetim.AddChild(new Menu("Kullanıcı Oturum Raporu", "/SH/SH_Ticket/Dashboard"));
            sistemYonetim.AddChild(new Menu("Harita Yapılandırması", "/UT/VWUT_LocationConfig/Index"));
            sistemYonetim.AddChild(new Menu("Versiyon Yönetimi", "/SYS/VWSYS_Version/Index"));

            return sistemYonetim;
        }

        public string GetHtml()
        {
            return Recursive(_root, 0);
        }

        public string Recursive(List<Menu> menuList, int generation)
        {
            var str = new StringBuilder();
            var subGeneration = generation + 1;

            foreach (var menu in menuList.Where(a => a.visible))
            {
                if (menu.name == null)
                {
                    continue;
                }

                var placement = subGeneration == 1 ? "top" : "right";

                str.AppendLine("<li>");
                str.AppendLine("<a" + (menu.newBlank ? " target='_blank'" : "") + " href=\"" + menu.url + "\" >");
                if (!string.IsNullOrEmpty(menu.icon))
                {
                    str.AppendLine("<i data-toggle=\"tooltip\" title=\"" + menu.name + "\" data-placement=\"" + placement + "\" class=\"" + menu.icon + "\"></i>");
                }

                if (generation == 0)
                {
                    str.AppendLine("<span class=\"nav-label\"> " + menu.name + "</span>");
                }
                else
                {
                    str.AppendLine(menu.name);
                }
                if (menu.childs.Count() > 0)
                {
                    str.AppendLine("<span class=\"fa arrow\"></span>");
                }
                str.AppendLine("</a>");

                if (menu.childs.Count() > 0)
                {

                    var cls = subGeneration == 2 ? "nav-third-level" : "nav-second-level";

                    str.AppendLine("<ul class=\"nav " + cls + " collapse\">");
                    str.AppendLine(Recursive(menu.childs, subGeneration));
                    str.AppendLine("</ul>");
                }

                str.AppendLine("</li>");
            }
            return str.ToString();
        }
    }



}