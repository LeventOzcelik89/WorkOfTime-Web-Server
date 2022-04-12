using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMCRM_PresentationModel : VWCRM_Presentation
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWCRM_PresentationProducts[] Products { get; set; }
        public CRM_Contact LastContact { get; set; }
        public CMP_Company CustomerCompany { get; set; }
        public List<Guid> OpponentCompanies { get; set; } = new List<Guid> { };
        public OpponentCompanyTitles[] OpponentCompanies_Titles { get; set; } = new OpponentCompanyTitles[] { };
        public List<Guid> SalesPersons { get; set; } = new List<Guid> { };
        public string ProductsJson { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
        public VWCMP_Tender LastTender { get; set; }
        public IGeometry location { get; set; }

        public VMCRM_PresentationModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var presentation = db.GetVWCRM_PresentationById(this.id);
            if (presentation != null)
            {
                this.B_EntityDataCopyForMaterial(presentation, true);
                var companies = db.GetCRM_PresentationOpponentCompanyByPresentationId(this.id);
                this.OpponentCompanies = companies.Where(a => a.OpponentCompanyId.HasValue).Select(a => a.OpponentCompanyId.Value).ToList();
                this.OpponentCompanies_Titles = db.GetVWCRM_PresentationOpponentCompanyByPresentationIdWithIdAndName(this.id);
                this.Products = db.GetVWCRM_PresentationProductsByPresentationId(this.id);
                this.LastContact = db.GetCRM_ContactLastContact(this.id);
                if (this.CustomerCompanyId.HasValue)
                {
                    var customer = db.GetCMP_CompanyById(this.CustomerCompanyId.Value);
                    if (customer != null)
                    {
                        this.customerEmail = customer.email;
                        this.customerPhone = customer.phone;
                    }
                }

            }
            this.LastContact = this.LastContact ?? new CRM_Contact();
            this.LastContact.ContactStartDate = DateTime.Now;
            this.LastContact.ContactEndDate = DateTime.Now.AddHours(1);
            this.SalesPersons = db.GetUserByRoleId(SHRoles.SatisPersoneli).ToList();
            this.PresentationStageId = this.PresentationStageId ?? db.GetCRM_ManagerStageDefaultValue()?.id;


            return this;
        }

        public ResultStatus Save(Guid? userId, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();
            var presentation = db.GetVWCRM_PresentationById(this.id);
            var rs = new ResultStatus { result = true, };
            if (presentation == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert();
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                rs = Update();
            }

            if (_trans == null)
            {
                if (rs.result) this.trans.Commit(); else this.trans.Rollback();
            }

            return rs;
        }


        private ResultStatus Insert()
        {
            var customerCompany = db.GetCMP_CompanyById((Guid)this.CustomerCompanyId);
            var rs = new ResultStatus { result = true };

            if (this.CustomerCompanyId == Guid.Empty)
            {

                if (string.IsNullOrEmpty(this.CustomerCompany.name))
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = "Öğrenci Adı soyadı zorunludur.Lütfen kontrol edip tekrar deneyin.",
                    };
                }
                this.CustomerCompanyId = this.CustomerCompany.id;
                this.CustomerCompany.type = (int)EnumCMP_CompanyType.Diger;
                this.CustomerCompany.taxType = (int)EnumCMP_CompanyTaxType.GercekKisi;
                this.CustomerCompany.created = this.created;
                this.CustomerCompany.createdby = this.createdby;
                rs = db.InsertCMP_Company(this.CustomerCompany, this.trans);
                customerCompany = this.CustomerCompany;

            }
            else
            {

                if (customerCompany == null)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = "Seçmiş olduğunuz müşteri silinmiş. Lütfen kontrol edip tekrar deneyin."
                    };
                }
            }

            var state = db.GetCRM_ManagerStageById(this.PresentationStageId.Value);

            rs &= db.InsertCRM_Presentation(new CRM_Presentation().B_EntityDataCopyForMaterial(this, true), this.trans);
            rs &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                description = "Potansiyel/Fırsat oluşturuldu.",
                presentationId = this.id,
                type = (int)EnumCRM_PresentationActionType.YeniFırsat,
                location = this.location
            }, this.trans);

            rs &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                description = "Potansiyel/Fırsat aşaması belirlendi => " + state.Name,
                presentationId = this.id,
                color = state.color,
                type = (int)EnumCRM_PresentationActionType.AsamaBelirleme,
                location = this.location
            }, this.trans);


            if (this.OpponentCompanies.Count() > 0)
            {
                rs &= db.BulkInsertCRM_PresentationOpponentCompany(this.OpponentCompanies.Select(x => new CRM_PresentationOpponentCompany
                {
                    createdby = this.createdby,
                    OpponentCompanyId = x,
                    PresentationId = this.id
                }), this.trans);
            }



            if (ProductsJson != null && ProductsJson != "")
            {
                var selectedProducts = Infoline.Helper.Json.Deserialize<List<CRM_PresentationProducts>>(ProductsJson)
                .GroupBy(a => a.ProductId)
                .Select(a => new CRM_PresentationProducts
                {
                    createdby = this.createdby,
                    created = this.created,
                    PresentationId = this.id,
                    ProductId = a.Key,
                    Amount = a.Sum(c => c.Amount),
                }).ToArray();

                rs &= db.BulkInsertCRM_PresentationProducts(selectedProducts, this.trans);
            }
            else
            {
                var serviceSelectedProducts = this.Products.GroupBy(a => a.ProductId).Select(a => new CRM_PresentationProducts
                {
                    createdby = this.createdby,
                    created = this.created,
                    PresentationId = this.id,
                    ProductId = a.Key,
                    Amount = a.Sum(x => x.Amount)
                });
                rs &= db.BulkInsertCRM_PresentationProducts(serviceSelectedProducts, this.trans);
            }


            if (this.LastContact != null && this.LastContact.ContactType.HasValue)
            {
                this.LastContact.PresentationId = this.id;
                this.LastContact.created = this.created;
                this.LastContact.createdby = this.createdby;
                this.LastContact.PresentationStageId = this.PresentationStageId;
                rs &= db.InsertCRM_Contact(this.LastContact, this.trans);
                rs &= db.InsertCRM_ContactUser(new CRM_ContactUser
                {
                    ContactId = this.LastContact.id,
                    created = this.created,
                    createdby = this.createdby,
                    UserId = this.createdby,
                    UserType = (Int32)EnumCRM_ContactUserUserType.OwnerUser,
                }, this.trans);
                rs &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    description = "Yeni Aktivite/Randevu eklendi",
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.YeniAktivite,
                    contactId = this.LastContact.id,
                    location = this.location
                }, this.trans);
            }


            if (this.SalesPersonId.HasValue)
            {   ///Todo:Şahin
                ///bu kısıma pek güvenlmediğinden try bloğuna alındı
                try
                {
                    //Arayüze koyulacak
                    var _products = db.GetVWCRM_PresentationProductsByPresentationId(this.id);
                    var productlist = string.Join(", ", _products.Select(c => c.Product_Title + " " + c.Amount + " Adet"));
                    var _salesUser = db.GetVWSH_UserById(this.SalesPersonId.Value);
                    if (_salesUser != null)
                    {
                        var _salesUserManager = new List<Guid> { _salesUser.id };
                        if (_salesUser.Manager1.HasValue) _salesUserManager.Add(_salesUser.Manager1.Value);
                        //if (_salesUser.Manager2.HasValue) _salesUserManager.Add(_salesUser.Manager2.Value);

                        var users = string.Join(";", db.GetSH_UserByIds(_salesUserManager.ToArray()).Select(c => c.email));
                        var viewItem = db.GetVWCRM_PresentationById(this.id);
                        var companySector = db.GetVWCMP_SectorItemByCompanyId(this.CustomerCompanyId.Value);
                        if (_salesUserManager != null)
                        {
                            var text = "<h3>Merhaba,</h3>";
                            text += "<p>{0} isimli personel tarafından {1} tarihinde ({2}) potansiyel oluşturulmuştur.</p>";
                            text += "<p><b>Potansiyel Bilgileri ;</b></p>";
                            text += "<p>Müşteri : {6}</p>";
                            text += "<p>Kanal : {7}</p>";
                            text += "<p>Aşama : {8}</p>";
                            text += "<p>Satış Personeli : {9}</p>";
                            text += "<p>Ürünler : {3}</p>";
                            text += "<p>Potansiyelin detayı için lütfen <a href='{4}/CRM/VWCRM_Presentation/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                            text += "<p> Bilgilerinize. </p>";
                            var mesaj = string.Format(text, _salesUser.firstname + " " + _salesUser.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", this.created),
                                this.Name, productlist, TenantConfig.Tenant.GetWebUrl(), this.id, viewItem.CustomerCompany_Title, viewItem.ChannelCompany_Title, viewItem.Stage_Title, viewItem.SalesPerson_Title,
                                 viewItem.CustomerCompany_Title);
                            new Email().Template("Template1", "potansiyel.jpg", "Yeni Potansiyel Hakkında", mesaj)
                             .Send((Int16)EmailSendTypes.YeniMusteri, users, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "Yeni Potansiyel Hakkında"), true);
                        }
                    }
                }
                catch
                {
                }

            }

            return new ResultStatus
            {
                result = rs.result,
                message = rs.result ? "Potansiyel Fırsat kaydı oluşturuldu..." : "Potansiyel Fırsat kaydı oluşturulurken sorunlar oluştu."
            };
        }

        private ResultStatus Update(DbTransaction trans = null)
        {

            var oldPresentation = db.GetVWCRM_PresentationById(this.id);
            var presentation = new CRM_Presentation().B_EntityDataCopyForMaterial(this, true);
            presentation.createdby = oldPresentation.createdby;
            presentation.created = oldPresentation.created;
            var dbresult = db.UpdateCRM_Presentation(presentation, true, this.trans);

            var products = db.GetVWCRM_PresentationProductsByPresentationId(this.id);
            dbresult &= db.BulkDeleteCRM_PresentationProducts(products.Select(x => new CRM_PresentationProducts().B_EntityDataCopyForMaterial(x)), this.trans);

            if (ProductsJson != null && ProductsJson != "")
            {
                dbresult &= db.BulkInsertCRM_PresentationProducts(Infoline.Helper.Json.Deserialize<List<CRM_PresentationProducts>>(ProductsJson).GroupBy(a => a.ProductId)
                    .Select(a => new CRM_PresentationProducts
                    {
                        createdby = this.createdby,
                        created = this.created,
                        PresentationId = this.id,
                        ProductId = a.Key,
                        Amount = a.Sum(c => c.Amount),
                    }).ToArray(), this.trans);
            }
            else
            {
                dbresult &= db.BulkInsertCRM_PresentationProducts(Products.GroupBy(a => a.ProductId)
                    .Select(a => new CRM_PresentationProducts
                    {
                        createdby = this.createdby,
                        created = this.created,
                        PresentationId = this.id,
                        ProductId = a.Key,
                        Amount = a.Sum(c => c.Amount),
                    }).ToArray(), this.trans);
            }


            var companies = db.GetCRM_PresentationOpponentCompanyByPresentationId(this.id);
            dbresult &= db.BulkDeleteCRM_PresentationOpponentCompany(companies, this.trans);
            dbresult &= db.BulkInsertCRM_PresentationOpponentCompany(this.OpponentCompanies.Select(x => new CRM_PresentationOpponentCompany
            {
                createdby = this.createdby,
                OpponentCompanyId = x,
                PresentationId = this.id
            }), this.trans);

            var presentationActionList = new List<CRM_PresentationAction>();

            if (oldPresentation.PresentationStageId != presentation.PresentationStageId && presentation.PresentationStageId.HasValue)
            {
                var oldstate = db.GetCRM_ManagerStageById(this.PresentationStageId.Value);
                var newstate = db.GetCRM_ManagerStageById(presentation.PresentationStageId.Value);
                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    color = newstate.color,
                    type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                    description = "Aşama Güncellendi. " + oldstate.Name + " => " + newstate.Name,
                    location = this.location
                });
            }

            if (oldPresentation.VodafoneOffer != presentation.VodafoneOffer)
            {
                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.FırsatDüzenle,
                    description = "Tahmini Beklenen Ciro güncellendi." + oldPresentation.VodafoneOffer + " => " + this.VodafoneOffer,
                    location = this.location
                });
            }

            if (oldPresentation.CustomerCompanyId != presentation.CustomerCompanyId)
            {
                var oldCustomer = oldPresentation.CustomerCompanyId.HasValue ? db.GetVWCMP_CompanyById(oldPresentation.CustomerCompanyId.Value) : null;
                var newCustomer = this.CustomerCompanyId.HasValue ? db.GetVWCMP_CompanyById(this.CustomerCompanyId.Value) : null;
                var customer = String.IsNullOrEmpty(oldPresentation.CustomerCompany_Title) ? " " : oldPresentation.CustomerCompany_Title + " firmasından alınarak ";

                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.FırsatDüzenle,
                    description = "Müşteri güncellendi. " + oldCustomer.name + " => " + newCustomer.name,
                    location = this.location
                });
            }

            if (oldPresentation.ChannelCompanyId != presentation.ChannelCompanyId && this.ChannelCompanyId.HasValue)
            {
                var newChannel = this.ChannelCompanyId.HasValue ? db.GetVWCMP_CompanyById(this.ChannelCompanyId.Value) : null;
                var newTitle = newChannel != null ? newChannel.name : " ";
                var channel = String.IsNullOrEmpty(oldPresentation.ChannelCompany_Title) ? " " : oldPresentation.ChannelCompany_Title + " firmasından alınarak ";
                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.FırsatDüzenle,
                    description = "Kanal güncellendi. Yeni Kanal : " + newTitle,
                    location = this.location
                });
            }

            if (oldPresentation.Name != presentation.Name)
            {
                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.FırsatDüzenle,
                    description = "Potansiyel Fırsat adı güncellendi. " + oldPresentation.Name + " > " + presentation.Name,
                    location = this.location
                });
            }

            if (oldPresentation.SalesPersonId != presentation.SalesPersonId)
            {
                var oldSalesPerson = oldPresentation.SalesPersonId.HasValue ? db.GetVWSH_UserById(oldPresentation.SalesPersonId.Value) : null;
                var oldTitle = oldSalesPerson != null ? "Eski satış personeli : " + oldSalesPerson.FullName : " ";
                var newSalesPerson = this.SalesPersonId.HasValue ? db.GetVWSH_UserById(this.SalesPersonId.Value) : null;
                var newTitle = newSalesPerson != null ? "Yeni satış personeli : " + newSalesPerson.FullName : " ";
                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.FırsatDüzenle,
                    description = "Satış personeli güncellendi. " + oldTitle + " => " + newTitle,
                    location = this.location
                });
            }

            if (oldPresentation.CompletionRate != presentation.CompletionRate)
            {
                presentationActionList.Add(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.changedby,
                    presentationId = this.id,
                    type = (short)EnumCRM_PresentationActionType.FırsatDüzenle,
                    description = "Kapanma yüzdesi güncellendi. " + oldPresentation.CompletionRate + " => " + this.CompletionRate,
                    location = this.location
                });
            }

            if (presentationActionList.Count() > 0)
            {
                db.BulkInsertCRM_PresentationAction(presentationActionList);

                ///Todo:Şahin
                ///bu kısıma pek güvenlmediğinden try bloğuna alındı
                try
                {
                    if (this.SalesPersonId.HasValue)
                    {
                        //Arayüze koyulacak
                        var _products = db.GetVWCRM_PresentationProductsByPresentationId(this.id);
                        var productlist = string.Join(", ", _products.Select(c => c.Product_Title + " " + c.Amount + " Adet"));
                        var _salesUser = db.GetVWSH_UserById(this.SalesPersonId.Value);
                        if (_salesUser != null)
                        {
                            var _salesUserManager = new List<Guid> { _salesUser.id };
                            if (_salesUser.Manager1.HasValue) _salesUserManager.Add(_salesUser.Manager1.Value);
                            //if (_salesUser.Manager2.HasValue) _salesUserManager.Add(_salesUser.Manager2.Value);

                            var users = string.Join(";", db.GetSH_UserByIds(_salesUserManager.ToArray()).Select(c => c.email));
                            var viewItem = db.GetVWCRM_PresentationById(this.id);
                            var companySector = db.GetVWCMP_SectorItemByCompanyId(this.CustomerCompanyId.Value);
                            if (_salesUserManager != null)
                            {
                                var text = "<h3>Merhaba,</h3>";
                                text += "<p>{0} isimli personel tarafından {1} tarihinde ({2}) potansiyel üzerinde değişiklikler yapılmıştır.</p>";
                                text += "<p><b>Potansiyel Düzenleme Bilgileri ;</b></p>";
                                text += string.Join("", presentationActionList.Select(a => string.Format("<p>{0}</p>", a.description)));
                                text += "<p>Potansiyelin detayı için lütfen <a href='{4}/CRM/VWCRM_Presentation/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                                text += "<p> Bilgilerinize. </p>";
                                var mesaj = string.Format(text, _salesUser.firstname + " " + _salesUser.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", this.changed),
                                    this.Name, productlist, TenantConfig.Tenant.GetWebUrl(), this.id, viewItem.CustomerCompany_Title, viewItem.ChannelCompany_Title, viewItem.Stage_Title, viewItem.SalesPerson_Title,
                                     viewItem.CustomerCompany_Title);

                                new Email().Template("Template1", "potansiyel.jpg", "Yeni Potansiyel Hakkında", mesaj)
                                 .Send((Int16)EmailSendTypes.YeniMusteri, users, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "Yeni Potansiyel Hakkında"), true);
                            }
                        }
                    }
                }
                catch { }
            }


            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? "Potansiyel kaydı güncellendi..." : "Potansiyel kaydı güncellenirken sorunlar oluştu."
            };
        }
        public ResultStatus InsertNote(Guid userId, string note, IGeometry location)
        {

            var action = new CRM_PresentationAction
            {
                created = DateTime.Now,
                createdby = userId,
                presentationId = this.id,
                description = "Not : " + note,
                type = (short)EnumCRM_PresentationActionType.NotEkle,
                color = "#4b4c50",
                location = location
            };

            var res = db.InsertCRM_PresentationAction(action);
            res.objects = db.GetVWCRM_PresentationActionById(action.id);

            return res;
        }

        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;

            if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
            {
                filter |= new BEXP
                {
                    Operand1 = (COL)"SalesPersonId",
                    Operator = BinaryOperator.Like,
                    Operand2 = (VAL)string.Format("%{0}%", userStatus.user.id.ToString())
                };
            }

            query.Filter &= filter;

            return query;

        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbres = new ResultStatus { result = true };
            var _contactUsers = new List<CRM_ContactUser>();
            //Potansiyel
            var presentation = db.GetCRM_PresentationById(id);
            if (presentation == null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Potansiyel/Fırsat bulunamadı"
                };
            }
            var _trans = db.BeginTransaction();
            //Potansiyelin Toplantıları
            var _contact = db.GetCRM_ContactByPresentationId(presentation.id);
            if (_contact.Count() > 0)
            {
                foreach (var _con in _contact)
                {
                    var _contactUser = db.GetCRM_ContactUserByContactId(_con.id);
                    if (_contactUser.Count() > 0)
                    {
                        foreach (var _cntUs in _contactUser)
                        {
                            //Toplantıdaki personeller listeye ekleniyor.
                            _contactUsers.Add(_cntUs);
                        }
                    }


                    if (_contactUsers.Count() > 0)
                    {
                        //Toplantılardaki personeller siliniyor.
                        dbres &= db.BulkDeleteCRM_ContactUser(_contactUsers, _trans);
                    }

                }

                var contactFiles = db.GetSYS_FilesInDataId(_contact.Select(c => c.id).ToArray());
                if (contactFiles.Count() > 0)
                {
                    //Toplantı dosyaları siliniyor.
                    dbres &= db.BulkDeleteSYS_Files(contactFiles, _trans);
                }

                //Toplantı siliniyor
                dbres &= db.BulkDeleteCRM_Contact(_contact, _trans);
            }

            //Ürünler
            var _products = db.GetCRM_PresentationProductsByPresentationId(presentation.id);
            if (_products.Count() > 0)
            {
                //Potansiyelin ürünleri siliniyor.
                dbres &= db.BulkDeleteCRM_PresentationProducts(_products, _trans);
            }

            var _opponentCompany = db.GetCRM_PresentationOpponentCompanyByPresentationId(presentation.id);
            if (_opponentCompany.Count() > 0)
            {
                dbres &= db.BulkDeleteCRM_PresentationOpponentCompany(_opponentCompany, _trans);
            }

            var presentationActions = db.GetCRM_PresentationActionByPresentationId(presentation.id);

            if (presentationActions.Count() > 0)
            {
                dbres &= db.BulkDeleteCRM_PresentationAction(presentationActions, _trans);
            }

            var presentationFiles = db.GetSYS_FilesByDataIdArray(presentation.id).ToArray();
            if (presentationFiles.Count() > 0)
            {
                //Potansiyel/Fırsat resimleri siliniyor.
                dbres &= db.BulkDeleteSYS_Files(presentationFiles, _trans);
            }

            //Potansiyel siliniyor.
            dbres &= db.DeleteCRM_Presentation(presentation, _trans);

            if (dbres.result == false)
            {
                _trans.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "Potansiyel/Fırsat silme işlemi başarısız oldu"
                };

            }

            _trans.Commit();

            return new ResultStatus
            {
                result = true,
                message = "Potansiyel/Fırsat silme işlemi başarıyla gerçekleşti"
            };
        }


        public ResultStatus GetReportDate(SalesFilter filter)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var resData = new ResultData();
            var presentations = db.GetVWCRM_Presentation();
            var stages = db.GetCRM_ManagerStage();
            resData.allPresentationCount = presentations.Count();
            resData.allTotalCustomer = presentations.GroupBy(x => x.CustomerCompanyId).Count();

            var endDate = new DateTime(filter.endDate.Year, filter.endDate.Month, filter.endDate.Day, 23, 59, 59);

            var filterPresentation = new VWCRM_Presentation[] { };
            if (presentations.Count() > 0)
            {
                if (filter.userIds != null && filter.userIds.Where(x => x.HasValue).Count() > 0)
                {
                    filterPresentation = db.GetVWCRM_PresentationBySalesPersonIds(filter.userIds.Select(x => x.Value).ToArray()).Where(x => x.created >= filter.startDate && x.created <= endDate).ToArray();
                }
                else
                {
                    filterPresentation = presentations.Where(x => x.created >= filter.startDate && x.created <= endDate).ToArray();
                }
            }

            if (filterPresentation.Count() > 0)
            {
                resData.stageList = stages.Select(x => new StageList
                {
                    color = x.color,
                    name = x.Name,
                    count = filterPresentation.Where(v => v.PresentationStageId == x.id).Count()
                }).ToList();

                resData.stageList = resData.stageList.Where(a => a.count > 0).ToList();
                resData.totalBid = db.GetVWCMP_TenderByPresentationIds(filterPresentation.Select(v => v.id).ToArray()).Where(x => x.totalAmount.HasValue).Sum(x => x.totalAmount.Value);

                var res = db.GetVWCMP_TenderByPresentationIds(filterPresentation.Select(c => c.id).ToArray());
                resData.tenderProductPrices = db.GetVWCMP_InvoiceItemByInvoiceIds(res.Select(r => r.id).ToArray()).Where(x => x.productId.HasValue).GroupBy(c => c.Product_Title).Select(v => new KeyValue
                {
                    Key = v.Key,
                    Value = v.Select(f => f.price).Sum()
                }).OrderByDescending(x => x.Value).ToArray();


                resData.productAndQuantity = db.GetVWCRM_PresentationProductsByPresentationIds(filterPresentation.Select(c => c.id).ToArray()).Where(x => x.ProductId.HasValue).GroupBy(v => v.Product_Title).Select(v => new KeyValue
                {
                    Key = v.Key,
                    Value = v.Select(x => x.Amount).Sum()
                }).OrderByDescending(c => c.Value).ToArray();

                resData.totalCustomer = filterPresentation.GroupBy(x => x.CustomerCompanyId).Count();
                if (filter.rangeStart.HasValue && filter.rangeEnd.HasValue)
                {
                    var contact = db.GetCRM_ContactByPresentationIds(filterPresentation.Where(x => x.CompletionRate >= filter.rangeStart && x.CompletionRate <= filter.rangeEnd).Select(x => x.id).ToArray());
                    resData.totalVisit = contact.Count(x => x.ContactStatus == (Int16)EnumCRM_ContactContactStatus.ToplantiGerceklesti);
                }
                else if (filter.rangeStart.HasValue)
                {
                    var contact = db.GetCRM_ContactByPresentationIds(filterPresentation.Where(x => x.CompletionRate >= filter.rangeStart && x.CompletionRate <= 100).Select(x => x.id).ToArray());
                    resData.totalVisit = contact.Count(x => x.ContactStatus == (Int16)EnumCRM_ContactContactStatus.ToplantiGerceklesti);
                }
                else
                {
                    var contact = db.GetCRM_ContactByPresentationIds(filterPresentation.Select(x => x.id).ToArray());
                    resData.totalVisit = contact.Count(x => x.ContactStatus == (Int16)EnumCRM_ContactContactStatus.ToplantiGerceklesti);
                }
                var customerIds = filterPresentation.Where(x => x.CustomerCompanyId.HasValue).Select(x => x.CustomerCompanyId.Value).ToArray();
                if (customerIds.Count() > 0)
                {
                    var companys = db.GetCMP_CompanyByIdsAndStartAndEnd(customerIds, filter.startDate, endDate);
                    resData.totalNewCustomer = companys.Count();
                    resData.totalNewCustomerVisit = db.GetVWCRM_ContactByCustomerCompanyIds(companys.Select(v => v.id).ToArray()).Where(x => x.created >= filter.startDate && x.created <= endDate).Count();
                }

                if (filter.rangeStart.HasValue && filter.rangeEnd.HasValue)
                {
                    resData.presentationTotalValue = filterPresentation.Where(x => x.PresentationStageId == new Guid("eddfb2b1-4591-4060-9e72-c75ccfebe50a") && x.VodafoneOffer.HasValue && x.CompletionRate >= filter.rangeStart && x.CompletionRate <= filter.rangeEnd).Sum(x => x.VodafoneOffer.Value);
                }
                else if (filter.rangeStart.HasValue)
                {
                    resData.presentationTotalValue = filterPresentation.Where(x => x.PresentationStageId == new Guid("eddfb2b1-4591-4060-9e72-c75ccfebe50a") && x.VodafoneOffer.HasValue && x.CompletionRate >= filter.rangeStart && x.CompletionRate <= 100).Sum(x => x.VodafoneOffer.Value);
                }
                else
                {
                    resData.presentationTotalValue = filterPresentation.Where(x => x.PresentationStageId == new Guid("eddfb2b1-4591-4060-9e72-c75ccfebe50a") && x.VodafoneOffer.HasValue).Sum(x => x.VodafoneOffer.Value);
                }
                resData.totalEndorsement = filterPresentation.Where(x => x.PresentationStageId == new Guid("266abe2b-6303-4e0e-9601-b3eeaa32d50b") && x.VodafoneOffer.HasValue).Sum(x => x.VodafoneOffer.Value);
            }

            return new ResultStatus { result = true, objects = resData };
        }
    }

    public class SalesFilter
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Guid?[] userIds { get; set; }
        public int? rangeStart { get; set; }
        public int? rangeEnd { get; set; }
    }

    public class ResultData
    {
        //Yeni eklenen müşteri sayısı
        public int totalNewCustomer { get; set; }
        //Toplam müşteri sayısı tarih bazlı potansiyel üzerinden
        public int totalCustomer { get; set; }
        //Tüm müşteri sayısı
        public int allTotalCustomer { get; set; }
        //Toplam ziyaret sayısı
        public int totalVisit { get; set; }
        //Toplam yeni müşteri ziyaret sayısı
        public int totalNewCustomerVisit { get; set; }
        //Toplam Potansiyel tl değeri
        public double presentationTotalValue { get; set; }
        //Toplam satış cirosu
        public double totalEndorsement { get; set; }
        //Potansiyel aşamaları toplamını döner
        public List<StageList> stageList { get; set; }
        //Teklif tutarı
        public double totalBid { get; set; }
        //Tüm tekliflerde bulunan ürünlerin gruplanarak fiyatlarının listelenmesi 
        public KeyValue[] tenderProductPrices { get; set; }

        public KeyValue[] productAndQuantity { get; set; }
        public int allPresentationCount { get; set; }
    }

    public class StageList
    {
        public string name { get; set; }
        public string color { get; set; }
        public int count { get; set; }
    }

    public class VMVWCRM_Presentation : VWCRM_Presentation
    {
        public VWCRM_PresentationAction[] Actions { get; set; }
        public VWCMP_Tender LastTender { get; set; }
        public bool? isAddContact { get; set; }
    }
    public class VMVWCRM_PresentationAction : VWCRM_PresentationAction
    {
        public VWCRM_Presentation[] Presentation { get; set; }
    }


    public class VMCRM_PresentationForMap
    {
        public VWCRM_Presentation[] Presentations { get; set; }
        public VWCMP_Company[] Customers { get; set; }
        public VWCRM_Contact[] Contacts { get; set; }
        public VWCRM_PresentationProducts[] Products { get; set; }
        public VWCRM_PresentationOpponentCompany[] OpponentCompany { get; set; }
        public VWCRM_ContactUser[] ContactUsers { get; set; }
        public VWCRM_ManagerStage[] Stages { get; set; }
    }

    public class VMCRM_PresentationDetailByPerson
    {
        public VWSH_User User { get; set; }
        public VWCRM_ContactUser LastContact { get; set; }
        public VWCRM_ContactUser NextContact { get; set; }
        public Dictionary<string, int?> CountArray { get; set; }
        public Dictionary<string, string> SummaryData { get; set; }
        public VWCRM_Presentation[] Presentations { get; set; }
        public VWCRM_ContactUser[] Contacts { get; set; }
        public VWCRM_PresentationProducts[] Products { get; set; }
        public Guid[] CompletedStages { get; set; }
    }

    public class OpponentCompanyTitles
    {
        public Guid? id { get; set; }
        public string name { get; set; }
    }
}
