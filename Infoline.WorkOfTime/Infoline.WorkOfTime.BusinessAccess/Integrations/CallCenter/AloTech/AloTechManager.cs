using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.AloTech.Dto;
using Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.AloTech
{
    public class AloTechManager : ICallCenterService
    {
        private readonly string token;
        private readonly string endPoint;

        public AloTechManager(string endPoint, string token)
        {
            this.token = token;
            this.endPoint = endPoint;
        }

        public ResultStatus AddCampaignToList(ListDto listDto)
        {
            throw new NotImplementedException();
        }

        public ResultStatus<ContactDto> AddListToContact(ContactDto contactDto)
        {

            var phone = contactDto.Phone.TrimStart('+').TrimStart('9');
            phone = phone.Length == 10 ? "0" + phone : phone;

            var query = "&function=addcontacttocampaign&uniqueid=" + contactDto.UniqueId + "&&startup_profile=" + EnumStartupProfile.mobile;
            if (!string.IsNullOrEmpty(contactDto.CampaignId)) query += "&campaign=" + contactDto.CampaignId;
            if (!string.IsNullOrEmpty(contactDto.ListName)) query += "&listname=" + contactDto.ListName;
            if (!string.IsNullOrEmpty(contactDto.Name)) query += "&name=" + contactDto.Name;
            if (!string.IsNullOrEmpty(contactDto.SurName)) query += "&surname=" + contactDto.SurName;
            if (!string.IsNullOrEmpty(contactDto.Email)) query += "&email=" + contactDto.Email;
            if (!string.IsNullOrEmpty(phone)) query += "&mobilephone=" + phone;
            if (contactDto.PlannedDate != null) query += "&planned_date=" + contactDto.PlannedDate.Value.ToString("yyyy-MM-dd hh:mm:ss");

            var result = this.CallService<AloTechResultDto>(query);
            return new ResultStatus<ContactDto>
            {
                result = result.result && result.objects?.success == true,
                message = result.objects?.message,
                objects = contactDto,
            };
        }

        public List<ListDto> GetLists(string campaignId)
        {
            throw new NotImplementedException();
        }

        public List<CampaignDto> GetCampaigns()
        {
            var result = this.CallService<AloTechCampaignsDto>("&function=getcampaignlist");
            return result.objects?.CampaignList?.Select(a => new CampaignDto
            {
                CampaingId = a.campaignkey,
                CampaignName = a.campaignname,
                CampaignType = a.campaigntype,
                CampaignIsReady = a.isReady,
                CampaignStatus = a.status
            }).ToList();
        }

        public ResultStatus ImportListToContact(List<ContactDto> contactDtos)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var data = Infoline.Helper.Json.Serialize(new
                    {
                        tenant = "fbu.alo-tech.com",
                        app_token = this.token,
                        listname = contactDtos.Select(a => a.ListName).FirstOrDefault(),
                        Datalist = contactDtos.Select(c => new
                        {
                            campaignid = c.CampaignId,
                            phone = c.Phone,
                        })
                    });

                    client.Headers.Add("content-type", "application/json; charset=utf-8");
                    var res = client.UploadString("https://alotech-api.appspot.com/addcontacttocampaign", HttpMethod.Post.Method, data);

                    var resData = Infoline.Helper.Json.Deserialize<AloTechImportResultDto>(res);

                    return new ResultStatus
                    {
                        result = resData.completed,
                        message = resData.completed ? "Aktarım işlemi başarıyla tamamlandı." : "Aktarım yapılamadı." + resData.message,
                    };
                }
                catch (Exception ex)
                {
                    return new ResultStatus
                    {
                        message = ex.Message,
                        result = false
                    };
                }
            }
        }

        public ResultStatus SetCampaignCall(CampaignCallDto campaignCallDto)
        {
            var phone = campaignCallDto.CallbackPhone.TrimStart('+').TrimStart('9');
            phone = phone.Length == 10 ? "0" + phone : phone;
            var query = "&function=setcampaigncall2&queuekey=" + campaignCallDto.DialId;
            query += "&username=" + campaignCallDto.AgentName + "&finishcode=" + campaignCallDto.FinishCode + "&is_success=true&reasoncode=" + campaignCallDto.ReasonCode.ToString().Replace('_', ' ');
            if (campaignCallDto.ReasonCode == EnumReasonCode.Randevu)
            {
                query += "&callbackphone=" + phone;
                query += "&callbackdate=" + campaignCallDto.CallbackTime.ToString("yyyy-MM-dd");
                query += "&callbacktime=" + campaignCallDto.CallbackTime.ToString("hh:mm:ss");
                query += "&callback_phone_type=" + EnumStartupProfile.mobile.ToString();
            }

            var result = this.CallService<AloTechResultDto>(query);
            return new ResultStatus
            {
                result = result.result && result.objects.success == true,
                message = result.objects?.message,
            };
        }

        protected ResultStatus<TResponse> CallService<TResponse>(string data)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var res = client.DownloadString(endPoint + "?app_token=" + this.token + data);
                    return new ResultStatus<TResponse>
                    {
                        result = true,
                        message = "Veri okundu",
                        objects = Infoline.Helper.Json.Deserialize<TResponse>(res)
                    };
                }
                catch (Exception ex)
                {
                    return new ResultStatus<TResponse>
                    {
                        message = ex.Message,
                        result = false
                    };
                }
            }
        }
    }

    public enum EnumStartupProfile
    {
        business = 0,
        custom = 1,
        home = 2,
        mobile = 3
    }
}
