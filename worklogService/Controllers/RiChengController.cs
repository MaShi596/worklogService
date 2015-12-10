using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using worklogService.CommonMethod;
using worklogService.DBoperate;
using worklogService.Models;
using worklogService.Models.CommunicationClass;



namespace worklogService.Controllers
{
    public class RiChengController : ApiController
    {
        //public class RiChenginfo
        //{
        //    string richengId;
        //    public string RichengId
        //    {
        //        get { return richengId; }
        //        set { richengId = value; }
        //    }
        //    string richengContent;
        //    public string RichengContent
        //    {
        //        get { return richengContent; }
        //        set { richengContent = value; }
        //    }
        //    string personName;
        //    public string PersonName
        //    {
        //        get { return personName; }
        //        set { personName = value; }
        //    }
        //    string personId;
        //    public string PersonId
        //    {
        //        get { return personId; }
        //        set { personId = value; }
        //    }
        //    string personDeptName;
        //    public string PersonDeptName
        //    {
        //        get { return personDeptName; }
        //        set { personDeptName = value; }
        //    }
        //    string richengTime;
        //    public string RichengTime
        //    {
        //        get { return richengTime; }
        //        set { richengTime = value; }
        //    }
        //    string richengSub;

        //    public string RichengSub
        //    {
        //        get { return richengSub; }
        //        set { richengSub = value; }
        //    }

