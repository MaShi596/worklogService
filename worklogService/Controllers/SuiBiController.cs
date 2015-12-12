using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using worklogService.CommonMethod;
using worklogService.DBoperate;
using worklogService.Models;
using worklogService.Models.CommunicationClass;
using System.Collections;

namespace worklogService.Controllers
{
    public class SuiBiController : ApiController
    {

        public class OwnSuiBi
        {
            int suibiid;
            public int SuiBiId
            {
                get { return suibiid; }
                set { suibiid = value; }

            }

            string contents;
            public string Contents
            {
                get { return contents; }
                set { contents = value; }
            }

            string writetime;
            public string Writetime
            {
                get { return writetime; }
                set { writetime = value; }
            }
            string wktuserid;
            private int SuiBi;

            public string Wktuserid
            {
                get { return wktuserid; }
                set { wktuserid = value; }
            }
        }
        public class OwnSuiBiAll
        {
            List<OwnSuiBi> list;
            private List<OwnSuiBi> suibilist;

            public List<OwnSuiBi> List1
            {
                get { return suibilist; }
                set { suibilist = value; }
            }
        }


        public HttpResponseMessage GetOneSuiBi(string userid,string sbtime)
        //获得个人随笔
        {

            string res = "";
            BaseService baseservice = new BaseService();
            string sqlstr = "";
            if (sbtime == "0")
            {
                //sqlstr = "select row=row_number()over(order by getdate()), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME" +
                //         " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                //          "where dbo.SuiBi.WkTUserId= "+ userid .ToString ()+"   and dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime> "+sbtime .ToString ()+"  order by dbo.SuiBi.WriteTime DESC";
                //Console.WriteLine (sqlstr);

                //sqlstr = "with cte as" +
                //          "(" +
                //          "select top 100 percent row=row_number()over(order by SuiBi.WriteTime()desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME" +
                //          "from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                //          "where dbo.SuiBi.WkTUserId=" + userid.ToString() + " and dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime> " + sbtime.ToString() + " order by dbo.SuiBi.WriteTime DESC" +
                //          ") " +
                //          "select * from cte where row between" + "1" + "and" + "10"  +"order by dbo.SuiBi.WriteTime desc";
                sqlstr= "with cte as"+
                    "(select top 100 percent row=row_number()over(order by(SuiBi.WriteTime)desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME "+
                    " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID "+
                    " where  dbo.SuiBi.STATE= 0 and dbo.SuiBi.WkTUserId="+ userid +" and dbo.SuiBi.WriteTime> "+ sbtime  +
                    ")"+
                    "select * from cte where row >0 and row <11 order by WriteTime desc";
                Console.WriteLine(sqlstr);

                //sqlstr = "with cte as " +
                //           "( " +
                //           " select row=row_number()over(order by getdate()), * from SuiBiV where WkTUserId =  " + userid.ToString() + " and STATE = 0 and WriteTime > " + sbtime.ToString() +
                //           ") " +
                //           " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as" +
                         "(select top 100 percent row=row_number()over(order by(SuiBi.WriteTime)desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME " +
                         " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                         " where  dbo.SuiBi.STATE= 0 and dbo.SuiBi.WkTUserId="+ userid +"and dbo.SuiBi.WriteTime<" + sbtime +
                         ")" +
                         "select * from cte where row >0 and row <11 order by WriteTime desc";
                //sqlstr = "with cte as " +
                //           "( " +
                //           " select row_number()over(order by getdate()), * from SuiBiView where WkTUserId =  " + userid.ToString() + " and STATE = 0 and WriteTime < " + sbtime.ToString() +
                //           ") " +
                //           " select * from cte where row between " + "1" + " and " + "10";
            }
            // sqlstr = "from SuiBi where State=" + (int)IEntity.stateEnum.Normal + "and  WkTUserId=" + userid + " and WriteTime>" + sbtime + "order by WriteTime desc ";
            IList nbhsuibi = baseservice.ExecuteSQL(sqlstr);
            List<SuiBiinfo> suibilist = new System.Collections.Generic.List<SuiBiinfo>();
            if (nbhsuibi != null && nbhsuibi.Count > 0)
            {
                
                
                for (int i=0;i <nbhsuibi.Count ;i++)
                {
                    SuiBiinfo st = new SuiBiinfo();                                        
                    object[] o = (object[])nbhsuibi [i];
                    st.Suibicontent = o[1].ToString ();
                    st.Personid = o[2].ToString();    
                    st.Persondeptname = o[3].ToString().Trim();              
                    st.Writetime = new DateTime(Convert.ToInt64(o[5].ToString())).ToString("yyyy年MM月dd日 HH:mm:ss");
                    st.TimeTick = o[5].ToString();
                    st.Suibiid = o[7].ToString();
                    st.Personname = o[8].ToString (); 
                    suibilist.Add (st);
                }
                //for (int i = 0; i < 9; i++)
                //{
                //    SuiBiinfo st = new SuiBiinfo();
                //    st.Suibitime = new DateTime(((Models.SuiBi)nbhsuibi[i]).WriteTime).ToString("yyyy年MM月dd日 HH:mm");
                //    st.Suibicontentbefor = Htmlsuibibefor(((Models.SuiBi)nbhsuibi[i]).Contents);
                //    st.Suibicontent = ((SuiBi)nbhsuibi[i]).Contents;
                //    st.Suibiid = ((Models.SuiBi)nbhsuibi[i]).Id.ToString();

                //    //List <com > own=new System.Collections.Generic.List<com>() ;

                //}
                worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll all = new worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll();
                all.List = suibilist;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
            else if (nbhsuibi != null && nbhsuibi.Count > 0 && nbhsuibi.Count < 10)
            {
                for (int i = 0; i < nbhsuibi.Count; i++)
                {
                    SuiBiinfo st = new SuiBiinfo();
                    st.Writetime = new DateTime().ToString("yyyy年MM月dd日 HH:mm");
                    st.TimeTick = ((Models.SuiBi)nbhsuibi[i]).WriteTime.ToString();
                    // st.Suibicontentbefor = Htmlsuibibefor(((Models.SuiBi)nbhsuibi[i]).Contents);
                    st.Suibiid = ((Models.SuiBi)nbhsuibi[i]).Id.ToString();
                    suibilist.Add(st);

                }
                worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll all = new worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll();
                all.List = suibilist;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
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
                string data = "";
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }

            /* string res = "";
             BaseService baseservice = new BaseService();
             IList suibitext = baseservice.loadEntityList("from SuiBi where wktuserid= " + userid.ToString() + "");
             List<SuiBiinfo > ownsuibi=new System.Collections.Generic.List<SuiBiinfo>();
             if (ownsuibi != null && ownsuibi.Count > 0)
             {
                 for (int i = 0; i < ownsuibi.Count; i++)
                 {
 
                 }
             }*/
        }
        public HttpResponseMessage GetALLSuiBi(string sbtime)
        //获得所有随笔
        {

            string res = "";
            BaseService baseservice = new BaseService();
            string sqlstr = "";
            if (sbtime == "0")
            {
                //sqlstr = "select row=row_number()over(order by getdate()), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME" +
                //         " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                //          "where dbo.SuiBi.WkTUserId= "+ userid .ToString ()+"   and dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime> "+sbtime .ToString ()+"  order by dbo.SuiBi.WriteTime DESC";
                //Console.WriteLine (sqlstr);

                //sqlstr = "with cte as" +
                //          "(" +
                //          "select top 100 percent row=row_number()over(order by SuiBi.WriteTime()desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME" +
                //          "from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                //          "where dbo.SuiBi.WkTUserId=" + userid.ToString() + " and dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime> " + sbtime.ToString() + " order by dbo.SuiBi.WriteTime DESC" +
                //          ") " +
                //          "select * from cte where row between" + "1" + "and" + "10"  +"order by dbo.SuiBi.WriteTime desc";
                sqlstr = "with cte as" +
                    "(select top 100 percent row=row_number()over(order by(SuiBi.WriteTime)desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME " +
                    " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                    " where  dbo.SuiBi.STATE= 0  and dbo.SuiBi.WriteTime> " + sbtime +
                    ")" +
                    "select * from cte where row >0 and row <11 order by WriteTime desc";
                Console.WriteLine(sqlstr);

                //sqlstr = "with cte as " +
                //           "( " +
                //           " select row=row_number()over(order by getdate()), * from SuiBiV where WkTUserId =  " + userid.ToString() + " and STATE = 0 and WriteTime > " + sbtime.ToString() +
                //           ") " +
                //           " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as" +
                         "(select top 100 percent row=row_number()over(order by(SuiBi.WriteTime)desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME " +
                         " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
                         " where  dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime<" + sbtime +
                         ")" +
                         "select * from cte where row >0 and row <11 order by WriteTime desc";
                //sqlstr = "with cte as " +
                //           "( " +
                //           " select row_number()over(order by getdate()), * from SuiBiView where WkTUserId =  " + userid.ToString() + " and STATE = 0 and WriteTime < " + sbtime.ToString() +
                //           ") " +
                //           " select * from cte where row between " + "1" + " and " + "10";
            }
            IList nbhsuibi = baseservice.ExecuteSQL(sqlstr);
            List<SuiBiinfo> suibilist = new System.Collections.Generic.List<SuiBiinfo>();
            if (nbhsuibi != null && nbhsuibi.Count > 0)
            {


                for (int i = 0; i < nbhsuibi.Count; i++)
                {
                    SuiBiinfo st = new SuiBiinfo();
                    object[] o = (object[])nbhsuibi[i];
                    st.Suibicontent = o[1].ToString();
                    st.Personid = o[2].ToString();
                    st.Persondeptname = o[3].ToString().Trim();
                    st.Writetime = new DateTime(Convert.ToInt64(o[5].ToString())).ToString("yyyy年MM月dd日 HH:mm:ss");
                    st.TimeTick = o[5].ToString();
                    st.Suibiid = o[7].ToString();
                    st.Personname = o[8].ToString();
                    suibilist.Add(st);
                }
                //for (int i = 0; i < 9; i++)
                //{
                //    SuiBiinfo st = new SuiBiinfo();
                //    st.Suibitime = new DateTime(((Models.SuiBi)nbhsuibi[i]).WriteTime).ToString("yyyy年MM月dd日 HH:mm");
                //    st.Suibicontentbefor = Htmlsuibibefor(((Models.SuiBi)nbhsuibi[i]).Contents);
                //    st.Suibicontent = ((SuiBi)nbhsuibi[i]).Contents;
                //    st.Suibiid = ((Models.SuiBi)nbhsuibi[i]).Id.ToString();

                //    //List <com > own=new System.Collections.Generic.List<com>() ;

                //}
                worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll all = new worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll();
                all.List = suibilist;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
            else if (nbhsuibi != null && nbhsuibi.Count > 0 && nbhsuibi.Count < 10)
            {
                for (int i = 0; i < nbhsuibi.Count; i++)
                {
                    SuiBiinfo st = new SuiBiinfo();
                    st.Writetime = new DateTime(((Models.SuiBi)nbhsuibi[i]).WriteTime).ToString("yyyy年MM月dd日 HH:mm:ss");
                    st.TimeTick = ((Models.SuiBi)nbhsuibi[i]).WriteTime.ToString();
                    // st.Suibicontentbefor = Htmlsuibibefor(((Models.SuiBi)nbhsuibi[i]).Contents);
                    st.Suibiid = ((Models.SuiBi)nbhsuibi[i]).Id.ToString();
                    suibilist.Add(st);

                }
                worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll all = new worklogService.Controllers.SuiBiController.SuiBiinfo.SuiBiAll();
                all.List = suibilist;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
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
                string data = "1";
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }

            /* string res = "";
             BaseService baseservice = new BaseService();
             IList suibitext = baseservice.loadEntityList("from SuiBi where wktuserid= " + userid.ToString() + "");
             List<SuiBiinfo > ownsuibi=new System.Collections.Generic.List<SuiBiinfo>();
             if (ownsuibi != null && ownsuibi.Count > 0)
             {
                 for (int i = 0; i < ownsuibi.Count; i++)
                 {
 
                 }
             }*/
        }


        public class SuiBiinfo
        {
            string suibiid;

            public string Suibiid
            {
                get { return suibiid; }
                set { suibiid = value; }
            }
            string suibicontent;

            public string Suibicontent
            {
                get { return suibicontent; }
                set { suibicontent = value; }
            }

            string personname;

            public string Personname
            {
                get { return personname; }
                set { personname = value; }
            }
            string personid;
            public string Personid
            {
                get { return personid; }
                set { personid = value; }
            }
            string persondeptname;
            public string Persondeptname
            {
                get { return persondeptname; }
                set { persondeptname = value; }
            }
            string writetime;

            public string Writetime
            {
                get { return writetime; }
                set { writetime = value; }
            }

            string timeTick;

            public string TimeTick
            {
                get { return timeTick; }
                set { timeTick = value; }
            }


            public class SuiBiAll
            {
                List<SuiBiinfo> list;

                public List<SuiBiinfo> List
                {
                    get { return list; }
                    set { list = value; }
                }
            }
            //public HttpResponseMessage GetSuiBi(string userid,string sbtime)
            //{
            //    string sqlstr = null;
            //    if (sbtime == "0")
            //    {
            //        sqlstr = "with cte as" +
            //                 "(select top 100 percent row=row_number()over(order by(SuiBi.WriteTime)desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME " +
            //                 " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
            //                 " where  dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime> " + sbtime +
            //                 ")" +
            //                 "select * from cte where row >0 and row <11 order by WriteTime ";
            //        //sqlstr = "with cte as " +
            //        //            "( " +
            //        //            " select row=row_number()over(order by WriteTime()), * from SuiBi where  Writetime > " + sbtime.ToString() +
            //        //            ") " +
            //        //            " select * from cte where row between " + "1" + " and " + "10";
            //    }
            //    else
            //    {
            //        sqlstr = "with cte as" +
            //                 "(select top 100 percent row=row_number()over(order by(SuiBi.WriteTime)desc), dbo.SuiBi.Contents, dbo.SuiBi.WkTUserId, dbo.WK_T_DEPT.KD_NAME, dbo.WK_T_USER.KD_ID, dbo.SuiBi.WriteTime, dbo.SuiBi.STATE, dbo.SuiBi.Id, dbo.WK_T_USER.KU_NAME " +
            //                 " from  dbo.SuiBi INNER JOIN dbo.WK_T_USER ON dbo.SuiBi.WkTUserId = dbo.WK_T_USER.KU_ID INNER JOIN dbo.WK_T_DEPT ON dbo.WK_T_USER.KD_ID = dbo.WK_T_DEPT.KD_ID " +
            //                  " where  dbo.SuiBi.STATE= 0 and dbo.SuiBi.WriteTime< " + sbtime +
            //                      ")" +
            //                   "select * from cte where row >0 and row <11 order by WriteTime ";
            //        //sqlstr = "with cte as " +
            //        //            "( " +
            //        //            " select row=row_number()over(order by WriteTime()), * from SuiBi where  Writetime < " + sbtime.ToString() +
            //        //            ") " +
            //        //            " select * from cte where row between " + "1" + " and " + "10";



            //    }
            //    string res = "";
            //    BaseService baseeriver = new BaseService();
            //    IList nbhsuibiallsql = baseeriver.ExecuteSQL(sqlstr);

            //    List<SuiBiinfo> info = new List<SuiBiinfo>();
            //    if (nbhsuibiallsql != null && nbhsuibiallsql.Count > 0)
            //    {
            //        for (int i = 0; i < nbhsuibiallsql.Count; i++)
            //        {
            //            //SuiBiinfo st = new SuiBiinfo();
            //            //object[] o = (object[])nbhsuibi[i];
            //            //st.Suibicontent = o[1].ToString();
            //            //st.Personid = o[2].ToString();
            //            //st.Persondeptname = o[3].ToString().Trim();
            //            //st.Writetime = new DateTime(Convert.ToInt64(o[5].ToString())).ToString("yyyy年MM月dd日 HH:mm");
            //            //st.Suibiid = o[7].ToString();s
            //            //st.Personname = o[8].ToString(); 

            //            object[] sb = (object[])nbhsuibiallsql[i];
            //            SuiBiinfo sub = new SuiBiinfo();

            //            sub.Suibiid = sb[1].ToString();
            //            sub.Personid = sb[4].ToString();
            //            //sub.Personname = sb[4].ToString();
            //            sub.Suibicontent = sb[2].ToString();
            //            // sub.Persondeptname = o.;
            //            sub.Writetime = new DateTime(Convert.ToInt64(sb[3])).ToString("yyyy年MM月dd日 HH:mm");
            //            //sub.Persondeptname = o.WkTUserId.Kdid.KdName.ToString().Trim();
            //            WkTUser user = new WkTUser();
            //            user = (WkTUser)baseeriver.loadEntity(user, Convert.ToInt64(sb[4]));
            //            sub.Personname = user.KuName;
            //            sub.Persondeptname = user.Kdid.KdName.ToString().Trim();
            //            info.Add(sub);
            //        }


            //        //if (nbhsuibiallsql != null && nbhsuibiallsql.Count > 0)
            //        //{
            //        //    List<SuiBiinfo> info = new List<SuiBiinfo>();
            //        //    foreach(SuiBi o in nbhsuibiallsql)
            //        //    { 
            //        //        SuiBiinfo sub = new SuiBiinfo();
            //        //        sub.Suibiid = o.Id.ToString();
            //        //        sub.Personid = o.WkTUserId.Id.ToString();
            //        //        sub.Personname  = o.WkTUserId.KuName.ToString();
            //        //        sub.Suibicontent = o.Contents.ToString();
            //        //       // sub.Persondeptname = o.;
            //        //        sub.Suibitime = new DateTime(o.TimeStamp).ToString("yyyy年MM月dd日 HH:mm");
            //        //        sub.Persondeptname = o.WkTUserId.Kdid.KdName.ToString().Trim();
            //        //        info.Add(sub);

            //        //    }

            //        //for (int x = 0; x < nbhsuibiallsql.Count; x++)
            //        //{
            //        //    SuiBiinfo sub = new SuiBiinfo();
            //        //    sub.Personid = ((SuiBiinfo)nbhsuibiallsql[x]).Personid;
            //        //    sub.Personname = ((SuiBiinfo)nbhsuibiallsql[x]).Personname;
            //        //    sub.Suibicontent = ((SuiBiinfo)nbhsuibiallsql[x]).Suibicontent;
            //        //    sub.Persondeptname = ((SuiBiinfo)nbhsuibiallsql[x]).Persondeptname;
            //        //    sub.Suibitime = ((SuiBiinfo)nbhsuibiallsql[x]).Suibitime;
            //        //}
            //        SuiBiAll all = new SuiBiAll();
            //        all.List = info;
            //        res = "成功";
            //        string data = JsonTools.ObjectToJson(all);
            //        var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
            //        var result = new HttpResponseMessage(HttpStatusCode.OK)
            //        {
            //            Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            //        };
            //        return result;
            //    }
            //    else
            //    {
            //        res = "没有内容";
            //        string data = "";
            //        var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
            //        var result = new HttpResponseMessage(HttpStatusCode.OK)
            //        {
            //            Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            //        };
            //        return result;
            //    }

            //}

        }
    }
}
