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
            string richengId;
            public string RichengId
            {
                get { return richengId; }
                set { richengId = value; }
            }
            string richengContent;
            public string RichengContent
            {
                get { return richengContent; }
                set { richengContent = value; }
            }
            string personName;
            public string PersonName
            {
                get { return personName; }
                set { personName = value; }
            }
            string personId;
            public string PersonId
            {
                get { return personId; }
                set { personId = value; }
            }
            string personDeptName;
            public string PersonDeptName
            {
                get { return personDeptName; }
                set { personDeptName = value; }
            }
            string richengTime;
            public string RichengTime
            {
                get { return richengTime; }
                set { richengTime = value; }
            }
            string richengSub;

            public string RichengSub
            {
                get { return richengSub; }
                set { richengSub = value; }
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
        public HttpResponseMessage GetAllRiCheng(string userid, string rctime)
        {
            string sqlstr = null;
            if (rctime == "0")
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where  ScheduleTime > " + rctime.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where  ScheduleTime < " + rctime.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            string res = "";
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.ExecuteSQL(sqlstr);
            List<RiChenginfo> info = new List<RiChenginfo>();
            if (nbhstaff.Count > 0 && nbhstaff != null)
            {
                for (int i = 0; i < nbhstaff.Count; i++)
                {
                    object[] rc = (object[])nbhstaff[i];
                    RiChenginfo ric = new RiChenginfo();

                    ric.RichengId = rc[1].ToString();
                    ric.PersonId = rc[6].ToString();
                    ric.RichengContent = rc[2].ToString();
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[3])).ToString("yyyy年MM月dd日 HH:mm");
                    WkTUser user = new WkTUser();
                    user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[6]));
                    ric.PersonName = user.KuName;
                    ric.PersonDeptName = user.Kdid.KdName.ToString();
                    ric.RichengSub = rc[4].ToString();
                    info.Add(ric);
                }

                RiChengAll l = new RiChengAll();
                l.List = info;
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

        public HttpResponseMessage GetOwnRiCheng(string userid, string rctime)
        {
            string sqlstr = null;
            if (rctime == "0")
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where  ScheduleTime > " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where  ScheduleTime < " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            string res = "";
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.ExecuteSQL(sqlstr);
            List<RiChenginfo> info = new List<RiChenginfo>();
            if (nbhstaff.Count > 0 && nbhstaff != null)
            {
                for (int i = 0; i < nbhstaff.Count; i++)
                {
                    object[] rc = (object[])nbhstaff[i];
                    RiChenginfo ric = new RiChenginfo();

                    ric.RichengId = rc[1].ToString();
                    ric.PersonId = rc[6].ToString();
                    ric.RichengContent = rc[2].ToString();
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[3])).ToString("yyyy年MM月dd日 HH:mm");
                    WkTUser user = new WkTUser();
                    user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[6]));
                    ric.PersonName = user.KuName;
                    ric.PersonDeptName = user.Kdid.KdName.ToString();
                    ric.RichengSub = rc[4].ToString();
                    info.Add(ric);
                }

                RiChengAll l = new RiChengAll();
                l.List = info;
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
