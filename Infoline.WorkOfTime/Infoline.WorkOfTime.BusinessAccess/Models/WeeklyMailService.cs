using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{

    public class ItemWeekEnd
    {
        public string _mainTitle { get; set; }
        public string _content { get; set; }

        public ItemWeekEnd(string mainTitle, string content)
        {
            _mainTitle = mainTitle;
            _content = content;
        }
    }

    public class WeeklyMailService
    {
        public List<ItemWeekEnd> ItemsEnd { get; set; } = new List<Models.ItemWeekEnd>();
        public List<VWPRJ_Project> PRJ_TodayInsertProject { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_TodayInsertProjectKMs { get; set; }
        public List<VWPRJ_Project> PRJ_ApproachLastSevenDayProject { get; set; }
        public List<VWPRJ_Project> PRJ_TodayEndProjects { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_ApproachLastSevenDayStartProjectKMs { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_TodayStartProjectKMs { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_ApproachLastSevenDayEndProjectKMs { get; set; }
        public List<VWPRJ_ProjectTimeline> PRJ_TodayEndProjectKMs { get; set; }

        public List<VWINV_CompanyCalendar> INV_CompanyMeetingsTodayInsert { get; set; }
        public List<VWINV_CompanyPersonCalendar> INV_CompanyNotificationsTodayInsert { get; set; }
        public List<VWINV_CompanyPersonCalendar> INV_CompanyPresentationsTodayInsert { get; set; }

        public List<VWINV_Permit> INV_TodayPermitAcceptPersons { get; set; }
        public List<VWINV_Permit> INV_TodayRequestPermitOffer { get; set; }

        public int FTM_OpenTaskCount { get; set; }
        public List<VWFTM_Task> FTM_TodayOpenTask { get; set; }
        public List<VWFTM_TaskOperation> FTM_TodaySolutionReportTaskOperation { get; set; }
        public List<VWFTM_TaskOperation> FTM_TodaySolutionAcceptTaskOperation { get; set; }
        public int INV_ApproachMaintenanceCount { get; set; }

        public List<VWCRM_Presentation> CRM_TodayInsertPresentation { get; set; }
        public List<VWCRM_Contact> CRM_TodayHappeningMeeting { get; set; }
        public List<VWCRM_Contact> CRM_TodayCancelMeeting { get; set; }
        public List<VWCRM_Presentation> CRM_TodayAcceptSales { get; set; }
        public List<VWCRM_Presentation> CRM_PresentationTodayPositiveResults { get; set; }

        public WeeklyMailService(WorkOfTimeDatabase db, string _url)
        {

            //Proje Yönetimi
            PRJ_TodayInsertProject = db.GetVWPRJ_ProjectByInsertToday(-7).ToList();
            PRJ_TodayInsertProjectKMs = db.GetVWPRJ_ProjectTimeLineByTodayInsert(-7).ToList();
            PRJ_ApproachLastSevenDayProject = db.GetVWPRJ_ProjectApproachByLastSevenDay(-7).ToList();
            PRJ_TodayEndProjects = db.GetVWPRJ_ProjectApproachByEnd(-7).ToList();
            PRJ_ApproachLastSevenDayStartProjectKMs = db.GetVWPRJ_ProjectTimeLineApproachStartByLastSevenDay(-7).ToList();
            PRJ_TodayStartProjectKMs = db.GetVWPRJ_ProjectTimeLineByTodayStart(-7).ToList();
            PRJ_ApproachLastSevenDayEndProjectKMs = db.GetVWPRJ_ProjectTimeLineApproachEndByLastSevenDay(-7).ToList();
            PRJ_TodayEndProjectKMs = db.GetVWPRJ_ProjectTimeLineByTodayEnd(-7).ToList();
            //Proje Yönetimi

            //Buhaftaki Etkinlikler
            INV_CompanyMeetingsTodayInsert = db.GetVWINV_CompanyMeetingByTodayInsert(-7).ToList();
            INV_CompanyNotificationsTodayInsert = db.GetVWINV_CompanyPersonNotificationByTodayInsert(-7).ToList();
            INV_CompanyPresentationsTodayInsert = db.GetVWINV_CompanyPersonPresentationByTodayInsert(-7).ToList();
            //Buhaftaki Etkinlikler


            //İnsan Kaynakları
            INV_TodayPermitAcceptPersons = db.GetVWINV_PermitPersonToday(-7).ToList();
            INV_TodayRequestPermitOffer = db.GetVWINV_PermitRequestOfferByToday(-7).ToList();

            var INV_TodayRequestCommissions = db.GetVWINV_CommissionsRequestByToday().ToArray();

            var INV_TodayRequestCommissionPersons = db.GetVWINV_CommissionsPersonsByIds(INV_TodayRequestCommissions.Select(x => x.id).ToArray()).GroupBy(x => x.id).Select(x => new
            {
                Names = String.Join(",", x.Select(d => d.Person_Title)),
                CommissionsType = x.Select(d => d.Information_Title).FirstOrDefault(),
                StartDate = x.Select(d => d.StartDate).FirstOrDefault(),
                EndDate = x.Select(d => d.EndDate).FirstOrDefault()
            }).ToArray();

            var INV_CommissionAcceptWaiting = db.GetVWINV_CommissionsWaitingByAccept().ToList();


            var INV_AcceptWaitingPersons = db.GetVWINV_CommissionsPersonsByIds(INV_CommissionAcceptWaiting.Select(x => x.id).ToArray()).GroupBy(x => x.id).Select(x => new
            {
                Names = String.Join(",", x.Select(d => d.Person_Title)),
                CommissionsType = x.Select(d => d.Information_Title).FirstOrDefault(),
                StartDate = x.Select(d => d.StartDate).FirstOrDefault(),
                EndDate = x.Select(d => d.EndDate).FirstOrDefault()
            }).ToArray();

            //İnsan Kaynakları

            //Saha Görev Yönetimi
            FTM_OpenTaskCount = db.GetVWFTM_OpenTask().Count();
            FTM_TodayOpenTask = db.GetVWFTM_OpenTodayTask(-7).ToList();
            FTM_TodaySolutionReportTaskOperation = db.GetFTM_TodayReportSolution(-7).ToList();
            FTM_TodaySolutionAcceptTaskOperation = db.GetFTM_TodayAcceptSolutionTaskOperation(-7).ToList();
            var FTM_TodayClosed = db.GetFTM_MostAcceptSolutionPersonTaskOperation().GroupBy(x => x.user_Title).Select(a => new
            {
                //Data = a.ToArray(),
                Title = a.Key,
                Count = a.Count()
            }).OrderByDescending(a => a.Count);
            //Saha Görev Yönetimi

            //Satışlar
            CRM_TodayInsertPresentation = db.GetVWCRM_PresentationInsertByToday(-7).ToList();
            CRM_TodayHappeningMeeting = db.GetVWCRM_ContactTodayHappeningMeeting(-7).ToList(); // Toplantı İsmi ile ilgili bilgi yok.
            CRM_TodayCancelMeeting = db.GetVWCRM_ContactTodayCanceldMeeting(-7).ToList(); // Toplantı İsmi ile ilgili bilgi yok.
        
            CRM_PresentationTodayPositiveResults = db.GetVWCRM_PresentationPositiveResultByToday(-7).ToList();
            //Satışlar

            if (PRJ_TodayInsertProject.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bu hafta eklenen proje sayısı</a>" + "*" + PRJ_TodayInsertProject.Count()
                        ));
            }

            if (PRJ_TodayInsertProjectKMs.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bu hafta eklenen proje kilometre taşı sayısı</a>" + "*" + PRJ_TodayInsertProjectKMs.Count()
                        ));
            }

            if (PRJ_ApproachLastSevenDayProject.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bitişi yaklaşan proje sayısı</a>" + "*" + PRJ_ApproachLastSevenDayProject.Count()
                        ));
            }

            if (PRJ_TodayEndProjects.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bu hafta biten proje adedi</a>" + "*" + PRJ_TodayEndProjects.Count()
                        ));
            }

            if (PRJ_ApproachLastSevenDayStartProjectKMs.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Yaklaşan proje kilometre taşı sayısı</a>" + "*" + PRJ_ApproachLastSevenDayStartProjectKMs.Count()
                        ));
            }

            if (PRJ_TodayStartProjectKMs.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bu hafta başlayan kilometre taşı sayısı</a>" + "*" + PRJ_TodayStartProjectKMs.Count()
                        ));
            }

            if (PRJ_ApproachLastSevenDayEndProjectKMs.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bitişi yaklaşan proje kilometre taşı adedi</a>" + "*" + PRJ_ApproachLastSevenDayEndProjectKMs.Count()
                        ));
            }

            if (PRJ_TodayEndProjectKMs.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Proje Yönetimi",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/PRJ/VWPRJ_Project/Index>Bu hafta biten proje kilometre taşı adedi</a>" + "*" + PRJ_TodayEndProjectKMs.Count()
                        ));
            }

            if (INV_CompanyMeetingsTodayInsert.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Buhaftaki Etkinlikler",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index>Bu hafta eklenen toplantı sayısı</a>" + "*" + INV_CompanyMeetingsTodayInsert.Count()
                        ));
            }

            if (INV_CompanyNotificationsTodayInsert.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Buhaftaki Etkinlikler",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index>Bu hafta eklenen mevcut duyuru adedi</a>" + "*" + INV_CompanyNotificationsTodayInsert.Count()
                        ));
            }

            if (INV_CompanyPresentationsTodayInsert.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Buhaftaki Etkinlikler",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/Account/Index>Bu hafta gerçekleşecek sunum sayısı</a>" + "*" + INV_CompanyPresentationsTodayInsert.Count()
                        ));
            }

            if (INV_TodayPermitAcceptPersons.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "İnsan Kaynakları",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/INV/VWINV_Permit/Index>Bu hafta izinli olan personel sayısı</a>" + "*" + INV_TodayPermitAcceptPersons.Count()
                        ));
            }

            if (INV_TodayRequestPermitOffer.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "İnsan Kaynakları",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/INV/VWINV_Permit/Index>İzin talebi onayı beklemekte olan personel sayısı</a>" + "*" + INV_TodayRequestPermitOffer.Count()
                        ));
            }

            if (INV_TodayRequestCommissionPersons.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "İnsan Kaynakları",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/INV/VWINV_Commissions/Index> Gerçekleşen görevlendirme mevcudu</a>" + "*" + INV_TodayRequestCommissionPersons.Count()
                        ));
            }

            if (INV_AcceptWaitingPersons.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "İnsan Kaynakları",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/INV/VWINV_Commissions/Index>Onay bekleyen görevlendirme sayısı</a>" + "*" + INV_AcceptWaitingPersons.Count()
                   ));
            }

            if (FTM_TodayOpenTask.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Saha Görev Yönetimi",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/FTM/VWFTM_Task/Index>Saha görevi sayısı</a>" + "*" + FTM_TodayOpenTask.Count()
                        ));
            }

            if (FTM_TodaySolutionAcceptTaskOperation.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Saha Görev Yönetimi",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/FTM/VWFTM_Task/Index>Çözümü gerçekleşen saha görevi sayısı</a>" + "*" + FTM_TodaySolutionAcceptTaskOperation.Count()
                        ));
            }

            if (FTM_TodaySolutionReportTaskOperation.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Saha Görev Yönetimi",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/FTM/VWFTM_Task/Index>Çözümü onaylanan saha görevi adedi</a>" + "*" + FTM_TodaySolutionReportTaskOperation.Count()
                        ));
            }

            if (FTM_TodayClosed.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Saha Görev Yönetimi",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/FTM/VWFTM_Task/Index>En çok iş kapatan personel sayısı</a>" + "*" + FTM_TodayClosed.Count()
                        ));
            }

            if (CRM_TodayInsertPresentation.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Satış Faaliyetleri",
                    "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/CRM/VWCRM_Presentation/Index>Yeni eklenen potansiyel mevcudu</a>" + "*" + CRM_TodayInsertPresentation.Count()
                        ));
            }


            if (CRM_PresentationTodayPositiveResults.Count() > 0)
            {
                ItemsEnd.Add(new Models.ItemWeekEnd(
                    "Satış Faaliyetleri",
                     "<a style ='font-size:12px;font-weight:bold;color: #000; text-decoration: none;' href=" + _url + "/CRM/VWCRM_Presentation/Index> Bu hafta eklenen potansiyel sayısı</a>" + "*" + CRM_PresentationTodayPositiveResults.Count()
                ));
            }
        }

        public string GetDayEndHTML()
        {
            string htmlText = "";
            string[] colorArray = new string[] { "#1ab394", "#1c84c6", "#23c6c8", "#f8ac59", "#ed5565", "#1ab394", "#1c84c6", "#23c6c8", "#f8ac59", "#ed5565" };
            int count = 0;
            foreach (var mainTitles in ItemsEnd.GroupBy(a => a._mainTitle))
            {
                var cm = mainTitles.SelectMany(a => a._content).Count();
                if (cm != 0)
                {
                    htmlText += "<table cellspacing='0' cellpadding='0' style='width:100%;border:1px solid " + colorArray[count] + ";border-spacing: 0;'>"
                            + "<tr style='background-color:" + colorArray[count] + ";height:35px'>"
                            + "<td colspan=2 style='vertical-align:middle;padding-left:10px'><img style='width: 100%;max-width: 20px' src='" + BaseUrl(mainTitles.Key) + " '>&nbsp;&nbsp;<span style='color:white;font-weight:bold;'>" + mainTitles.Key + "</span></td>"

                            + "</tr>";
                    foreach (var content in mainTitles.GroupBy(a => a._content))
                    {
                        htmlText +=
                        "<tr style='line-height: 34px;background-color:#fff;'>"
                        + "<td style='padding-left:10px;border-bottom: 1px solid " + colorArray[count] + ";'>"
                        + "" + content.Key.Split('*')[0] + ""
                        + "</td>"
                        + "<td style='width:30px;text-align:center;border-bottom: 1px solid " + colorArray[count] + ";'>"
                        + "<div style='background-color:" + colorArray[count] + ";width:20px;height: 25px; margin:6px 6px 10px 6px;'>"
                        + "<span style='color:#fff;font-family:Tahoma;font-weight:bold;font-size:13px;'>" + content.Key.Split('*')[1] + "</span><br></br>"
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
                        mainTitles.IndexOf("Buhaftaki Etkinlikler") > -1 ? type = 2 :
                        mainTitles.IndexOf("Satın Almalar") > -1 ? type = 3 :
                        mainTitles.IndexOf("Saha Görev Yönetimi") > -1 ? type = 4 :
                        mainTitles.IndexOf("Satış Faaliyetleri") > -1 ? type = 5 :
                        0;

            var url = "";
            switch (type)
            {
                case 0: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFQSURBVDiNrZG7SpxRFIW/M1GwMiJExMvYpLHW5AEMKKabNEE0D6C1DESwFcRHsAu5dNZpUlgrouVMTJNinEZGMsXgjc/C/ctk/JURXHDg7L32WqyzDzwn1D51VC2oZXU3zqqagut7SLyiXqpV9ZP3sRjcpbqcZ1CPwW31R47B9+BUa5muEOIEvIpeE+jPCfkyOICh0NwapJQEKkGOAX9yDI6B8bhXQvPfE96rLfWf+la9aIt/rr5RmzEzn+l6QjwR7h+BE2AfmAU+x9xG9GaAEaCoFlNKf5O6AHwBXgBloAqUgNfA1zBYAn4DO8AksAlcA0uoBxHzm7qes/1OrMWs6h7qWRQltdGFwan6Ie6NApBtswkM5Gy/E4PAWVYUgGlgC2h1Ic7QCs3UXUcdUI+6eMKhepc0tduqvcA7YI7b7xoOqg7UgJ/Ar5TS1RPSPo4bJ0WEQwRMgMcAAAAASUVORK5CYII="; break;
                case 1: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAGySURBVDiNhdJNiI9RFAbw544x+VhITDKaZGZIFpNImfK1YCVJFiOZSVLKzsJGSVazMCywUGwsRDRSYycWbNCshJrGQslCvmWhGD+b+8+bxTib+z73POe5533OSWqgYCQzBJZiXfOuDcuxqpQiyfwGeQtOohcXsCDJUJLRmm9rEedgBEPowxI88Dcu4Am+YRrfcQ6T6Gq214X2Sr5dz0EcwjPcxQCOVaHzrcKdOIPNGMYrnMAv7MfX2kl347H7GEd/sBKza+JGLf6A37WTPfV7a+V0YArvMdZeSplqmLooyfd6Jsl0kvVJSpI7uJlkQ5LeJJ2llI9tVfU49iV5V5OTVWB1ktdJhpNcS3IoycMkH5LsaBp4HkewHV9wFD/rv0/gBw7gFnZhDea2io/gaUPsSnV9GI8a47yOg+j4d7vm4TJ6Kl6Lq3VUJ/CyCjxvzR17cRb9pV6MJ/mcZHGST0kWJllR8Ysk25IMlFIeV353KeVNs4uWmRdbGPfqOC+19gA96MdpDGamwOFq4ATGMKsu2+6a3zijQCWdwihWV9yJZU1O2380+pJsaoFSyvtSytsm4Q/i/PXco1Ry8QAAAABJRU5ErkJggg=="; break;
                case 2: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFeSURBVDiNndO/b41RHAbwz7m9q6TX0FhIpNdiMggmaSRWi80qYTBLBInFZJCwS40W/4CI9tpYCGlD0dI0kkuHRkTiDh5Dz5u8SuLlWZ6Tc57ne57v+SHJMMkdSHIxf8elqp1PMuzjLgb+HUcw38PR/zA3ONZDH9NJetjdwTSo2gH6Wr2tJJl0OIPvVZsk6bcqr+N5x+gDHICSJHhfStm/U5VkDgWfMcRjnMBTPMJs08LSn7ZJMk6ymeR+1V2ofD3Ji19aqONd2MI0vuJmTbCKNSzgFh7gVGNMkqUkV+v4SuUbSTaSfExyr86drXzttwRYwUO8rbxc++zhpe0rXq9rrxtTu8CnGnWMdy0u2MAbPCmlnKzJL+8sMIdz1Xge33AGUxjhNG7jS/ugS5IJNnEYB/EMh/AKe6puC/swKqX8SDKFD5gpSUY4XmOOdcMM9mJRtr/zYsdn3GCSZCHJ7E+XzmWrlMF+ngAAAABJRU5ErkJggg=="; break;
                case 3: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFXSURBVDiNlZI/SNxBEIW/USM5tBCsbATTaSNHQAjYBasrtElrZyuEgIVgJxaxEYSoAQtr65SGEIIIBm0sBL3m4CAgGD1EDOJ9Nnvyy8L55zU7++bNY2Z2oQ3Ukjqn/lT31S/qQK7rUjdSXI2Iz6m4F9gBeoB1oAFUgCO1EhF7DwbAX6AbWFLXI6IBzKfc24j4l0xrwDGwpQ5HRDNvuaa+T/GJ+kGdVfvVGfVKfafeqOVWXRQMtoE3QBWYAiaAj0AZ6AMmI+KHWk+aP0C92MEn/8eC2q0uq2NJM5JpTosG41nyWq0U8kPqYabZLI5QAi6BV9lLnQAXwGhadhHT+SJ/+zIMdmSOezwf1Yio5QbfAZ9p8K1tRu1sw5eetFXX1Fv1wPT31VC/qk31WB1qV1zOlrSY+NGMX2nV5Du4BO4K9/N0NjL+7LERptVf6qr6OuN31Q21p8XfA7OzcGr+QGdLAAAAAElFTkSuQmCC"; break;
                case 4: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAHdElNRQfjCxUPExbSdObOAAABQUlEQVQoz22QzSvDARzGP79tWMN+h205eKnJW8pw5eKgKFMuXlZE7epIKTk5KRwc1JI/QFGOwsVRc/NW2yjJyewl0YQeh/22Nu25fXue7/f5Pg8UoSEqwLDIfgK4yBI3ohVU6lZKC5LCelNfOWeTTw0skWAEGOOWZWvJLqfschoaBkxyuPGQ5BWvcQCgdc5IsOkwzkHz/HCNl0+acFq3QzzhIuQAIMcUXfQS44YjS3DJHnCZ9xvUsfI40YD1Q5VulZZZiLmLGzcpfo1wMdsFbUZjYWjVnQ51r46S8Bd6AYfV16MW6SRixP53ZAPVKqA5PKzhVVg9qisvaoUZrvhggiDjvBFlVqsgk2bcakE57SiujCY1qmkl9awNfatap5KkBxs1+Ehjss8XETxk8OHAjp8g3fhtRbN62jFLzLd5J8vWH6B2i7jd3JbvAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE5LTExLTIxVDE1OjE5OjIyKzAwOjAwdMIXfAAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOS0xMS0yMVQxNToxOToyMiswMDowMAWfr8AAAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"; break;
                case 5: url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAHdElNRQfjCxUPEjuOsIv6AAAAuUlEQVQoz4WQuw7BYBiGn781icMqEYtGXEAXhiYGg91msbgCs8HgJqx2BpvESEwiRFyCwcjMa2jj0Kq+45cn7+GDBBnQkAouBY7sGZvDN2AB0KFKnhUXJuorEwVOAPRJM8ehFEpRQ5Y8NZTVTDX1tIstpJu68nQNlfwABhQRZzOK93BUjs5s/vnC0oAEwJQNdnC/U6cNYEzqBa9pBaPhwcIH/Ajf4cqWewDYuOR8hzfwu4OxSFAikKgn/hQ2aBxktaQAAAAldEVYdGRhdGU6Y3JlYXRlADIwMTktMTEtMjFUMTU6MTg6NTkrMDA6MDCTwiGhAAAAJXRFWHRkYXRlOm1vZGlmeQAyMDE5LTExLTIxVDE1OjE4OjU5KzAwOjAw4p+ZHQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAAASUVORK5CYII="; break;
            }
            return url;
        }



    }
}
