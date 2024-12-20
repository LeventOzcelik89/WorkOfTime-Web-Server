﻿using Infoline.Framework.Database;
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
                var usermanagers = db.GetINV_CompanyPersonDepartmentsByIdUserAndTypeCurrentWork(CallContext.Current.UserId, (int)EnumINV_CompanyDepartmentsType.Organization);

                if (usermanagers.Count()==0)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Departman bulunamadı." });
                }
                var managers = usermanagers.FirstOrDefault();
                var managerList = new List<Guid?>();
                managerList.Add(managers.Manager1);
                managerList.Add(managers.Manager2);
                managerList.Add(managers.Manager3);
                managerList.Add(managers.Manager4);
                managerList.Add(managers.Manager5);
                managerList.Add(managers.Manager6);
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


        [HandleFunction("VWSH_User/CompanyGuide")]
        public void VWSH_UserGetCompanyGuide(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var userId = CallContext.Current.UserId;
                var company = db.GetVWCMP_CompanyByCreatedby(userId);
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                BEXP filter = null;
                filter |= new BEXP
                {
                    Operand1 = (COL)"companyId",
                    Operator = BinaryOperator.In,
                    Operand2 = new ARR { Values = company.Select(x => (VAL)x.id).ToArray() }
                };
                cond.Filter &= filter;
                var model = db.GetVWSH_UserOtherPersonWithCond(cond);
                RenderResponse(context, new ResultStatus() { result = true, objects = model });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWSH_User/GetTaskPersonals")]
        public void VWSH_GetTaskPersonals(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond.Take = 10000;
                var users = db.GetVWSH_User(cond);

                if (users == null)
                {
                    RenderResponse(context, new ResultStatus { result = true, message = "Personel Bulunamadı" });
                }

                var cc = users.Where((x => x.RoleIds != null && (x.RoleIds.Contains(SHRoles.SahaGorevYonetici) ||
               x.RoleIds.Contains(SHRoles.SahaGorevOperator) || x.RoleIds.Contains(SHRoles.SahaGorevPersonel))));
                foreach (var user in users)
                {
                    user.loginname = "******";
                    user.password = "******";
                }
                // users.Where(x => x.RoleIds.IndexOf(roles.sahaGorev) )
                RenderResponse(context, new ResultStatus { result = true, objects = cc });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("SH_User/InsertCustomerPerson")]
        public void SH_UserInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<VMSH_UserModel>(context);

                data.created = DateTime.Now;
                data.createdby = CallContext.Current.UserId;

                if (!string.IsNullOrEmpty(data.RoleIds))
                {
                    data.Roles = data.RoleIds.Split(',').Select(a => new Guid(a)).ToList();
                }

                RenderResponse(context, data.Save());

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