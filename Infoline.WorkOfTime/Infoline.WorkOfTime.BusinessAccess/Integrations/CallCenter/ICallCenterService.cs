using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter
{
    public interface ICallCenterService
    {
        ResultStatus<ContactDto> AddListToContact(ContactDto contactDto);
        ResultStatus AddCampaignToList(ListDto listDto);
        List<CampaignDto> GetCampaigns();
        List<ListDto> GetLists(string campaignId);
        ResultStatus ImportListToContact(List<ContactDto> contactDtos);       
        ResultStatus SetCampaignCall(CampaignCallDto  campaignCallDto);
    }
}
