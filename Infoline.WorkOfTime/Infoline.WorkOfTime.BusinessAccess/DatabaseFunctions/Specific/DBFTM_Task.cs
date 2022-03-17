using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(FTM_Task), "type")]
    public enum EnumFTM_TaskType
    {
        [Description("Arıza"), Generic("icon", "icon-tools", "iconsvg", "M155 506q-8-8-11-22t-3-25-2-11q-2-2-17-15t-19-17q-16-14-28 4l-70 76q-11 12 2 24 2 2 18 14t20 16q6 6 27 6t37 14q14 14 18 38t10 30q2 0 9 7t26 22 41 31q134 90 186 96 122 0 148-2 12 0-8-8-120-52-152-76-80-56-36-114 34-46 38-48 8-8-2-14-2-2-38-35t-38-35q-14-8-18-4-42 48-71 60t-67-12z m286-26l410-476q18-22-2-38l-48-42q-22-14-38 4l-414 472q-8 8 0 20l72 62q12 8 20-2z m554 202q16-104-16-166-50-88-154-62-56 12-100-32l-82-78-68 78 68 70q24 24 31 53t6 65 5 58q12 56 140 112 12 6 18-3t2-15q-12-12-46-80-14-10-12-35t40-53q58-40 96 22 6 12 26 41t22 33q4 10 13 9t11-17z m-858-684l254 248 76-86-246-242q-20-20-38-4l-46 46q-22 18 0 38z", "order", "10","color", "#1ce6ed")]
        Ariza = 0,
        [Description("Bakım"), Generic("icon", "icon-wrench", "iconsvg", "M214 29q0 14-10 25t-25 10-25-10-11-25 11-25 25-11 25 11 10 25z m360 234l-381-381q-21-20-50-20-29 0-51 20l-59 61q-21 20-21 50 0 29 21 51l380 380q22-55 64-97t97-64z m354 243q0-22-13-59-27-75-92-122t-144-46q-104 0-177 73t-73 177 73 176 177 74q32 0 67-10t60-26q9-6 9-15t-9-16l-163-94v-125l108-60q2 2 44 27t75 45 40 20q8 0 13-5t5-14z", "order", "11","color", "#EA899A")]
        Bakim = 1,
        [Description("Kalibrasyon"), Generic("icon", "icon-gauge-1", "iconsvg", "M406 178q34 56 214 284t194 220q12-6-96-278t-138-326q-50-86-136-36t-38 136z m94 380q-168 0-284-127t-116-311q0-30 2-46 2-22-12-37t-34-17-36 12-18 34q0 8-1 26t-1 28q0 226 145 382t355 156q72 0 134-18l-70-86q-40 4-64 4z m362-62q138-154 138-376 0-38-2-56-2-20-16-33t-34-13l-4 0q-22 4-35 20t-11 36q2 14 2 46 0 150-80 268 6 14 20 51t22 57z", "order", "13", "color", "#025669")]
        Kalibrasyon = 2,
        [Description("Parça Değişimi"), Generic("icon", "icon-shuffle", "iconsvg", "M372 582q-34-52-77-153-12 25-20 41t-23 35-28 32-36 19-45 8h-125q-8 0-13 5t-5 13v107q0 8 5 13t13 5h125q139 0 229-125z m628-446q0-8-5-13l-179-179q-5-5-12-5-8 0-13 6t-5 12v107q-18 0-48 0t-45-1-41 1-39 3-36 6-35 10-32 16-33 22-31 30-31 39q33 52 76 152 12-25 20-40t23-36 28-31 35-20 46-8h143v107q0 8 5 13t13 5q6 0 13-5l178-178q5-5 5-13z m0 500q0-8-5-13l-179-179q-5-5-12-5-8 0-13 6t-5 12v107h-143q-27 0-49-8t-38-25-29-34-25-44q-18-34-43-95-16-37-28-62t-30-59-36-55-41-47-50-38-60-23-71-10h-125q-8 0-13 5t-5 13v107q0 8 5 13t13 5h125q27 0 48 9t39 25 28 34 26 43q17 35 43 96 16 36 28 62t30 58 36 56 41 46 50 39 59 23 72 9h143v107q0 8 5 13t13 5q6 0 13-5l178-178q5-5 5-13z", "order", "14", "color", "#cc5114")]
        ParcaDegisimi = 3,
        [Description("Kontrol"), Generic("icon", "icon-eye", "iconsvg", "M929 314q-85 132-213 197 34-58 34-125 0-103-73-177t-177-73-177 73-73 177q0 67 34 125-128-65-213-197 75-114 187-182t242-68 243 68 186 182z m-402 215q0 11-8 19t-19 7q-70 0-120-50t-50-119q0-11 8-19t19-8 19 8 8 19q0 48 34 82t82 34q11 0 19 8t8 19z m473-215q0-19-11-38-78-129-210-206t-279-77-279 77-210 206q-11 19-11 38t11 39q78 128 210 205t279 78 279-78 210-205q11-20 11-39z", "order", "15", "color", "#2552b2")]
        Kontrol = 4,
        [Description("Gel Al"), Generic("icon", "icon-loop-1", "iconsvg", "M703 610q96 0 165-76t70-184-77-183-183-77q-22 0-37 15t-16 37 16 36 37 16q65 0 110 46t46 110-39 111-92 46l-108 0 66-68q15-15 15-37t-15-37-36-15-36 15l-194 194 194 193q15 15 36 15t36-15 15-37-15-37l-66-68 108 0z m-427-275q15 15 37 15t36-15l194-193-194-193q-15-15-36-15t-37 15-15 37 15 37l67 67-108 0q-96 0-166 77t-69 183 76 184 184 76q21 0 37-15t16-36-16-37-37-15q-65 0-111-46t-45-111 39-110 92-46l108 0-67 68q-15 15-15 36t15 37z", "order", "16", "color", "#EFA94A")]
        GelAL = 5,
        [Description("Keşif"), Generic("icon", "icon-binoculars", "iconsvg", "M393 671v-428q0-15-11-25t-25-11v-321q0-15-10-25t-26-11h-285q-15 0-25 11t-11 25v285l139 488q4 12 17 12h237z m178 0v-392h-142v392h142z m429-500v-285q0-15-11-25t-25-11h-285q-15 0-25 11t-11 25v321q-15 0-25 11t-11 25v428h237q13 0 17-12z m-589 661v-125h-197v125q0 8 5 13t13 5h161q8 0 13-5t5-13z m375 0v-125h-197v125q0 8 5 13t13 5h161q8 0 13-5t5-13z", "order", "17", "color", "#924E7D")]
        Kesif = 6,
        [Description("Diğer"), Generic("icon", "icon-hashtag", "iconsvg", "M553 279l36 142h-142l-36-142h142z m429 281l-32-125q-4-14-17-14h-182l-36-142h173q9 0 14-7 6-8 4-16l-32-125q-2-13-17-13h-182l-45-183q-4-14-18-14h-125q-9 0-14 7-5 7-4 16l44 174h-142l-45-183q-4-14-17-14h-126q-8 0-14 7-5 7-3 16l43 174h-173q-9 0-14 7-5 6-4 15l32 125q4 14 17 14h182l36 142h-173q-9 0-14 7-6 8-4 16l32 125q2 13 17 13h182l46 183q3 14 17 14h125q9 0 14-7 5-7 4-16l-44-174h142l45 183q4 14 18 14h125q8 0 14-7 5-7 3-16l-43-174h173q9 0 14-7 5-6 4-15z", "order", "20", "color", "#fff200")]
        Diger = 7,
        [Description("Talep"), Generic("icon", "icon-logout-3", "iconsvg", "M0 84q0-84 59-143t144-60h72q33 0 57 24t23 57-23 57-57 23h-72q-18 0-30 13t-12 29v532q0 17 12 29t30 13h72q33 0 57 23t23 57-23 57-57 23h-72q-84 0-144-59t-59-143v-532z m238 252q0 33 24 57t57 23h344l-110 111q-24 23-24 57t24 57q23 23 57 23t56-23l248-248q23-24 23-57t-23-57l-243-242q-23-23-57-23t-57 23q-23 23-23 57t24 57l105 105h-344q-33 0-57 23t-24 57z", "order", "12", "color", "#f1f49f")]
        Talep = 8,
        [Description("Refakat"), Generic("icon", "ii-kisiler", "iconsvg", "M237.227 688.95c8.875-6.765 21.535-5.058 28.331 3.817 59.578 78.103 149.969 122.88 247.994 122.88s188.416-44.777 248.025-122.88c4.003-5.213 9.992-7.944 16.105-7.944 4.251 0 8.564 1.334 12.226 4.127 8.875 6.796 10.581 19.456 3.817 28.331-67.305 88.188-169.425 138.768-280.173 138.768-110.716 0-212.837-50.579-280.142-138.768-6.765-8.875-5.058-21.535 3.817-28.331zM728.343 581.182c0-34.816 28.331-63.116 63.116-63.116 34.816 0 63.116 28.3 63.116 63.116 0 34.785-28.3 63.085-63.116 63.085-34.785 0-63.116-28.3-63.116-63.085zM856.064 497.121c-0.155 0.062-16.074 4.934-16.074 4.934-5.741 1.738-12.009 0.683-16.818-2.948l-31.713-23.738-31.682 23.738c-4.841 3.631-11.109 4.686-16.849 2.948 0 0-15.919-4.872-16.043-4.934-25.6-8.533-42.791-32.396-42.791-59.361v-95.387c0-3.693 1.086-7.292 3.103-10.364l31.992-48.687v-127.876c0-10.426 8.44-18.866 18.866-18.866h106.806c10.426 0 18.866 8.44 18.866 18.866v127.876l31.992 48.687c2.017 3.072 3.103 6.672 3.103 10.364v95.387c0 26.965-17.191 50.828-42.76 59.361zM235.675 518.066c34.785 0 63.085 28.3 63.085 63.116 0 34.785-28.3 63.085-63.085 63.085-34.816 0-63.116-28.3-63.116-63.085 0-34.816 28.3-63.116 63.116-63.116zM300.249 497.121c-0.155 0.062-16.043 4.934-16.043 4.934-5.772 1.738-12.009 0.683-16.849-2.948l-31.682-23.738-31.713 23.738c-4.841 3.631-11.078 4.686-16.849 2.948 0 0-15.888-4.872-16.043-4.934-25.569-8.533-42.76-32.396-42.76-59.361v-95.387c0-3.693 1.086-7.292 3.103-10.364l31.992-48.687v-127.876c0-10.426 8.44-18.866 18.866-18.866h106.806c10.395 0 18.835 8.44 18.835 18.866v127.876l32.023 48.687c2.017 3.072 3.103 6.672 3.103 10.364v95.387c0 26.965-17.222 50.828-42.791 59.361zM594.975 670.115c0 44.87-36.523 81.392-81.424 81.392-44.87 0-81.392-36.523-81.392-81.392 0-44.901 36.523-81.392 81.392-81.392 44.901 0 81.424 36.492 81.424 81.392zM602.981 549.283l-0.155 0.031-23.211 7.106c-3.693 1.148-7.602-0.807-8.906-4.406l-48.842-134.051c-2.824-7.727-13.777-7.727-16.57 0l-48.873 134.051c-1.055 2.917-3.817 4.748-6.765 4.748-0.714 0-25.352-7.447-25.352-7.447-29.665-9.868-49.493-37.392-49.493-68.515v-141.777c0-1.427 0.403-2.793 1.179-3.972l50.983-77.576v-196.267c0-4.003 3.227-7.23 7.23-7.23h158.72c4.003 0 7.23 3.227 7.23 7.23v196.267l50.952 77.576c0.776 1.179 1.21 2.544 1.21 3.972v142.15c0 30.968-19.921 58.461-49.338 68.112zM532.635 552.107c-1.986 2.172-4.965 3.289-7.944 3.289h-22.249c-3.010 0-5.958-1.117-7.975-3.289-3.134-3.413-3.568-8.347-1.365-12.195l11.916-17.936-5.585-47.011 10.985-29.168c1.055-2.948 5.213-2.948 6.299 0l10.954 29.168-5.554 47.011 11.916 17.936c2.203 3.848 1.738 8.782-1.396 12.195z", "order", "18", "color", "#20f747")]
        Refakat = 9,
        [Description("Şikayet"), Generic("icon", "icon-warning", "iconsvg", "M991 186q64-107 12-199-52-93-177-93l-625 0q-126 0-177 93-53 92 11 199l309 513q62 107 170 107 106 0 169-107z m-477-112q34 0 57 23t23 57-23 58-57 24-58-24-23-58 23-57 58-23z m85 387q5 13 5 32 0 38-26 64t-64 27-65-27-26-64q0-17 6-34l72-178q5-9 13-9t11 9q74 178 74 180z", "order", "19", "color", "#721422")]
        Şikayet = 10,
        [Description("Denetim"), Generic("icon", "icon-retweet-3", "iconsvg", "M714 11q0-7-5-13t-13-5h-535q-5 0-8 1t-5 4-3 4-2 7 0 6v335h-107q-15 0-25 11t-11 25q0 13 8 23l179 214q11 12 27 12t28-12l178-214q9-10 9-23 0-15-11-25t-25-11h-107v-214h321q9 0 14-6l89-108q4-6 4-11z m357 232q0-13-8-23l-178-214q-12-13-28-13t-27 13l-179 214q-8 10-8 23 0 14 11 25t25 11h107v214h-322q-9 0-14 7l-89 107q-4 5-4 11 0 7 5 12t13 6h536q4 0 7-1t5-4 3-5 2-6 1-7v-334h107q14 0 25-11t10-25z", "order", "20", "color", "#F44611")]
        Soktak = 11,
    }

    [EnumInfo(typeof(FTM_Task), "priority")]
    public enum EnumFTM_TaskPriority
    {
        [Description("Yüksek"), Generic("color", "EF5352")]
        Yuksek = 0,
        [Description("Orta"), Generic("color", "F8AC59")]
        Orta = 1,
        [Description("Düşük"), Generic("color", "1ab394")]
        Dusuk = 2,
    }

    [EnumInfo(typeof(VWFTM_Task), "SLAStatus")]
    public enum EnumFTM_TaskSLAStatus
    {
        [Description("Vaktinde Tamamlananlar"), KeyValue("color", "rgb(26 179 148)")]
        Tamamlananlar = 0,
        [Description("Süresi Yaklaşanlar"), KeyValue("color", "rgb(248 172 89)")]
        Yaklasanlar = 1,
        [Description("Cezaya Girenler"), KeyValue("color", "rgb(237 85 101)")]
        Asanlar = 2,
        [Description("Tanımsızlar"), KeyValue("color", "rgb(102 102 102)")]
        Tanimsiz = 3,
    }

    [EnumInfo(typeof(VWFTM_Task), "planLater")]
    public enum EnumFTM_TaskPlanLater
    {
        [Description("Hayır"), Generic("order", "2")]
        Hayir = 0,
        [Description("Evet"), Generic("order", "1")]
        Evet = 1
    }

    [EnumInfo(typeof(VWFTM_Task), "isSendDocuments")]
    public enum EnumFTM_TaskPersonIsSendDocuments
    {
        [Description("Hayır"), Generic("order", "2")]
        Hayır = 0,
        [Description("Evet"), Generic("order", "1")]
        Evet = 1
    }
    [EnumInfo(typeof(VWFTM_Task), "sendMailCustomer")]
    public enum EnumFTM_TaskPersonSendMailCustomer
    {
        [Description("Hayır"), Generic("order", "2")]
        Hayır = 0,
        [Description("Evet"), Generic("order", "1")]
        Evet = 1
    }

    partial class WorkOfTimeDatabase
    {


        public FTM_Task[] GetFTM_TaskByFixtureIds(Guid[] productIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_Task>().Where(a => a.fixtureId.In(productIds)).Select(a => new FTM_Task { id = a.id, fixtureId = a.fixtureId, }).Execute().ToArray();
            }
        }

        public FTM_Task[] GetFTM_TaskByCustomerId(Guid customerId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_Task>().Where(a => a.customerId == customerId).Execute().ToArray();
            }
        }

        public FTM_Task[] GetFTM_TaskByCustomerIds(Guid[] customerIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_Task>().Where(a => a.customerId.In(customerIds)).Execute().ToArray();
            }
        }

        public Guid[] GetFTM_TaskByCreatedBy(Guid createdBy, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_Task>().Where(a => a.createdby == createdBy).Execute().Select(a=>a.id).ToArray();
            }
        }


        public object GetFTM_TaskLocationCenterPointByLocation(string location, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var test = @"DECLARE @g geometry;  SET @g = geometry::STGeomFromText('"+location+"', 0); SELECT @g.STCentroid()";
                
                return db.ExecuteReader(test).FirstOrDefault();
            }
        }

    }
}
