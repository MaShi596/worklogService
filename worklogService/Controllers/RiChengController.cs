using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using worklogService.CommonMethod;
using worklogService.DBoperate;
using worklogService.Models;
using worklogService.Models.CommunicationClass;

namespace worklogService.Controllers
{
    public class RiChengController : ApiController
    {
        public class RiChenginfo
        {
            string personId;
            public string PersonId
            {
                get { return personId; }
                set { personId = value; }
            }
            string personName;
            public string PersonName
            {
                get { return personName; }
                set { personName = value; }
            }
            string personDept;
            public string PersonDept
            {
                get { return personDept; }
                set { personDept = value; }
            }
            string richengId;
            public string RichengId
            {
                get { return richengId; }
                set { richengId = value; }
            }
            string contenttxt140;
            public string Contenttxt140
            {
                get { return contenttxt140; }
                set { contenttxt140 = value; }
            }
            string richengTime;
            public string RichengTime
            {
                get { return richengTime; }
                set { richengTime = value; }
            }
            string contenttxtAll;
            public string ContenttxtAll
            {
                get { return contenttxtAll; }
                set { contenttxtAll = value; }
            }
            string logtick;
            public string Logtick
            {
                get { return logtick; }
                set { logtick = value; }
            }
            string personMD5code;
            public string PersonMD5code
            {
                get { return personMD5code; }
                set { personMD5code = value; }
            }
        }
        public class RiChengAll
        {
            List<RiChenginfo> list;
            public List<RiChenginfo> List
            {
                get { return list; }
                set { list = value; }
            }
        }
        public HttpResponseMessage GetRiCheng(string userid)
        {
            string res = "";
            long thisDay = DateTime.Now.Date.Ticks;
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.loadEntityList("from StaffSchedule where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + userid + " and ScheduleTime>=" + thisDay + " order by ScheduleTime asc");
            if (nbhstaff.Count > 0 && nbhstaff != null)
            {
                List<RiChenginfo> staff = new List<RiChenginfo>();
                foreach(StaffSchedule n in nbhstaff)
                {
                    RiChenginfo ri = new RiChenginfo();
                    ri.RichengId = n.Id.ToString();
                    ri.PersonId = n.Staff.Id.ToString();
                    ri.PersonName = n.Staff.KuName;
                    ri.PersonMD5code = n.Staff.ImgMD5Code;
                    ri.RichengTime = new DateTime(n.ScheduleTime).ToString("yyyy年MM月dd日 HH:mm");
                    ri.ContenttxtAll = n.Content;
                    
                    staff.Add(ri);
                }
                RiChengAll l = new RiChengAll();
                l.List =staff;
                res = "成功";
                string data = JsonTools.ObjectToJson(l);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
            else
            {
                res = "没有内容";
                string data = "1";//JsonTools.ObjectToJson(l);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
        }
    }
}
