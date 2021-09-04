using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class Items
    {
        public string _mainTitle { get; set; }
        public string _content { get; set; }


        public Items(string mainTitle, string content)
        {
            _mainTitle = mainTitle;
            _content = content;
        }
    }


    public class PerDayMailService
    {
        public List<Items> Items { get; set; } = new List<Models.Items>();
        public List<VWPRJ_Project> PRJ_ApproachLastSevenDayProject { get; set; }
        public List<VWPRJ_Project> PRJ_TodayEndProjects { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_ApproachLastSevenDayStartProjectKMs { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_TodayStartProjectKMs { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_ApproachLastSevenDayEndProjectKMs { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_TodayEndProjectKMs { get; set; }
        public List<VWINV_CompanyCalendar> INV_CompanyMeetings { get; set; }
        public List<VWSH_User> SH_PersonsBirhtday { get; set; }
        public List<VWSH_User> SH_PersonsJobStart { get; set; }
        public List<VWINV_CompanyPersonCalendar> INV_CompanyNotifications { get; set; }
        public List<VWINV_CompanyPersonCalendar> INV_CompanyPresentations { get; set; }
        public List<VWINV_Permit> INV_TodayPermitAcceptPersons { get; set; }
        //public VWINV_CommissionsPersons[] INV_TodayCommissionPersons { get; set; }
        public List<VWINV_Permit> INV_TodayPermitWaitingtPersons { get; set; }
        public List<VWINV_CommissionsPersons> INV_CommissionAcceptPersons { get; set; }
        public List<VWFTM_Task> FTM_OpenTask { get; set; }
        public List<VWFTM_Task> FTM_AcceptWaitingTask { get; set; }
        public List<VWCRM_Contact> CRM_SalesMeetings { get; set; }
        public PerDayMailService(WorkOfTimeDatabase db, string _url)
        {
            #region Proje Yönetimi
            PRJ_ApproachLastSevenDayProject = db.GetVWPRJ_ProjectApproachByLastSevenDay(0).ToList();
            PRJ_TodayEndProjects = db.GetVWPRJ_ProjectApproachByEnd(0).ToList();
            PRJ_ApproachLastSevenDayStartProjectKMs = db.GetVWPRJ_ProjectTimeLineApproachStartByLastSevenDay(0).ToList();
            PRJ_TodayStartProjectKMs = db.GetVWPRJ_ProjectTimeLineByTodayStart(0).ToList();
            PRJ_ApproachLastSevenDayEndProjectKMs = db.GetVWPRJ_ProjectTimeLineApproachEndByLastSevenDay(0).ToList();
            PRJ_TodayEndProjectKMs = db.GetVWPRJ_ProjectTimeLineByTodayEnd(0).ToList();
            #endregion

            #region Bugünkü Etkinlikler
            INV_CompanyMeetings = db.GetINV_CompanyMeetingByNamesToday().ToList();
            SH_PersonsBirhtday = db.GetPersonsBirthdayByToday().ToList();
            SH_PersonsJobStart = db.GetPersonsJobStartByToday().ToList();
            INV_CompanyNotifications = db.GetVWINV_CompanyNotificationsByToday().ToList();
            INV_CompanyPresentations = db.GetVWINV_CompanyPresentationByToday().ToList();
            #endregion



            #region İnsan Kaynakları
            INV_TodayPermitAcceptPersons = db.GetVWINV_PermitPersonToday(0).ToList();

            var TodayCommissions = db.GetVWINV_CommissionsPersonByToday().ToArray();

            var INV_TodayCommissionPersons = db.GetVWINV_CommissionsPersonsByIds(TodayCommissions.Select(x => x.id).ToArray()).GroupBy(x => x.id).Select(x => new
            {
                Names = String.Join(",", x.Select(d => d.Person_Title)),
                CommissionsType = x.Select(d => d.Information_Title).FirstOrDefault(),
                StartDate = x.Select(d => d.StartDate).FirstOrDefault(),
                EndDate = x.Select(d => d.EndDate).FirstOrDefault()
            }).ToArray();
            #endregion

            #region Saha Görev Yönetimi
            FTM_OpenTask = db.GetVWFTM_OpenTask().ToList();
            FTM_AcceptWaitingTask = db.GetVWFTM_WaitingAcceptTask().ToList();
            #endregion

            #region Satışlar
            CRM_SalesMeetings = db.GetVWCRM_Contact().ToList();
            var todayContacts = CRM_SalesMeetings.Where(a => a.ContactStartDate.Value.Date == DateTime.Now.Date);
            var CRM_ContactSalesUsers = db.GetVWCRM_ContactSalesUsersByIds(todayContacts.Select(x => x.id).ToArray()).GroupBy(x => x.id).Select(x => new
            {
                Names = String.Join(",", x.Select(d => d.User_Title)),
                StartDate = x.Select(d => d.ContactStartDate).FirstOrDefault(),
                EndDate = x.Select(d => d.ContactEndDate).FirstOrDefault(),
                CompanyName = x.Select(d => d.CustomerCompanyId_Title).FirstOrDefault()
            }).ToArray();
            #endregion

            if (INV_TodayPermitAcceptPersons.Count() > 0)
            {
                Items.Add(new Models.Items(
                   "İnsan Kaynakları",
                   "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/INV/VWINV_Permit/Index>Bugün izni olan personel sayısı</a>" + "*" + INV_TodayPermitAcceptPersons.Count()
                   ));
            }

            if (PRJ_ApproachLastSevenDayProject.Count() > 0)
            {
                Items.Add(new Models.Items(
                    "Proje Yönetimi",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bitiş tarihi yaklaşan proje adedi</a>" + "*" + PRJ_ApproachLastSevenDayProject.Count()
                    ));
            }

            if (PRJ_TodayEndProjects.Count() > 0)
            {
                Items.Add(new Models.Items(
                  "Proje Yönetimi",
                  "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bugün süresi dolan proje sayısı</a>" + "*" + PRJ_TodayEndProjects.Count()
                  ));
            }

            if (PRJ_ApproachLastSevenDayStartProjectKMs.Count() > 0)
            {
                Items.Add(new Models.Items(
                  "Proje Yönetimi",
                  "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Başlangıç tarihi yaklaşan proje kilometre taşı bulunmaktadır.</a>" + "*" + PRJ_ApproachLastSevenDayStartProjectKMs.Count()
                  ));
            }

            if (PRJ_TodayStartProjectKMs.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Proje Yönetimi",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bugün başlayan proje kilometre taşı mevcut</a>" + "*" + PRJ_TodayStartProjectKMs.Count()
                 ));
            }

            if (PRJ_ApproachLastSevenDayEndProjectKMs.Count() > 0)
            {
                Items.Add(new Models.Items(
                "Proje Yönetimi",
                "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bitiş tarihi yaklaşan proje kilometre taşı mevcut</a>" + "*" + PRJ_ApproachLastSevenDayEndProjectKMs.Count()
                ));
            }

            if (PRJ_TodayEndProjectKMs.Count() > 0)
            {
                Items.Add(new Models.Items(
                   "Proje Yönetimi",
                   "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bugün süresi dolan proje kilometre taşı sayısı</a>" + "*" + PRJ_TodayEndProjectKMs.Count()
                   ));
            }

            if (INV_CompanyMeetings.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Bugünkü Etkinlikler",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index>Bugün şirket içinde bulunan toplantı adedi</a>" + "*" + INV_CompanyMeetings.Count()
                 ));
            }

            if (SH_PersonsBirhtday.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Bugünkü Etkinlikler",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index>Bugün doğum günü olan personeller</a>&nbsp<span style='font-size:11px;font-weight:bold;color: #1ab394;'>(" +  string.Join(", ", SH_PersonsBirhtday.Select(x => x.FullName).ToArray()) + ")</span>" + "*" + SH_PersonsBirhtday.Count()
                 ));
            }

            if (SH_PersonsJobStart.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Bugünkü Etkinlikler",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index>Bugün iş giriş yıl dönümü kutlayan personel sayısı</a>" + "*" + SH_PersonsJobStart.Count()
                 ));
            }

            if (INV_CompanyNotifications.Count() > 0)
            {
                Items.Add(new Models.Items(
                "Bugünkü Etkinlikler",
                "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index> duyuru mevcut</a>" + "*" + INV_CompanyNotifications.Count()
                ));
            }

            if (INV_CompanyPresentations.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Bugünkü Etkinlikler",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index> sunum sayısı</a>" + "*" + INV_CompanyPresentations.Count()
                 ));
            }

            if (INV_TodayCommissionPersons.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "İnsan Kaynakları",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/INV/VWINV_Commissions/Index>Bugün aktif olan görevlendirme sayısı" + "*" + INV_TodayCommissionPersons.Count()
                 ));
            }

            if (FTM_OpenTask.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Saha Görev Yönetimi",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/FTM/VWFTM_Task/Index> Açık görev/devam eden görev sayısı</a>" + "*" + FTM_OpenTask.Count()
                 ));
            }


            if (FTM_AcceptWaitingTask.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Saha Görev Yönetimi",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/FTM/VWFTM_Task/Index>Onay bekleyen görev sayısı</a>" + "*" + FTM_AcceptWaitingTask.Count()
                 ));
            }

            if (CRM_ContactSalesUsers.Count() > 0)
            {
                Items.Add(new Models.Items(
                 "Bugünkü Etkinlikler",
                 "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/CRM/VWCRM_Presentation/Index>Bugün yapılacak satış toplantısı adedi</a>" + "*" + CRM_ContactSalesUsers.Count()
                 ));
            }
        }

        public string GetHTML()
        {
            string htmlText = "";
            string[] colorArray = new string[] { "#1ab394", "#1c84c6", "#23c6c8", "#f8ac59", "#ed5565", "#1ab394", "#1c84c6", "#23c6c8", "#f8ac59", "#ed5565" };
            int count = 0;
            foreach (var mainTitles in Items.GroupBy(a => a._mainTitle))
            {
                var cm = mainTitles.SelectMany(a => a._content).Count();
                if (cm != 0)
                {
                    htmlText += "<table cellspacing='0' cellpadding='0' style='width:100%;border:1px solid " + colorArray[count] + ";border-spacing: 0;'>"
                            + "<tr style='background-color:" + colorArray[count] + ";height:35px'>"
                            + "<td colspan=2 style='vertical-align:middle;padding-left:10px'><img style='width: 100%;max-width: 20px' src='" + BaseUrl(mainTitles.Key)  + " '>&nbsp;&nbsp;<span style='color:white;font-weight:bold;'>" + mainTitles.Key + "</span></td>"
                            + "</tr>";


                    foreach (var content in mainTitles.GroupBy(a => a._content))
                    {
                        htmlText += 
                        "<tr style='line-height: 34px;background-color:#fff;'>"
                        + "<td style='padding-left:10px;border-bottom: 1px solid " + colorArray[count] + ";'>"
                        + "" + content.Key.Split('*')[0] +  ""
                        + "</td>"
                        + "<td style='width:30px;text-align:center;border-bottom: 1px solid " + colorArray[count] + ";'>"
                        + "<div style='background-color:" + colorArray[count] + ";width:20px;height: 25px; margin:6px 6px 10px 6px;'>"
                        + "<span style='color:#fff;font-family:Tahoma;font-weight:bold;font-size:13px;vertical-align:middle;'>"  + content.Key.Split('*')[1] + "</span><br></br>"
                        + "</div></td></tr>";
                    }
                    htmlText += "</table><br>";
                }
                count++;
            }
            return htmlText;
        }

        public string BaseUrl(string mainTitles)
        {
            int type = 0;
            var icons = mainTitles.IndexOf("İnsan Kaynakları") > -1 ? type = 0 :
                        mainTitles.IndexOf("Proje Yönetimi") > -1 ? type = 1 :
                        mainTitles.IndexOf("Bugünkü Etkinlikler") > -1 ? type = 2 :
                        mainTitles.IndexOf("Satın Almalar") > -1 ? type = 3 :
                        mainTitles.IndexOf("Saha Görev Yönetimi") > -1 ? type = 4 : 
                        0;

            var url = "";
            switch (type)
            {
                case 0: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFQSURBVDiNrZG7SpxRFIW/M1GwMiJExMvYpLHW5AEMKKabNEE0D6C1DESwFcRHsAu5dNZpUlgrouVMTJNinEZGMsXgjc/C/ctk/JURXHDg7L32WqyzDzwn1D51VC2oZXU3zqqagut7SLyiXqpV9ZP3sRjcpbqcZ1CPwW31R47B9+BUa5muEOIEvIpeE+jPCfkyOICh0NwapJQEKkGOAX9yDI6B8bhXQvPfE96rLfWf+la9aIt/rr5RmzEzn+l6QjwR7h+BE2AfmAU+x9xG9GaAEaCoFlNKf5O6AHwBXgBloAqUgNfA1zBYAn4DO8AksAlcA0uoBxHzm7qes/1OrMWs6h7qWRQltdGFwan6Ie6NApBtswkM5Gy/E4PAWVYUgGlgC2h1Ic7QCs3UXUcdUI+6eMKhepc0tduqvcA7YI7b7xoOqg7UgJ/Ar5TS1RPSPo4bJ0WEQwRMgMcAAAAASUVORK5CYII="; break;
                case 1: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAGySURBVDiNhdJNiI9RFAbw544x+VhITDKaZGZIFpNImfK1YCVJFiOZSVLKzsJGSVazMCywUGwsRDRSYycWbNCshJrGQslCvmWhGD+b+8+bxTib+z73POe5533OSWqgYCQzBJZiXfOuDcuxqpQiyfwGeQtOohcXsCDJUJLRmm9rEedgBEPowxI88Dcu4Am+YRrfcQ6T6Gq214X2Sr5dz0EcwjPcxQCOVaHzrcKdOIPNGMYrnMAv7MfX2kl347H7GEd/sBKza+JGLf6A37WTPfV7a+V0YArvMdZeSplqmLooyfd6Jsl0kvVJSpI7uJlkQ5LeJJ2llI9tVfU49iV5V5OTVWB1ktdJhpNcS3IoycMkH5LsaBp4HkewHV9wFD/rv0/gBw7gFnZhDea2io/gaUPsSnV9GI8a47yOg+j4d7vm4TJ6Kl6Lq3VUJ/CyCjxvzR17cRb9pV6MJ/mcZHGST0kWJllR8Ysk25IMlFIeV353KeVNs4uWmRdbGPfqOC+19gA96MdpDGamwOFq4ATGMKsu2+6a3zijQCWdwihWV9yJZU1O2380+pJsaoFSyvtSytsm4Q/i/PXco1Ry8QAAAABJRU5ErkJggg=="; break;
                case 2: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFeSURBVDiNndO/b41RHAbwz7m9q6TX0FhIpNdiMggmaSRWi80qYTBLBInFZJCwS40W/4CI9tpYCGlD0dI0kkuHRkTiDh5Dz5u8SuLlWZ6Tc57ne57v+SHJMMkdSHIxf8elqp1PMuzjLgb+HUcw38PR/zA3ONZDH9NJetjdwTSo2gH6Wr2tJJl0OIPvVZsk6bcqr+N5x+gDHICSJHhfStm/U5VkDgWfMcRjnMBTPMJs08LSn7ZJMk6ymeR+1V2ofD3Ji19aqONd2MI0vuJmTbCKNSzgFh7gVGNMkqUkV+v4SuUbSTaSfExyr86drXzttwRYwUO8rbxc++zhpe0rXq9rrxtTu8CnGnWMdy0u2MAbPCmlnKzJL+8sMIdz1Xge33AGUxjhNG7jS/ugS5IJNnEYB/EMh/AKe6puC/swKqX8SDKFD5gpSUY4XmOOdcMM9mJRtr/zYsdn3GCSZCHJ7E+XzmWrlMF+ngAAAABJRU5ErkJggg=="; break;
                case 3: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFXSURBVDiNlZI/SNxBEIW/USM5tBCsbATTaSNHQAjYBasrtElrZyuEgIVgJxaxEYSoAQtr65SGEIIIBm0sBL3m4CAgGD1EDOJ9Nnvyy8L55zU7++bNY2Z2oQ3Ukjqn/lT31S/qQK7rUjdSXI2Iz6m4F9gBeoB1oAFUgCO1EhF7DwbAX6AbWFLXI6IBzKfc24j4l0xrwDGwpQ5HRDNvuaa+T/GJ+kGdVfvVGfVKfafeqOVWXRQMtoE3QBWYAiaAj0AZ6AMmI+KHWk+aP0C92MEn/8eC2q0uq2NJM5JpTosG41nyWq0U8kPqYabZLI5QAi6BV9lLnQAXwGhadhHT+SJ/+zIMdmSOezwf1Yio5QbfAZ9p8K1tRu1sw5eetFXX1Fv1wPT31VC/qk31WB1qV1zOlrSY+NGMX2nV5Du4BO4K9/N0NjL+7LERptVf6qr6OuN31Q21p8XfA7OzcGr+QGdLAAAAAElFTkSuQmCC"; break;
                case 4: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAHdElNRQfjCxUPExbSdObOAAABQUlEQVQoz22QzSvDARzGP79tWMN+h205eKnJW8pw5eKgKFMuXlZE7epIKTk5KRwc1JI/QFGOwsVRc/NW2yjJyewl0YQeh/22Nu25fXue7/f5Pg8UoSEqwLDIfgK4yBI3ohVU6lZKC5LCelNfOWeTTw0skWAEGOOWZWvJLqfschoaBkxyuPGQ5BWvcQCgdc5IsOkwzkHz/HCNl0+acFq3QzzhIuQAIMcUXfQS44YjS3DJHnCZ9xvUsfI40YD1Q5VulZZZiLmLGzcpfo1wMdsFbUZjYWjVnQ51r46S8Bd6AYfV16MW6SRixP53ZAPVKqA5PKzhVVg9qisvaoUZrvhggiDjvBFlVqsgk2bcakE57SiujCY1qmkl9awNfatap5KkBxs1+Ehjss8XETxk8OHAjp8g3fhtRbN62jFLzLd5J8vWH6B2i7jd3JbvAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE5LTExLTIxVDE1OjE5OjIyKzAwOjAwdMIXfAAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOS0xMS0yMVQxNToxOToyMiswMDowMAWfr8AAAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"; break;
            }
            return url;
        }
    }
}
