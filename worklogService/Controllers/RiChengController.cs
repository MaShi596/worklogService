using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Script.Serialization;
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

            string doState;

            public string DoState
            {
                get { return doState; }
                set { doState = value; }
            }
            string isRemind;

            public string IsRemind
            {
                get { return isRemind; }
                set { isRemind = value; }
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
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[3])).ToString("yyyy年MM月dd日 HH:mm:ss");
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





        public class RCDayBag
        {


            public RCDayBag()
            {
                RichengDayList = new List<RiChenginfo>();
            }
            string dayString;

            public string DayString
            {
                get { return dayString; }
                set { dayString = value; }
            }
            string dayTick;

            public string DayTick
            {
                get { return dayTick; }
                set { dayTick = value; }
            }
            List<RiChenginfo> richengDayList;

            public List<RiChenginfo> RichengDayList
            {
                get { return richengDayList; }
                set { richengDayList = value; }
            }


        
        }

        public class RCDayBagList
        {


            public RCDayBagList()
            {

                list = new List<RCDayBag>();
            
            }
            List<RCDayBag> list;

            public List<RCDayBag> List
            {
                get { return list; }
                set { list = value; }
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
            string res = "";
            string data = "1";
            
            BaseService baseservice = new BaseService();
            //List<RCDayBag> list = new List<RCDayBag>();
            RCDayBagList rrll = new RCDayBagList();

            long startick;
            long endtick;


            if (rctime == "0")
            {
                string fstr = "select top 1 LOG_T_STAFFSCHEDULE.ScheduleTime  from LOG_T_STAFFSCHEDULE  where WkTUserId = " + userid + " and STATE = 0 order by LOG_T_STAFFSCHEDULE.ScheduleTime desc";

                IList firres = baseservice.ExecuteSQL(fstr);

                object[] obj; 
                if (firres != null && firres.Count > 0)
                {
                    obj  = (object[])firres[0];
                    DateTime d = new DateTime(long.Parse(obj[0].ToString()));
                    string str = d.ToString("yyyy-MM-dd");
                    long daytick = d.Date.Ticks;
                    startick = d.AddDays(1).Ticks;//daytick;
                    endtick = d.Date.Ticks;

                    for (int i = 0; i < 10; i++)
                    {


                        RCDayBag rdb1 = new RCDayBag();
                        rdb1.DayString = str;
                        rdb1.DayTick = daytick.ToString();


                        string sql1 = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by LOG_T_STAFFSCHEDULE.ScheduleTime DESC ), * from LOG_T_STAFFSCHEDULE where  ScheduleTime < " + startick.ToString() + " and ScheduleTime > " + endtick.ToString() + "  and  WkTUserId=" + userid.ToString() +
                            ") " +
                            " select * from cte ";
                        //where row between " + "1" + " and " + "10";

                        IList nbhstaff1 = baseservice.ExecuteSQL(sql1);

                        //List<RiChenginfo> info = new List<RiChenginfo>();
                        if (nbhstaff1.Count > 0 && nbhstaff1 != null)
                        {
                            long tick2 = 0;
                            for (int i1 = 0; i1 < nbhstaff1.Count; i1++)
                            {
                                object[] rc = (object[])nbhstaff1[i1];
                                RiChenginfo ric = new RiChenginfo();

                                ric.RichengId = rc[1].ToString();
                                ric.PersonId = rc[6].ToString();
                                ric.RichengContent = rc[2].ToString();
                                ric.RichengTime = new DateTime(Convert.ToInt64(rc[3])).ToString("yyyy年MM月dd日 HH:mm:ss");
                                WkTUser user = new WkTUser();
                                user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[6]));
                                ric.PersonName = user.KuName;
                                ric.PersonDeptName = user.Kdid.KdName.ToString().Trim();
                                ric.RichengSub = rc[4].ToString();
                                ric.ArrangeManId = rc[10].ToString();
                                ric.TimeTick = rc[3].ToString();
                                ric.DoState = getDostate(rc[3].ToString(), rc[11]);//rc[11] == null ? rc[11].ToString() : "null";
                                ric.IsRemind = rc[5].ToString();
                               
                                    ric.RemindTime = new DateTime(Convert.ToInt64(rc[9])).ToString("yyyy年MM月dd日 HH:mm:ss");
                                
 
                                
                               
                                
                                rdb1.RichengDayList.Add(ric);
                                
                                //info.Add(ric);
                                tick2 = long.Parse(rc[3].ToString());

                            }
                            rrll.List.Add(rdb1);
                            //list.Add(rdb1);
                            //rdb1.RichengDayList = info;
                            string str2 = "select top 1 LOG_T_STAFFSCHEDULE.ScheduleTime  from LOG_T_STAFFSCHEDULE  where WkTUserId = " + userid + " and STATE = 0 and ScheduleTime < " + tick2.ToString() + " order by LOG_T_STAFFSCHEDULE.ScheduleTime desc";
                            IList firtick2 = baseservice.ExecuteSQL(str2);
                           
                            if (firtick2 != null && firtick2.Count > 0)
                            {
                                object[] obj2 = (object[])firtick2[0];
                                DateTime d2 = new DateTime(long.Parse(obj2[0].ToString()));

                                str = d2.ToString("yyyy-MM-dd");
                                daytick = d2.Date.Ticks;
                                startick = d2.AddDays(1).Ticks;
                                endtick = daytick;// d2.AddDays(-1).Ticks;


                            }
                            else
                            {
                                break;

                            }

                            


                        }
                        else
                        {

                            break;

                        }

                       

                    }

                    res = "成功";
                    //RCDayBagList rl = new RCDayBagList();
                    //rl.List = list;
                    data = JsonTools.ObjectToJson(rrll);



                }
                else
                {

                    res = "没有内容";
                    data = "1";

                }

                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };

                return result;
            }
            else
            {

               
               

                DateTime d = new DateTime(long.Parse(rctime));
                string str = d.ToString("yyyy-MM-dd");

                long daytick = 0;//d.Date.Ticks;
                startick = 0;// d.AddDays(1).Ticks;//daytick;
                endtick = 0;// d.Date.Ticks;




                string str3 = "select top 1 LOG_T_STAFFSCHEDULE.ScheduleTime  from LOG_T_STAFFSCHEDULE  where WkTUserId = " + userid + " and STATE = 0 and ScheduleTime < " + rctime.ToString() + " order by LOG_T_STAFFSCHEDULE.ScheduleTime desc";
                IList firtick3 = baseservice.ExecuteSQL(str3);

                if (firtick3 != null && firtick3.Count > 0)
                {
                    object[] obj3 = (object[])firtick3[0];

                    DateTime d3 = new DateTime(long.Parse(obj3[0].ToString()));

                    str = d3.ToString("yyyy-MM-dd");
                    daytick = d3.Date.Ticks;
                    startick = d3.AddDays(1).Ticks;
                    endtick = daytick;// d2.AddDays(-1).Ticks;
                }
                else
                {

                    res = "没有内容";
                    data = "1";

                    var jsonStr1 = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                    var result1 = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(jsonStr1, Encoding.UTF8, "text/json")
                    };

                    return result1;
                
                }




                for (int i = 0; i < 10; i++)
                {


                    RCDayBag rdb1 = new RCDayBag();
                    rdb1.DayString = str;
                    rdb1.DayTick = daytick.ToString();


                    string sql1 = "with cte as " +
                        "( " +
                        " select row=row_number()over(order by LOG_T_STAFFSCHEDULE.ScheduleTime DESC ), * from LOG_T_STAFFSCHEDULE where  ScheduleTime < " + startick.ToString() + " and ScheduleTime > " + endtick.ToString() + "  and  WkTUserId=" + userid.ToString() +
                        ") " +
                        " select * from cte ";
                    //where row between " + "1" + " and " + "10";

                    IList nbhstaff1 = baseservice.ExecuteSQL(sql1);

                    //List<RiChenginfo> info = new List<RiChenginfo>();
                    if (nbhstaff1.Count > 0 && nbhstaff1 != null)
                    {
                        long tick2 = 0;
                        for (int i1 = 0; i1 < nbhstaff1.Count; i1++)
                        {
                            object[] rc = (object[])nbhstaff1[i1];
                            RiChenginfo ric = new RiChenginfo();

                            ric.RichengId = rc[1].ToString();
                            ric.PersonId = rc[6].ToString();
                            ric.RichengContent = rc[2].ToString();
                            ric.RichengTime = new DateTime(Convert.ToInt64(rc[3])).ToString("yyyy年MM月dd日 HH:mm:ss");
                            WkTUser user = new WkTUser();
                            user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[6]));
                            ric.PersonName = user.KuName;
                            ric.PersonDeptName = user.Kdid.KdName.ToString().Trim();
                            ric.RichengSub = rc[4].ToString();
                            ric.ArrangeManId = rc[10].ToString();
                            ric.DoState = getDostate(rc[3].ToString(), rc[11]);//rc[11]==null?rc[11].ToString():"null";
                            ric.TimeTick = rc[3].ToString();
                            //info.Add(ric);
                            rdb1.RichengDayList.Add(ric);

                            tick2 = long.Parse(rc[3].ToString());

                        }
                        //rdb1.RichengDayList = info;
                        rrll.List.Add(rdb1);
                        string str2 = "select top 1 LOG_T_STAFFSCHEDULE.ScheduleTime  from LOG_T_STAFFSCHEDULE  where WkTUserId = " + userid + " and STATE = 0 and ScheduleTime < " + tick2.ToString() + " order by LOG_T_STAFFSCHEDULE.ScheduleTime desc";
                        IList firtick2 = baseservice.ExecuteSQL(str2);
                       
                        if (firtick2 != null && firtick2.Count > 0)
                        {
                            object[] obj = (object[])firtick2[0];
                            DateTime d2 = new DateTime(long.Parse(obj[0].ToString()));

                            str = d2.ToString("yyyy-MM-dd");
                            daytick = d2.Date.Ticks;
                            startick = d2.AddDays(1).Ticks;
                            endtick = daytick;// d2.AddDays(-1).Ticks;


                        }
                        else
                        {
                            break;

                        }

                    }
                    else
                    {

                        break;

                    }

                    //list.Add(rdb1);

                }

                res = "成功";
                //RCDayBagList rl = new RCDayBagList();
                //rl.List = list;
                data = JsonTools.ObjectToJson(rrll);



                //}
                //else
                //{

                //    res = "没有内容";
                //    data = "1";

                //}

                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };

                return result;

            }
            




            //string sqlstr = null;
            //if (rctime == "0")
            //{
            //    sqlstr = "with cte as " +
            //                "( " +
            //                " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where  ScheduleTime > " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +
            //                ") " +
            //                " select * from cte where row between " + "1" + " and " + "10";
            //}
            //else
            //{
            //    sqlstr = "with cte as " +
            //                "( " +
            //                " select row=row_number()over(order by getdate()), * from LOG_T_STAFFSCHEDULE where  ScheduleTime < " + rctime.ToString() + "and WkTUserId=" + userid.ToString() +
            //                ") " +
            //                " select * from cte where row between " + "1" + " and " + "10";
            //}
           
          
            //IList nbhstaff = baseservice.ExecuteSQL(sqlstr);
            //List<RiChenginfo> info = new List<RiChenginfo>();
            //if (nbhstaff.Count > 0 && nbhstaff != null)
            //{
            //    for (int i = 0; i < nbhstaff.Count; i++)
            //    {
            //        object[] rc = (object[])nbhstaff[i];
            //        RiChenginfo ric = new RiChenginfo();

            //        ric.RichengId = rc[1].ToString();
            //        ric.PersonId = rc[6].ToString();
            //        ric.RichengContent = rc[2].ToString();
            //        ric.RichengTime = new DateTime(Convert.ToInt64(rc[3])).ToString("yyyy年MM月dd日 HH:mm:ss");
            //        WkTUser user = new WkTUser();
            //        user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(rc[6]));
            //        ric.PersonName = user.KuName;
            //        ric.PersonDeptName = user.Kdid.KdName.ToString().Trim();
            //        ric.RichengSub = rc[4].ToString();
            //        ric.ArrangeManId = rc[10].ToString();
            //        ric.TimeTick = rc[3].ToString();
            //        info.Add(ric);
            //    }

            //    RiChengAll l = new RiChengAll();
            //    l.List = info;
            //    res = "成功";
            //    data = JsonTools.ObjectToJson(l);
            //    var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
            //    var result = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            //    };
            //    return result;
            //}
            //else
            //{
            //    res = "没有内容";
            //    data = "1";//JsonTools.ObjectToJson(l);
            //    var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
            //    var result = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            //    };
            //    return result;
            //}
        }

        private string getDostate(string timetick,object obj)
        {
            if (obj.ToString() == "1")
            {
                return obj.ToString();//1

            }
            else
            {

                long t = long.Parse(timetick);
                if (t < DateTime.Now.Ticks)
                {
                    return "0";
                }
                return "2";
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
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[4])).ToString("yyyy年MM月dd日 HH:mm:ss");//分享时间
                    ric.RemindTime = new DateTime(Convert.ToInt64(rc[7])).ToString("yyyy年MM月dd日 HH:mm:ss");//提醒时间
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
                                st.RichengTime = new DateTime(Convert.ToInt64(sf[3].ToString())).ToString("yyyy年MM月dd日 HH:mm:ss");//日程时间
                                st.RichengId = sf[1].ToString(); //日程id
                                st.RemindTime = new DateTime(Convert.ToInt64(sf[9].ToString())).ToString("yyyy年MM月dd日 HH:mm:ss");//提醒时间
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


        public HttpResponseMessage GetChangeDoState([FromUri]string id)
        {

            string res = null;
            string data = null;

            BaseService baseService = new BaseService();

            StaffSchedule s = new StaffSchedule();
            s = (StaffSchedule)baseService.loadEntity(s,long.Parse(id));
            if (s != null && s.Id > 0)
            {
                if (s.ScheduleTime < DateTime.Now.Ticks)
                {
                    res = "已过期无法修改";
                    data = "1";

                }
                else 
                {
                    if (s.DoState != null)
                    {

                        s.DoState = 1;
                        s.RemindTime = DateTime.Now.Ticks;
                        baseService.SaveOrUpdateEntity(s);
                        res = "修改成功";
                        data = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
                    }

                
                }
            }
            else
            {
                res = "出错";
                data = "1";
            }
            data = "1";


            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
             
        
        }

        public class RiChengData
        {
            public RiChengData()
            {

                Sharelist = new List<PersonInfo>();
            
            
            }

            public string Uid { get; set; }

            public string Content { get; set; } //: content,
            public string Title { get; set; }//: title,
            public string Dotime { get; set; }//: ddt,
            public bool Isre { get; set; } //: isc,
            public string Retime { get; set; }//: rdt,
            public List<PersonInfo> Sharelist { get; set; }//: sharePersonlist//sharePersonlist
        }

        public class jd
        {

            public string jsondata { get; set; }
        
        }

        public HttpResponseMessage addRicheng([FromBody]jd jdata)
        {

            BaseService baseService = new BaseService();



            string res = "";

            string json = jdata.jsondata;
            JavaScriptSerializer js = new JavaScriptSerializer();
            RiChengData list = js.Deserialize<RiChengData>(json);

            try
            {

                WkTUser ww = new WkTUser();
                ww = (WkTUser)baseService.loadEntity(ww, long.Parse(list.Uid));

                List<WkTUser> sharedUser = new List<WkTUser>();

                foreach (PersonInfo p in list.Sharelist)
                {

                    WkTUser w = new WkTUser();
                    w = (WkTUser)baseService.loadEntity(w, long.Parse(p.Id));

                    sharedUser.Add(w);
                }

                StaffSchedule staffSchedule = new StaffSchedule();
                staffSchedule.IfRemind = list.Isre ? (int)StaffSchedule.IfRemindEnum.Renmind : (int)StaffSchedule.IfRemindEnum.NotRemind;
                //会议时间
                staffSchedule.ScheduleTime = (DateTime.Parse(list.Dotime)).Ticks; //this.dateTimePicker2.Value.Ticks;//scheduleDate.Date.Ticks + dateTimePicker1.Value.TimeOfDay.Ticks;
                //提醒时间
                staffSchedule.RemindTime = (DateTime.Parse(list.Retime)).Ticks;//this.dateTimePicker1.Value.Ticks;//scheduleDate.Date.Ticks + dateTimePicker2.Value.TimeOfDay.Ticks;
                staffSchedule.Staff = ww;
                staffSchedule.StaffScheduleStaffs = sharedUser;
                staffSchedule.Subject = list.Title;
                staffSchedule.TimeStamp = DateTime.Now.Ticks;
                staffSchedule.State = (int)IEntity.stateEnum.Normal;
                staffSchedule.Content = list.Content;
                staffSchedule.ArrangeMan = ww;//user;

                baseService.SaveOrUpdateEntity(staffSchedule);

                res = "成功";
                string data = "1"; //JsonTools.ObjectToJson(list); //jdata.jsondata;//ss.Content;//JsonTools.ObjectToJson(ss);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;

            }
            catch(Exception ex)
            {
                res = ex.Message;
                string data = "1"; //JsonTools.ObjectToJson(list); //jdata.jsondata;//ss.Content;//JsonTools.ObjectToJson(ss);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            
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
                    ric.RichengTime = new DateTime(Convert.ToInt64(rc[4])).ToString("yyyy年MM月dd日 HH:mm:ss");//分享时间
                    ric.RemindTime = new DateTime(Convert.ToInt64(rc[7])).ToString("yyyy年MM月dd日 HH:mm:ss");//提醒时间
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
