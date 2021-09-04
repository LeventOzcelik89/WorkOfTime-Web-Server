using Infoline.WorkOfTime.BusinessAccess;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public class NavModelPYS
    {
        private List<Menu> _root = new List<Menu>();

        public NavModelPYS()
        {
            _root.Add(GetHomePage());
            _root.Add(GetOfisYonetimi());
            _root.Add(GetIK());
            _root.Add(GetSatisSiparis());
            _root.Add(GetSatinAlma());
            _root.Add(GetStokSevkiyat());
            _root.Add(GetProje());
            _root.Add(GetGorevYonetimi());
            _root.Add(GetCustomerInventories());
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

        private Menu GetOfisYonetimi()
        {
            var menu = new Menu("Ofis Yönetimi", "#", "fa fa-th-large");
            menu.AddChild(new Menu("Benim Takvimim", "/INV/VWINV_CompanyPersonCalendar/MyCalendar"));
            menu.AddChild(new Menu("Benim Bilgilerim", "/Account/Profile"));
            menu.AddChild(new Menu("Şirket Takvimi", "/INV/VWINV_CompanyPersonCalendar/Calendar"));
            menu.AddChild(new Menu("Şirket Rehberi", "/SH/VWSH_User/ContactOffice"));
            menu.AddChild(new Menu("Şirket Yönetmelikleri", "/CMP/VWCMP_Company/PersonnelRegulationDocument"));
            menu.AddChild(new Menu("Şirket Organizasyon", "/INV/VWINV_CompanyDepartments/Preview"));
            menu.AddChild(new Menu("Şirket Portalı", "#"));
            menu.AddChild(new Menu("Şirket Bilgileri", "/CMP/VWCMP_Company/IndexMy"));
            menu.AddChild(new Menu("Şirket Şube ve Depoları", "/CMP/VWCMP_Storage/IndexMy"));

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

            return ik;
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
            var projeyonetimi = new Menu("Proje Yönetimi", "#", "icon-briefcase");
            projeyonetimi.AddChild(new Menu("Görev Aldığım Projeler", "/PRJ/VWPRJ_Project/MyIndex"));
            projeyonetimi.AddChild(new Menu("Tüm Projeler", "/PRJ/VWPRJ_Project"));
            projeyonetimi.AddChild(new Menu("Proje Raporları", "/PRJ/VWPRJ_Project/Dashboard"));
            return projeyonetimi;
        }

        private Menu GetGorevYonetimi()
        {
            var sahayonetimi = new Menu("Görev Yönetimi", "#", "fa fa-wrench");
            sahayonetimi.AddChild(new Menu("Görev Listesi", "/FTM/VWFTM_Task/Index"));
            sahayonetimi.AddChild(new Menu("Görev Haritası", "/FTM/VWFTM_Task/Map"));
            //sahayonetimi.AddChild(new Menu("Görev Raporları", "/FTM/VWFTM_Task/Dashboard"));
            sahayonetimi.AddChild(new Menu("Aktivite İzleme", "/FTM/VWFTM_Task/ActivityTracking"));
            sahayonetimi.AddChild(new Menu("Görev Form Tanımları", "/FTM/VWFTM_TaskForm"));
            sahayonetimi.AddChild(new Menu("Bakım Envanterleri", "/PRD/VWPRD_Inventory/IndexMaintance"));
            sahayonetimi.AddChild(new Menu("Bakım Periyotları", "/PRD/VWPRD_InventoryTask/Index"));

            return sahayonetimi;
        }

        private Menu GetCustomerInventories()
        {
            var menu = new Menu("Arıza/Bakım Yönetimi", "#", "fa fa-wrench");
            menu.AddChild(new Menu("Envanter Listesi", "/PRD/VWPRD_Inventory/IndexCustomer"));
            menu.AddChild(new Menu("Geçmiş Arıza Bakımlar", "/FTM/VWFTM_Task/IndexCustomer"));
            return menu;
        }



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
            var menu = new Menu("Ürün&Stok Yönetimi", "#", "fa fa-cubes");
            menu.AddChild(new Menu("Ürün&Hizmet Listesi", "/PRD/VWPRD_Product/Index"));
            menu.AddChild(new Menu("Envanter Listesi", "/PRD/VWPRD_Inventory/Index"));
            menu.AddChild(new Menu("Stok ve Envanter İşlemleri", "/PRD/VWPRD_Transaction/Index"));
            menu.AddChild(new Menu("Stok Özetleri", "/PRD/VWPRD_StockSummary/Index"));
            menu.AddChild(new Menu("Stok Hareketleri", "/PRD/VWPRD_StockAction/Index"));
            return menu;
        }


        private Menu GetIsletmeCariler()
        {
            var menu = new Menu("Firma&Cari Yönetimi", "#", "fa fa-building");
            menu.AddChild(new Menu("Firma&Cari Listesi", "/CMP/VWCMP_Company/Index"));
            menu.AddChild(new Menu("Firma&Cari Şube ve Depoları", "/CMP/VWCMP_Storage/Index"));
            menu.AddChild(new Menu("Şube ve Depo Haritası", "/CMP/VWCMP_Storage/Map"));
            menu.AddChild(new Menu("Rehber", "/SH/VWSH_User/ContactCustomerPersons"));
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
            sistemYonetim.AddChild(new Menu("Ürün Kategorileri", "/PRD/VWPRD_Category"));
            sistemYonetim.AddChild(new Menu("Birim Tanımları", "/UT/UT_Unit/Index"));
            sistemYonetim.AddChild(new Menu("Para Birimleri", "/UT/UT_Currency/Index"));
            sistemYonetim.AddChild(new Menu("Lokasyonlar (Ülke/İl/İlçe)", "/UT/VWUT_Location/Index"));
            sistemYonetim.AddChild(new Menu("Sektör Tanımları", "/UT/VWUT_Sector/Index"));
            sistemYonetim.AddChild(new Menu("Banka Tanımları", "/UT/VWUT_Bank/Index"));
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