using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Web.Mvc
{
    public class NavModelIICS
    {
        private List<Menu> _root = new List<Menu>();

        public NavModelIICS()
        {
            _root.Add(GetHomePage());
            _root.Add(GetOfisYonetimi());
            _root.Add(GetIK());
            _root.Add(GetGorevlendirme());
            _root.Add(GetMasraf());
            _root.Add(GetAvans());
            _root.Add(GetDokumanYonetimi());
            _root.Add(GetSatinAlma());
            _root.Add(GetStokSevkiyat());
            _root.Add(GetProje());
            _root.Add(GetIsletmeCariler());
            _root.Add(GetMuhasabeYonetimi());
            _root.Add(GetYardimDestekYonetimi());
            _root.Add(GetYonetim());
            _root.Add(GetExternalLinks());
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

        private Menu GetDokumanYonetimi()
        {
            var menu = new Menu("Doküman Yönetimi", "#", "fa fa-file");
            menu.AddChild(new Menu("Dokümanlar", "/DOC/VWDOC_Document/Index"));
            menu.AddChild(new Menu("Tüm Revizyon Talepleri", "/DOC/VWDOC_DocumentRevisionRequest/Index"));
            menu.AddChild(new Menu("Revizyon Taleplerim", "/DOC/VWDOC_DocumentRevisionRequest/MyIndex"));
            menu.AddChild(new Menu("Revizyon Talepleri (Onay)", "/DOC/VWDOC_DocumentRevisionRequest/MyAboutIndex"));
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

        private Menu GetAdayOgrenciYonetimi()
        {
            var menu = new Menu("Aday Öğrenci İşlemleri", "#", "icon-graduation-cap");
            menu.AddChild(new Menu("Öğrenciler", "/CSM/VWCSM_Student"));
            menu.AddChild(new Menu("Öğrenci Görüşmeleri", "/CSM/VWCSM_Activity"));
            menu.AddChild(new Menu("Öğrenci Okulları", "/CSM/VWCSM_School"));
            menu.AddChild(new Menu("Öğrenci Bölümleri", "/CSM/VWCSM_Department"));
            menu.AddChild(new Menu("Süreç Aşamaları", "/CSM/VWCSM_Stage"));
            return menu;
        }

        private Menu GetHomePage()
        {
            var mypage = new Menu("Ana Sayfa", "/Account/Index", "fa fa-home");
            return mypage;
        }

        private Menu GetOfisYonetimi()
        {
            var menu = new Menu("Ofis Yönetimi", "#", "fa fa-th-large");
            menu.AddChild(new Menu("Benim Bilgilerim", "/Account/Profile"));
            menu.AddChild(new Menu("Şirket Rehberi", "/SH/VWSH_User/ContactOffice"));
            menu.AddChild(new Menu("Şirket Organizasyon", "/INV/VWINV_CompanyDepartments/Preview"));
            menu.AddChild(new Menu("Şirket Bilgileri", "/CMP/VWCMP_Company/IndexMy"));
            menu.AddChild(new Menu("Araç Kilometre Bilgilerim", "/CMP/VWCMP_CompanyCarKilometer/Index"));
            return menu;
        }

        private Menu GetIK()
        {
            var ik = new Menu("İnsan Kaynakları", "#", "fa fa-users");
            var personel = new Menu("Personel İşlemleri");
            personel.AddChild(new Menu("Personel Listesi", "/SH/VWSH_User/Index"));
            personel.AddChild(new Menu("Personel Raporları", "/INV/INV_CompanyPerson/Dashboard"));
            personel.AddChild(new Menu("Organizasyon Şeması Yönetimi", "/INV/VWINV_CompanyDepartments"));
            ik.AddChild(personel);

            var izinislemleri = new Menu("İzin İşlemleri");
            izinislemleri.AddChild(new Menu("Tüm İzinler", "/INV/VWINV_Permit/Index"));
            izinislemleri.AddChild(new Menu("İzin Raporları", "/INV/VWINV_Permit/Dashboard"));
            izinislemleri.AddChild(new Menu("İzin Taleplerim", "/INV/VWINV_Permit/MyIndex"));
            izinislemleri.AddChild(new Menu("İzin Talepleri (Onay)", "/INV/VWINV_Permit/MyAboutIndex"));
            izinislemleri.AddChild(new Menu("İzin Tipi Tanımları", "/INV/INV_PermitType/Index"));
            izinislemleri.AddChild(new Menu("Resmi İzin Tanımları", "/INV/VWINV_PermitOffical/Index"));
            ik.AddChild(izinislemleri);

            var maas = new Menu("Maaş İşlemleri");
            maas.AddChild(new Menu("Tüm Maaşlar", "/INV/VWINV_CompanyPersonSalary/Index"));
            maas.AddChild(new Menu("Maaş Raporları", "#"));
            maas.AddChild(new Menu("Maaş Geçmişim", "#"));
            ik.AddChild(maas);

            var harcirah = new Menu("Harcırah İşlemleri");
            harcirah.AddChild(new Menu("Tüm Harcırahlar"));
            harcirah.AddChild(new Menu("Harcırah Talepleri"));
            harcirah.AddChild(new Menu("Harcırah Taleplerim"));
            ik.AddChild(harcirah);

            // kalkcak
            var degerlendirme = new Menu("Performans 360");
            degerlendirme.AddChild(new Menu("İşe Giren Personel Değerlendirmeleri", "/INV/VWINV_CompanyPersonAssessment"));
            degerlendirme.AddChild(new Menu("Değerlendirmelerim", "/PDS/VWPDS_FormPermitTaskUser"));
            degerlendirme.AddChild(new Menu("Tüm Değerlendirmeler", "/PDS/VWPDS_FormResult/AllIndex"));
            degerlendirme.AddChild(new Menu("Değerlendirme Formları", "/PDS/VWPDS_EvaluateForm"));
            ik.AddChild(degerlendirme);

            var cvhavuzu = new Menu("Personel Adayı İşlemleri");
            cvhavuzu.AddChild(new Menu("CV Havuzu", "/HR/VWHR_Person"));
            cvhavuzu.AddChild(new Menu("Personel Talep İşlemleri", "/HR/VWHR_StaffNeeds/MyIndex"));
            cvhavuzu.AddChild(new Menu("Mülakatlar", "/HR/VWHR_PlanPerson/MyIndex"));
            ik.AddChild(cvhavuzu);
            return ik;
        }

        //Kullanmaya başladıklarında açılcak
        private Menu GetGorevlendirme()
        {
            var gorevlendirme = new Menu("Görevlendirme İşlemleri", "#", "fa fa-car");
            gorevlendirme.AddChild(new Menu("Tüm Görevlendirmeler", "/INV/VWINV_Commissions/Index"));
            gorevlendirme.AddChild(new Menu("Görevlendirme Raporları", "/INV/VWINV_Commissions/Dashboard"));
            gorevlendirme.AddChild(new Menu("Görevlendirme Taleplerim", "/INV/VWINV_Commissions/MyIndex"));
            gorevlendirme.AddChild(new Menu("Görevlendirme Talepleri (Onay)", "/INV/VWINV_Commissions/MyAboutIndex"));
            return gorevlendirme;
        }


        private Menu GetMasraf()
        {
            var masraf = new Menu("Masraf İşlemleri", "#", "fa fa-money");
            masraf.AddChild(new Menu("Tüm Masraf Talepleri", "/PA/VWPA_Transaction/IndexAllRequest"));
            masraf.AddChild(new Menu("Masraf Taleplerim", "/PA/VWPA_Transaction/IndexMyRequest"));
            masraf.AddChild(new Menu("Masraf Talepleri (Onay)", "/PA/VWPA_Transaction/IndexRequest"));
            return masraf;
        }

        private Menu GetAvans()
        {
            var avans = new Menu("Avans İşlemleri", "#", "icon-credit-card-1");
            avans.AddChild(new Menu("Tüm Avanslar", "/PA/VWPA_Advance/IndexAllRequest"));
            avans.AddChild(new Menu("Avans Taleplerim", "/PA/VWPA_Advance/IndexMyRequest"));
            avans.AddChild(new Menu("Avans Talepleri (Onay)", "/PA/VWPA_Advance/IndexRequest"));
            return avans;
        }

        private Menu GetCRM()
        {
            var crm = new Menu("Müşteri İlişkileri Yönetimi", "#", "fa fa-dollar");

            crm.AddChild(new Menu("Potansiyel Fırsatlar", "/CRM/VWCRM_Presentation/Index"));
            crm.AddChild(new Menu("Aktivite ve Randevular", "/CRM/VWCRM_Contact/Index"));
            crm.AddChild(new Menu("Satış ve Müşteri Raporu", "/CRM/VWCRM_Presentation/SalesReport"));


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

        private Menu GetProje()
        {
            var projeyonetimi = new Menu("Toplantı Yönetimi", "#", "icon-briefcase");
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
            sahayonetimi.AddChild(new Menu("Bakım Yönetimi", "/FTM/VWFTM_TaskPlan/Index"));
            sahayonetimi.AddChild(new Menu("Personel İş Takip", "/FTM/VWFTM_Task/PersonDailyReport"));
            sahayonetimi.AddChild(new Menu("Aktivite İzleme", "/FTM/VWFTM_Task/ActivityTracking"));
            sahayonetimi.AddChild(new Menu("Görev Haritası", "/FTM/VWFTM_Task/Map"));
            sahayonetimi.AddChild(new Menu("Detaylı Rapor", "/FTM/VWFTM_TaskGrid/Index"));
            sahayonetimi.AddChild(new Menu("Personel Raporu", "/FTM/VWFTM_Task/StaffReport"));
            sahayonetimi.AddChild(new Menu("Görev Şablonları", "/FTM/VWFTM_TaskTemplate/Index"));
            sahayonetimi.AddChild(new Menu("Bakım Envanterleri", "/PRD/VWPRD_Inventory/IndexMaintance"));
            sahayonetimi.AddChild(new Menu("Bakım Periyotları", "/PRD/VWPRD_InventoryTask/Index"));
            sahayonetimi.AddChild(new Menu("Görev Formları", "/FTM/VWFTM_TaskForm"));
            sahayonetimi.AddChild(new Menu("Görev Takvimi", "/FTM/VWFTM_TaskPlan/Calendar"));

            return sahayonetimi;
        }

        //private Menu GetCustomerInventories()
        //{
        //	var menu = new Menu("Arıza/Bakım Yönetimi", "#", "fa fa-wrench");
        //	menu.AddChild(new Menu("Envanter Listesi", "/PRD/VWPRD_Inventory/IndexCustomer"));
        //	menu.AddChild(new Menu("Geçmiş Arıza Bakımlar", "/FTM/VWFTM_Task/IndexCustomer"));
        //	return menu;
        //}

        private Menu GetSatinAlma()
        {
            var satinalma = new Menu("Satınalma Yönetimi", "#", "icon-basket-4");
            satinalma.AddChild(new Menu("Satın Alma Talepleri", "/CMP/VWCMP_Request/Index"));
            satinalma.AddChild(new Menu("Satın Alma Teklifleri", "/CMP/VWCMP_Tender/IndexBuying"));
            satinalma.AddChild(new Menu("Alış Faturaları", "/CMP/VWCMP_Invoice/IndexBuying"));
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
            var menu = new Menu("Envanter Yönetimi", "#", "fa fa-cubes");
            menu.AddChild(new Menu("Ürün&Envanter Listesi", "/PRD/VWPRD_Product/Index"));
            menu.AddChild(new Menu("Stok ve Envanter İşlemleri", "/PRD/VWPRD_Transaction/Index"));
            menu.AddChild(new Menu("Envanter Listesi", "/PRD/VWPRD_Inventory/Index"));
            menu.AddChild(new Menu("Şirket Şube/Depo/Kısımları", "/CMP/VWCMP_Storage/IndexMy"));
            menu.AddChild(new Menu("Firma&Cari Şube/Depo/Kısımları", "/CMP/VWCMP_Storage/Index"));
            // kalkcak araç listesi
            menu.AddChild(new Menu("Araç Listesi", "/CMP/VWCMP_CompanyCars/Index"));
            menu.AddChild(new Menu("Stok Özetleri", "/PRD/VWPRD_StockSummary/Index"));
            menu.AddChild(new Menu("Stok Hareketleri", "/PRD/VWPRD_StockAction/Index"));
            return menu;
        }

        private Menu GetIsletmeCariler()
        {
            var menu = new Menu("Firma", "#", "fa fa-building");
            menu.AddChild(new Menu("Firma Listesi", "/CMP/VWCMP_Company/Index"));
            menu.AddChild(new Menu("Firma Haritası", "/CMP/VWCMP_Storage/Map"));
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


            // kalkcak
            var rules = new Menu("Kural Tanımlama İşlemleri");
            rules.AddChild(new Menu("Kurallar", "/UT/VWUT_Rules/Index"));
            //rules.AddChild(new Menu("Kullanıcı Kural Aşamaları", "/UT/VWUT_RulesUserStage/Index"));
            sistemYonetim.AddChild(rules);
            sistemYonetim.AddChild(new Menu("Dil Tanımlamaları", "/SYS/Language/Index"));
            sistemYonetim.AddChild(new Menu("Ürün Kategorileri", "/PRD/VWPRD_Category"));
            sistemYonetim.AddChild(new Menu("Birim Tanımları", "/UT/UT_Unit/Index"));
            sistemYonetim.AddChild(new Menu("Para Birimleri", "/UT/UT_Currency/Index"));
            sistemYonetim.AddChild(new Menu("Lokasyonlar (Ülke/İl/İlçe)", "/UT/VWUT_Location/Index"));
            sistemYonetim.AddChild(new Menu("Firma&Cari Tip Tanımları", "/CMP/CMP_Types/Index"));
            sistemYonetim.AddChild(new Menu("Görev Konusu Tanımları", "/FTM/FTM_TaskSubject/Index"));
            sistemYonetim.AddChild(new Menu("Sektör Tanımları", "/UT/VWUT_Sector/Index"));
            sistemYonetim.AddChild(new Menu("Banka Tanımları", "/UT/VWUT_Bank/Index"));
            sistemYonetim.AddChild(new Menu("Satış Teklifi Şablonları", "/CMP/CMP_InvoiceDocumentTemplate/Index"));
            sistemYonetim.AddChild(new Menu("Grup / Ekip Tanımlamaları", "/SH/SH_Group/Index"));
            sistemYonetim.AddChild(new Menu("Rol Tanımları", "/SH/VWSH_Role"));
            sistemYonetim.AddChild(new Menu("Email Logları", "/SYS/VWSYS_Email"));
            sistemYonetim.AddChild(new Menu("Dış Bağlantı Yönetimi", "/SYS/VWSYS_ExternalLinks/Index"));
            sistemYonetim.AddChild(new Menu("Kullanıcı Oturum Raporu", "/SH/SH_Ticket/Dashboard"));
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