        //}
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
            string personDeptName;
            public string PersonDeptName
            {
                get { return personDeptName; }
                set { personDeptName = value; }
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
            string richengContent;

            string remindTime;
            public string RemindTime
            {
                get { return remindTime; }
                set { remindTime = value; }
            }
            public string RichengContent
            {
                get { return richengContent; }
                set { richengContent = value; }
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

            string richengSub;

            public string RichengSub
            {
                get { return richengSub; }
                set { richengSub = value; }
            }

            string arrangeManId;

            public string ArrangeManId
            {
                get { return arrangeManId; }
                set { arrangeManId = value; }
            }

            string timeTick;

            public string TimeTick
            {
                get { return timeTick; }
                set { timeTick = value; }
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

        /// <summary>
        /// 获得所有的日程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rctime"></param>
        /// <returns></returns>
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
                    ric.PersonDeptName = user.Kdid.KdName.ToString().Trim();
                    ric.RichengSub = rc[4].ToString();
                    ric.ArrangeManId = rc[10].ToString();
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

        /// <summary>
        /// 获得自己的日程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rctime"></param>
        /// <returns></returns>
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
                    ric.PersonDeptName = user.Kdid.KdName.ToString().Trim();
                    ric.RichengSub = rc[4].ToString();
                    ric.ArrangeManId = rc[10].ToString();
                    ric.TimeTick = rc[3].ToString();
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

        /// <summary>
        /// 获得分享的日程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rctime"></param>
        /// <returns></returns>
        public HttpResponseMessage GetShareRiCheng(string userid, string rctime)
        {
            string res = null;
            string sqlstr = null;
            if (rctime == "0")
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from dbo.RiCheng where  ScheduleTime > " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from dbo.RiCheng where  ScheduleTime < " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.ExecuteSQL(sqlstr);
            List<RiChenginfo> info = new List<RiChenginfo>();
            if (nbhstaff.Count > 0 && nbhstaff != null)
            {
                for (int i = 0; i < nbhstaff.Count; i++)
                {
                    object[] rc = (object[])nbhstaff[i];
                    RiChenginfo ric = new RiChenginfo();
                    ric.RichengId = rc[9].ToString();//日程Id
                    ric.PersonId = rc[2].ToString();//分享人Id
                    ric.RichengContent = rc[5].ToString();//日程内容
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[4])).ToString("yyyy年MM月dd日 HH:mm");//分享时间
                    ric.RemindTime = new DateTime(Convert.ToInt64(rc[7])).ToString("yyyy年MM月dd日 HH:mm");//提醒时间
                    WkTUser user = new WkTUser();
                    user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[3]));
                    ric.PersonName = user.KuName;//分享人姓名
                    ric.PersonDeptName = user.Kdid.KdName.ToString();//分享人部门
                    ric.RichengSub = rc[6].ToString();//日程主题
                    
                    ric.Logtick = rc[4].ToString();
                    ric.TimeTick = rc[4].ToString();
                    ric.ArrangeManId = rc[11].ToString();
                    //ric.Contenttxt140 = HtmlToReguFormat140(rc[2].ToString());//日志内容去格式前140
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

        
        /// <summary>
        /// 上下级查看分享
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="seeid"></param>
        /// <param name="rctime"></param>
        /// <returns></returns>
        public HttpResponseMessage GetRiCheng2(string userid, string seeid, string rctime)
        {
            string res = "";
            string sql1 = "select u from WkTUser u where u.KuName='" + userid + "'";
            string sql2 = "select u from WkTUser u where u.Id='" + seeid + "'";
            string sql3 = "select u.DeptId from Wktuser_M_Dept u where u.WktuserId=" + userid + " and u.State = " + (int)IEntity.stateEnum.Normal;
            BaseService baseservice = new BaseService();
            List<WkTDept> theDepts = new List<WkTDept>();
            IList theone = baseservice.loadEntityList(sql3);
            if (theone != null && theone.Count > 0)
            {
                List<RiChenginfo> rlist = new List<RiChenginfo>();
                WkTUser user = new WkTUser();
                user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(seeid));
                string m = user.Kdid.Id.ToString();
                int flog = 1;
                for (int i = 0; i < theone.Count; i++)
                {

                    string a = "";
                    a = ((WkTDept)theone[i]).Id.ToString();
                    if (a == m)
                    {
                        flog = 0;
                        string sqlstr = "";
                        if (rctime == "0")
                        {
                            sqlstr = "with cte as " +
                                        "( " +
                                        " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where WktUserId=" + seeid.ToString() + " and ScheduleTime > " + rctime.ToString() +
                                        ") " +
                                        " select * from cte where row between " + "1" + " and " + "10";
                        }
                        else
                        {
                            sqlstr = "with cte as " +
                                        "( " +
                                        " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where WktUserId=" + seeid.ToString() + " and ScheduleTime < " + rctime.ToString() +
                                        ") " +
                                        " select * from cte where row between " + "1" + " and " + "10";
                        }
                        IList one = baseservice.ExecuteSQL(sqlstr);

                        if (one != null && one.Count > 0)
                        {
                            for (int j = 0; j < one.Count; j++)
                            {
                                object[] sf = (object[])one[j];
                                RiChenginfo st = new RiChenginfo();
                                //// Personinfo p = new Personinfo();
                                st.PersonId = sf[6].ToString();//分享人的ID

                                WkTUser user1 = new WkTUser();
                                user1 = (WkTUser)baseservice.loadEntity(user1, Convert.ToInt64(st.PersonId));
                                st.PersonName = user1.KuName;//分享人的姓名
                                //long mm = user1.Kdid.Id;
                                WkTDept dept = new WkTDept();
                                dept = (WkTDept)baseservice.loadEntity(dept, Convert.ToInt64(m));
                                st.PersonDeptName = dept.KdName;//分享人的部门
                                st.Logtick = sf[3].ToString();
                                st.RichengTime = new DateTime(Convert.ToInt64(sf[3].ToString())).ToString("yyyy年MM月dd日 HH:mm");//日程时间
                                st.RichengId = sf[1].ToString(); //日程id
                                st.RemindTime = new DateTime(Convert.ToInt64(sf[9].ToString())).ToString("yyyy年MM月dd日 HH:mm");//提醒时间
                                st.RichengContent=sf[2].ToString();//日程内容
                                st.RichengSub=sf[4].ToString();//日程主题
                                st.ArrangeManId = sf[10].ToString();
                                st.TimeTick = sf[3].ToString();

                                rlist.Add(st);
                            }
                        }
                    }
                }
                RiChengAll l = new RiChengAll();
                l.List = rlist;
                res = "成功";
                string data = JsonTools.ObjectToJson(l);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;

                if (flog == 1)
                {
                    return RiChengYo(userid, seeid, rctime);

                }
            }
            else
            {
                return RiChengYo(userid, seeid, rctime);

            }
        }

        public HttpResponseMessage RiChengYo(string userid,string seeid,string rctime)
        {
            string res = null;
            string sqlstr = null;
            if (rctime == "0")
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from dbo.RiCheng where  ScheduleTime > " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +"and SharePersonId="+seeid.ToString()+
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from dbo.RiCheng where  ScheduleTime < " + rctime.ToString() + "and WkTUserId=" + userid.ToString() + "and SharePersonId=" + seeid.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.ExecuteSQL(sqlstr);
            List<RiChenginfo> info = new List<RiChenginfo>();
            if (nbhstaff.Count > 0 && nbhstaff != null)
            {
                for (int i = 0; i < nbhstaff.Count; i++)
                {
                    object[] rc = (object[])nbhstaff[i];
                    RiChenginfo ric = new RiChenginfo();
                    ric.RichengId = rc[9].ToString();//日程Id
                    ric.PersonId = rc[2].ToString();//分享人Id
                    ric.RichengContent = rc[5].ToString();//日程内容
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[4])).ToString("yyyy年MM月dd日 HH:mm");//分享时间
                    ric.RemindTime = new DateTime(Convert.ToInt64(rc[7])).ToString("yyyy年MM月dd日 HH:mm");//提醒时间
                    WkTUser user = new WkTUser();
                    user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[3]));
                    ric.PersonName = user.KuName;//分享人姓名
                    ric.PersonDeptName = user.Kdid.KdName.ToString();//分享人部门
                    ric.RichengSub = rc[6].ToString();//日程主题

                    ric.Logtick = rc[4].ToString();
                    //ric.Contenttxt140 = HtmlToReguFormat140(rc[2].ToString());//日志内容去格式前140

                    ric.TimeTick = rc[4].ToString();
                    ric.ArrangeManId = rc[11].ToString();
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
