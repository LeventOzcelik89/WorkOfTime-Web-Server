﻿using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Composition;
using System.Web;
using Infoline.WorkOfTime.BusinessAccess;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class CalendarHandler : BaseSmartHandler
    {
        public CalendarHandler()
            : base("CalendarHandler")
        {

        }

        [HandleFunction("Calendar/GetCalendar")]
        public void CalendarGetCalendar(HttpContext context)
        {
            try
            {
                var start = context.Request["start"];
                var end = context.Request["end"];
                var email = context.Request["email"];
                var listData = new List<MyCalendar>();
                var models = new CalendarModel().Load(CallContext.Current.UserId, Convert.ToDateTime(start), Convert.ToDateTime(end), null, email);
                foreach (var model in models)
                {
                    var fark = (model.end.Value - model.start.Value).TotalDays;
                    if (fark > 1)
                    {

                        var timeCounter = new DateTime(model.start.Value.Year, model.start.Value.Month, model.start.Value.Day);
                        while (timeCounter <= new DateTime(model.end.Value.Year, model.end.Value.Month, model.end.Value.Day))
                        {
                            var first = false;
                            if (model.start.Value.Year == timeCounter.Year && model.start.Value.Month == timeCounter.Month && model.start.Value.Day == timeCounter.Day)
                            {
                                first = true;
                            }
                            listData.Add(new MyCalendar
                            {
                                color = model.color,
                                createdby = model.createdby,
                                description = model.description,
                                id = model.id,
                                katilimcilar = model.katilimcilar,
                                title = model.title,
                                type = model.type,
                                start = new DateTime(timeCounter.Year, timeCounter.Month, timeCounter.Day, (first == true ? model.start.Value.Hour : 0), (first == true ? model.start.Value.Minute : 0), (first == true ? model.start.Value.Second : 0)),
                                end = (timeCounter == new DateTime(model.end.Value.Year, model.end.Value.Month, model.end.Value.Day) ? model.end : null)
                            });
                            timeCounter = timeCounter.AddDays(1);
                        }
                    }
                    else
                    {
                        listData.Add(new MyCalendar
                        {
                            color = model.color,
                            createdby = model.createdby,
                            description = model.description,
                            end = model.end,
                            id = model.id,
                            katilimcilar = model.katilimcilar,
                            start = model.start,
                            title = model.title,
                            type = model.type
                        });
                    }

                }
                var res = listData.GroupBy(x => x.start.Value.ToString("yyyy-MM-dd")).ToDictionary(a => a.Key, b =>
                   b.Where(x => x.start.Value.ToShortDateString() == b.Select(f => f.start.Value.ToShortDateString()).FirstOrDefault()).ToArray()
                );


                RenderResponse(context, res);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }
    }
}