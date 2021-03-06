﻿using Newtonsoft.Json.Linq;
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
using worklogService.RongCloud;
namespace worklogService.Controllers
{
    public class UsersController : ApiController
    {




        #region 更新头像

        public class Body
        {
            string base64str; //{ get; set; }

            public string Base64str
            {
                get { return base64str; }
                set { base64str = value; }
            }
        
        }

        ///更新头像
        public HttpResponseMessage UploadHeadlmg([FromBody]Body base64str,[FromUri]int id)
        {
            string res = "1";

            ImgBase64 base64 = new ImgBase64();

            res = base64.Base64StringToImage(base64str.Base64str, id);




            var jsonStr = "{\"Message\":" + "\"" + res  + "\" }";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
        
        }

        #endregion


        #region 签退
        //签退
        public HttpResponseMessage GetsignExitInfo(int id)
        {


            string res = "";
            string timetxt = "";
            BaseService baseService = new BaseService();
            DateTime today;

            today = DateTime.Now;
            WkTUser user = new WkTUser();
            user = (WkTUser)baseService.loadEntity(user, id);


            if (CNDate.isworkDay(today.Date.Ticks))
            {
                IList attendanceList = baseService.loadEntityList("from Attendance where STATE=" + (int)IEntity.stateEnum.Normal + " and User=" + user.Id + " and SignDate=" + today.Date.Ticks);
                if (attendanceList != null && attendanceList.Count == 1)
                {
                    Attendance todaySignStart = (Attendance)attendanceList[0];
                    IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE=" + (int)IEntity.stateEnum.Normal + " and StartTime<=" + today.Date.Ticks + " order by StartTime desc");
                    if (usuallyDayList != null && usuallyDayList.Count == 1)
                    {
                        UsuallyDay u = (UsuallyDay)usuallyDayList[0];

                        if (u.WorkTimeEnd <= today.TimeOfDay.Ticks)//未早退
                        {
                            if (todaySignStart.LateOrLeaveEarly == (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly)  //登陆为LateAndEarly表示迟到
                            {
                                todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Late; // 只是迟到
                            }
                            else
                            {
                                todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Normal;  //  正常签到
                            }
                        }


                        else //早退
                        {
                            if (todaySignStart.LateOrLeaveEarly == (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly)
                            {
                                todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly; //迟到并且早退
                            }
                            else
                            {
                                todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Early; //只是早退
                            }
                        }

                    }
                    todaySignStart.SignEndTime = today.TimeOfDay.Ticks;
                    todaySignStart.SignDate = today.Date.Ticks;
                    todaySignStart.SignDay = today.Day;
                    todaySignStart.SignMonth = today.Month;
                    todaySignStart.SignYear = today.Year;
                    todaySignStart.State = (int)IEntity.stateEnum.Normal;
                    todaySignStart.TimeStamp = DateTime.Now.Ticks;
                    todaySignStart.User = user;
                    try
                    {
                        baseService.SaveOrUpdateEntity(todaySignStart);
                        res = "签退成功";
                    }
                    catch
                    {
                        res = "签退失败";

                    }
                    timetxt = CNDate.getTimeByTimeTicks(todaySignStart.SignStartTime) + "-" + CNDate.getTimeByTimeTicks(today.TimeOfDay.Ticks);
                }
            }




            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":\"" + timetxt + "\"}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
        }
        #endregion



        #region 设置手机号
        public class PhoneNum
        {
            string phoneNumstring;

            public string PhoneNumstring
            {
                get { return phoneNumstring; }
                set { phoneNumstring = value; }
            }

          
            string perId;

            public string PerId
            {
                get { return perId; }
                set { perId = value; }
            }

        }

        public HttpResponseMessage GetSendPhoneNum([FromUri]string phonenum,[FromUri]string uid)
        {
            BaseService baseService = new BaseService ();

            string res = "";
            string phoneNum = phonenum;
            string perid =uid;
            long id = long.Parse(perid);
            WkTUser w = new WkTUser();
            w = (WkTUser)baseService.loadEntity(w, id);

            w.KuPhone = phoneNum;
            try
            {
                baseService.SaveOrUpdateEntity(w);
                res = "成功";
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }

            
            //string data = JsonTools.ObjectToJson(l);
            string data = "1";
            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":" + data + "}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
        
        }

        #endregion



        #region 获取签到信息 签到
        public HttpResponseMessage GetAttenceInfo(int id)
        {


            BaseService baseService = new BaseService ();
            string res = "";

            string timetxt = "" ;

            WkTUser user = new WkTUser ()  ;
            user = (WkTUser)baseService.loadEntity(user, id);
                   DateTime today;
                   
                   today = DateTime.Now;
                   
 

                    
                    if (CNDate.isworkDay(today.Date.Ticks))//工作日登录
                    {
                        //查询最近的工作起始时间安排
                        IList attendanceList = baseService.loadEntityList("from Attendance where STATE=" + (int)IEntity.stateEnum.Normal + " and User=" + user.Id + " and SignDate=" + today.Date.Ticks);

                        if (attendanceList != null && attendanceList.Count == 1)//今天登录过
                        {
                            res = "今天登录过";
                            Attendance atd = (Attendance)attendanceList[0];
                            timetxt += atd.SignStartTime != 0 ? CNDate.getTimeByTimeTicks(atd.SignStartTime) : "";
                            timetxt += "-";
                            timetxt += atd.SignEndTime != 0 ? CNDate.getTimeByTimeTicks(atd.SignEndTime) : "未签退";
                        }


                        else // 今天没有登陆过
                        {
                            Attendance todaySignStart = new Attendance();//用于记录考勤信息
                            IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE="
                                + (int)IEntity.stateEnum.Normal + " and StartTime<=" + today.Date.Ticks +
                                " order by StartTime desc"); //查询作息时间
                            if (usuallyDayList != null && usuallyDayList.Count == 1) //存在作息时间设置
                            {
                                UsuallyDay u = (UsuallyDay)usuallyDayList[0];
                                if (u.WorkTimeStart >= today.TimeOfDay.Ticks)
                                {
                                    todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Early; // 正常签到
                                }
                                else
                                {
                                    todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly; // 迟到
                                }
                            }
                            todaySignStart.SignStartTime = today.TimeOfDay.Ticks;
                            todaySignStart.SignDate = today.Date.Ticks;
                            todaySignStart.SignDay = today.Day;
                            todaySignStart.SignMonth = today.Month;
                            todaySignStart.SignYear = today.Year;
                            todaySignStart.State = (int)IEntity.stateEnum.Normal;
                            
                                todaySignStart.TimeStamp = DateTime.Now.Ticks;
                            
                            todaySignStart.User = user;
                            try
                            {
                                baseService.SaveOrUpdateEntity(todaySignStart);
                                res = "签到成功";
                            }
                            catch
                            {
                                res = "签到失败";
                                
                            }
                            timetxt = CNDate.getTimeByTimeTicks(todaySignStart.SignStartTime) + "-"+"未签退";
                        }
                    }
                    else
                    {
                        timetxt = "今天是休息日";

                        //this.attendance_label.Text = "今天是休息日";

                    }
                   
        
       
            //string data = JsonTools.ObjectToJson(l);

                    var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":\"" + timetxt + "\"}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
        }

        #endregion
     


        #region 获取部门人员列表
        public class DeptsandPerson
        {
            Dept dept;

            public Dept Dept
            {
                get { return dept; }
                set { dept = value; }
            }
            
            List<PersonInfo> persons;

            public List<PersonInfo> Persons
            {
                get { return persons; }
                set { persons = value; }
            }
            
        }

        public class DeptsandPersonlist
        {
            List<DeptsandPerson> list;

            public List<DeptsandPerson> List
            {
                get { return list; }
                set { list = value; }
            }

        
        }

        public HttpResponseMessage GetAlldeptAndusers()
        {
             BaseService baseservice = new BaseService();
             IList nhbdepts = baseservice.loadEntityList("select u from WkTDept u");
             IList nhbpersons = baseservice.loadEntityList("select u from WkTUser u ");
             List<DeptsandPerson> dplist = new List<DeptsandPerson>();
             
             foreach (WkTDept o in nhbdepts)
             {
                 DeptsandPerson dp = new DeptsandPerson();
                 
                 Dept d = new Dept();

                 d.Id = o.Id.ToString();
                 d.DeptName = o.KdName.Trim();
                 dp.Dept = d;
                 List<PersonInfo> pers = new List<PersonInfo>();

                 foreach (WkTUser n in nhbpersons)
                 {
                     if (n.Kdid.Id == o.Id)
                     {
                         PersonInfo per = new PersonInfo();
                         per.Id = n.Id.ToString();
                         per.PersonName = n.KuName;
                         per.PersonPhone = n.KuPhone;
                         per.MD5code = n.ImgMD5Code;
                         //per.Base64img = n.Base64Img;
                         per.IMToken = n.IMToken;
                         per.PersonDept = d;
                         pers.Add(per);
                     
                     }
                     
                 }
                 dp.Persons = pers;

                 dplist.Add(dp);
                 
             }
             DeptsandPersonlist l = new DeptsandPersonlist();

             l.List = dplist;

           

             
             
             string res = "成功";
             string data = JsonTools.ObjectToJson(l);
             
            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":" + data + "}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;


        }

        #endregion



        #region 登陆获取个人信息
        public class namepwd
        {

            public string  Name {get; set;}
            public string Pwd { get; set;}

        }
       

        public HttpResponseMessage toLoginMessage([FromBody]namepwd nn)
        {


            string res;
            string data = "1";


            string name = nn.Name; string orpwd = nn.Pwd; 
            //string name = "mashi"; string orpwd = "186754";

            BaseService baseservice = new BaseService();
            IList pwd = baseservice.ExecuteSQL("select right(sys.fn_VarBinToHexStr(hashbytes('MD5', '" + orpwd.Trim() + "')),32)"); // 通过数据库加密
            if (pwd == null || pwd.Count <= 0)
            {
                res =  "登录异常！";
            }
            object[] pwdArray = (object[])pwd[0];
            //因为是共用表 选择是工作小秘书相关的角色
            IList userList = baseservice.loadEntityList("select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and u.KuLid='" + name.Trim() + "' and u.KuPassWD='" + pwdArray[0] + "'");
            if (userList == null || userList.Count <= 0)
            {
                res =  "用户名或密码错误！";
            }
            else if (userList.Count > 1)
            {
                res =  "用户异常，请联系管理员！";
            }
            else
            {

                Role role = new Role();
                WkTUser u = (WkTUser)userList[0];
                foreach (WkTRole r in u.UserRole)
                {
                    if (r.KrDESC.Trim().Equals("工作小秘书角色"))//是本系统的用户角色
                    {
                        role.RoleOrder = r.KrOrder.ToString();
                        role.RoleName = r.KrName;
                    }
                }
               

                Dept d = new Dept();
                d.Id = u.Kdid.Id.ToString();
                d.DeptName = u.Kdid.KdName.Trim();

                PersonInfo per = new PersonInfo();
                per.Id = u.Id.ToString();
                per.PersonName = u.KuName;
                per.PersonPhone = u.KuPhone;
                per.PersonDept = d;
                per.PersonRole = role;
                per.PersonAccount = u.KuLid;
                per.IMToken = u.IMToken;
                //if(u.im)
                per.MD5code = u.ImgMD5Code;
                per.Base64img = u.Base64Img;


                
                data =  JsonTools.ObjectToJson(per);
                
                res =  "登录成功";
            }


            var jsonStr = "{\"Message\":" + "\""+res+"\"" + ","+" \"data\":"+ data+"}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
        }

        #endregion

        public HttpResponseMessage GetUserToken(int id)
        {
            string res = "";


            string data = "";



         



            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":" + data + "}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;

        
        }


        public HttpResponseMessage GetUserInfo(string uid)
        {
            string res = "错误";


            string data = "1";

            BaseService baseService = new BaseService();

            string perid = uid;
            long id = long.Parse(perid);
            WkTUser w = new WkTUser();
            w = (WkTUser)baseService.loadEntity(w, id);

            if (w.Id.ToString() == uid)
            {
                res = "成功";

                PersonInfo p = new PersonInfo();
                p.Id = w.Id.ToString();
                p.PersonName = w.KuName;
                if (w.KuPhone != null)
                {
                    p.PersonPhone = w.KuPhone.Trim();

                }
                p.MD5code = w.ImgMD5Code.Trim();


                Dept d = new Dept();
                d.Id = w.Kdid.Id.ToString();
                d.DeptName = w.Kdid.KdName.Trim();
                p.PersonDept = d;
                
                data = JsonTools.ObjectToJson(p);
                
            }

            


            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":" + data + "}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;


        }



        // GET api/users
        public IEnumerable<string> Get()
        {


            //BaseService baseservice = new BaseService();

            //IList i = baseservice.loadEntityList("select u from WkTUser u ");


            //String retstr = null;






            
            
            //foreach (WkTUser o in i)
            //{
            //    if (o.Id != 699 && o.Id != 758)
            //    {

            //        retstr = RongCloudServer.GetToken(RongCloudServer.appKey, 
            //            RongCloudServer.appsecret, 
            //            o.Id.ToString(), 
            //            o.KuLid.Trim(), 
            //            "http://www.qqw21.com/article/UploadPic/2012-11/201211259378560.jpg");

            //        if(retstr != null)
            //        {

            //            JObject obj = JObject.Parse(retstr);
            //            string s = (string)obj["token"];
            //            o.IMToken = s;

            //            baseservice.SaveOrUpdateEntity(o);

            //        }

            //    }

            //    //num++;
            //}
            ////return b;
            return new string[] { "value1", "value2" };
        }
        // GET api/users/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/users
        public void Post([FromBody]string value)
        {

        }

        // PUT api/users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        public void Delete(int id)
        {


        }
    }
}
