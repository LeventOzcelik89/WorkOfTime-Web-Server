using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SH_UserHandler : BaseSmartHandler
    {
        public SH_UserHandler()
            : base("SH_UserHandler")
        {

        }

        [HandleFunction("SH_User/MBUpdate")]
        public void SH_UserMBUpdate(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();

            var userId = CallContext.Current.UserId;
            var _user = ParseRequest<SH_User>(context);

            var _shUser = db.GetSH_UserById(_user.id == null ? userId : _user.id);

            if (!string.IsNullOrEmpty(_user.password))
            {
                _shUser.password = db.GetMd5Hash(db.GetMd5Hash(_user.password));
            }

            _shUser.changed = DateTime.Now;
            _shUser.changedby = userId;
            _shUser.firstname = _user.firstname.ToUpper();
            _shUser.lastname = _user.lastname.ToUpper();
            _shUser.email = _user.email;
            _shUser.cellphone = _user.cellphone;
            _shUser.address = _user.address;
            _shUser.phone = _user.phone;

            var dbresult = db.UpdateSH_User(_shUser);

            if (dbresult.result)
            {
                RenderResponse(context, new ResultStatus { result = true, objects = db.GetVWSH_UserById(_user.id), message = "Profil düzenleme işlemi başarıyla gerçekleşti.." });
            }
            else
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Profil düzenlemene işlemi başarısız oldu." });
            }
        }

        [HandleFunction("SH_User/ChangePassword")]
        public void SH_UserChangePassword(HttpContext context)
        {
            try
            {
                if (CallContext.Current == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                    return;
                }
                var item = ParseRequest<VWSH_UserChangePassword>(context);
                if (item == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Şifre alanları boş gönderilemez." });
                    return;
                }


                var res = new VMSH_UserModel().ChangePassword(item, CallContext.Current.UserId);

                RenderResponse(context, res);
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("VWSH_User/GetPageInfo")]
        public void VWSH_UserGetPageInfo(HttpContext context)
        {
            try
            {
                RenderResponse(context, new ResultStatus { result = true });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWSH_User/GetChildAndManager")]
        public void VWSH_UserGetChildAndManager(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var upDown = context.Request["upDown"];
                var childPersons = new ManagersCalculator().GetAllChilds(CallContext.Current.UserId);
                var usermanagers = db.GetVWINV_CompanyPersonDepartmentsByUserIdAndEndDateNullIsBaseTrue(CallContext.Current.UserId);

                if (usermanagers == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Departman bulunamadı." });
                }

                var managerList = new List<Guid?>();
                managerList.Add(usermanagers.Manager1);
                managerList.Add(usermanagers.Manager2);
                managerList.Add(usermanagers.Manager3);
                managerList.Add(usermanagers.Manager4);
                managerList.Add(usermanagers.Manager5);
                var childUserList = new List<VWSH_UserOrderType>();
                var chsay = 0;
                if (Convert.ToBoolean(upDown))
                {
                    foreach (var chilPerson in childPersons.Where(x => x.IdUser != CallContext.Current.UserId))
                    {
                        if (!chilPerson.IdUser.HasValue)
                        {
                            continue;
                        }
                        var user = db.GetVWSH_UserById(chilPerson.IdUser.Value);
                        if (user != null)
                        {
                            var data = new VWSH_UserOrderType().B_EntityDataCopyForMaterial(user);
                            data.order = (chsay + 1);
                            data.upDown = true;
                            childUserList.Add(data);
                            chsay = chsay + 1;
                        }
                    }
                }
                else
                {
                    var syc = 0;
                    foreach (var manager in managerList.Where(x => x.HasValue).ToArray())
                    {
                        var user = db.GetVWSH_UserById(manager.Value);
                        if (user != null)
                        {
                            var data = new VWSH_UserOrderType().B_EntityDataCopyForMaterial(user);
                            data.order = (syc + 1);
                            data.upDown = false;
                            childUserList.Add(data);
                            syc = syc + 1;
                        }
                    }
                }
                RenderResponse(context, new ResultStatus { result = true, objects = childUserList });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }

    public class VWSH_UserOrderType : VWSH_User
    {
        public bool upDown { get; set; }
        public int order { get; set; }
    }
}